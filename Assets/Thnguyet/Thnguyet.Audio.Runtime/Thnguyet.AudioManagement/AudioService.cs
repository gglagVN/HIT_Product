using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.AudioManagement
{
	// Cửa phát âm thanh duy nhất của game: SFX qua AudioManager (pool AudioPlayer),
	// nhạc nền qua MusicManager (crossfade). Volume Sfx/Music lưu PlayerPrefs.
	// Đặt sẵn GameObject "[AudioService]" kèm component này (gán playerPrefab) vào scene boot.
	/// <summary>
	/// Cửa vào duy nhất của hệ âm thanh: SFX, nhạc nền và âm lượng.
	/// </summary>
	/// <remarks>
	/// Kế thừa <see cref="SceneSingleton{T}"/> nên PHẢI đặt sẵn GameObject "[AudioService]" kèm component này trong scene boot
	/// và gán <c>playerPrefab</c>; thiếu thì <c>AudioService.Instance</c> trả null kèm log lỗi (không tự tạo).
	/// Object được DontDestroyOnLoad, sống xuyên scene. Âm lượng lưu PlayerPrefs theo khoá "sfx_volume"/"music_volume".
	/// </remarks>
	public class AudioService : SceneSingleton<AudioService>
	{
		public const string SfxVolumeKey = "sfx_volume";
		public const string MusicVolumeKey = "music_volume";
		private const float DefaultMusicFade = 0.5f;

		[SerializeField] private AudioPlayer playerPrefab;
		[SerializeField] private AudioConfigSO sfxConfig;
		[SerializeField] private AudioConfigSO musicConfig;
		[SerializeField] [Range(0f, 1f)] private float defaultSfxVolume = 1f;
		[SerializeField] [Range(0f, 1f)] private float defaultMusicVolume = 1f;
		[SerializeField] private float defaultSfxCooldown = 0.05f;

		private AudioManager _audioManager;
		private MusicManager _musicManager;
		private float _sfxVolume;
		private float _musicVolume;
		private readonly Dictionary<AudioClip, AudioSO> _sfxWrappers = new Dictionary<AudioClip, AudioSO>();
		private readonly Dictionary<AudioClip, AudioSO> _musicWrappers = new Dictionary<AudioClip, AudioSO>();
		private readonly Dictionary<AudioClip, float> _lastPlayTimes = new Dictionary<AudioClip, float>();

		/// Bộ phát SFX cấp thấp (pool AudioPlayer) — chỉ dùng khi cần API mà PlaySfx không phủ; null trước khi Awake chạy.
		public AudioManager Audio => _audioManager;

		/// Bộ phát nhạc nền cấp thấp (crossfade) — chỉ dùng khi cần API mà PlayMusic không phủ; null trước khi Awake chạy.
		public MusicManager Music => _musicManager;

		/// AudioSource thật của bài nhạc đang phát (gameplay rhythm cần seek/pitch/đồng bộ nhịp).
		public AudioSource MusicSource => _musicManager != null ? _musicManager.CurrentAudioSource : null;

		/// Âm lượng SFX 0..1, nhân vào mọi lần PlaySfx. Set sẽ clamp và ghi PlayerPrefs ngay (có Save, tránh set mỗi khung hình).
		public float SfxVolume
		{
			get { return _sfxVolume; }
			set
			{
				_sfxVolume = Mathf.Clamp01(value);
				PlayerPrefs.SetFloat(SfxVolumeKey, _sfxVolume);
				PlayerPrefs.Save();
			}
		}

		/// Âm lượng nhạc nền 0..1, áp ngay vào bài đang phát. Set sẽ clamp và ghi PlayerPrefs ngay (có Save, tránh set mỗi khung hình).
		public float MusicVolume
		{
			get { return _musicVolume; }
			set
			{
				_musicVolume = Mathf.Clamp01(value);
				PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolume);
				PlayerPrefs.Save();
				if (_musicManager != null) _musicManager.MasterVolume = _musicVolume;
			}
		}

		/// Nhạc đang bật hay không — suy ra từ MusicVolume &gt; 0.01, không phải cờ riêng.
		public bool IsMusicEnabled => _musicVolume > 0.01f;

		protected override void OnAwake()
		{
			AudioManager.PlayerPrefab = playerPrefab;
			_audioManager = new AudioManager(16);
			_musicManager = new MusicManager(_audioManager);
			_sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, defaultSfxVolume);
			_musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultMusicVolume);
			_musicManager.MasterVolume = _musicVolume;
		}

		private void Update()
		{
			if (_audioManager != null) _audioManager.Update();
		}

		private void OnDestroy()
		{
			if (_musicManager != null) { _musicManager.Dispose(); _musicManager = null; }
			if (_audioManager != null) { _audioManager.Dispose(); _audioManager = null; }
		}

		/// Phát SFX từ AudioSO (asset chuẩn của framework).
		/// Trả id để dừng/chỉnh sau qua <see cref="Audio"/>; trả -1 khi không phát được (audioSO null hoặc service chưa Awake).
		public int PlaySfx(AudioSO audioSO, float volume = 1f)
		{
			if (audioSO == null || _audioManager == null) return -1;
			int id = _audioManager.Play(audioSO, null);
			_audioManager.SetVolume(id, volume * _sfxVolume);
			return id;
		}

		/// Phát SFX từ AudioClip trần, có cooldown chống spam cùng một clip
		/// (cooldownSec < 0 dùng mặc định 0.05s, = 0 tắt cooldown).
		/// Trả -1 khi clip null HOẶC khi bị cooldown chặn — đừng coi -1 là lỗi.
		public int PlaySfx(AudioClip clip, float volume = 1f, float cooldownSec = -1f)
		{
			if (clip == null || _audioManager == null) return -1;

			float cooldown = cooldownSec < 0f ? defaultSfxCooldown : cooldownSec;
			if (cooldown > 0f)
			{
				if (_lastPlayTimes.TryGetValue(clip, out float lastTime) && Time.unscaledTime - lastTime < cooldown)
				{
					return -1;
				}
				_lastPlayTimes[clip] = Time.unscaledTime;
			}

			return PlaySfx(GetWrapper(clip, loop: false, sfxConfig, _sfxWrappers), volume);
		}

		/// Phát nhạc nền từ AudioSO, crossfade với bài đang phát.
		public void PlayMusic(AudioSO audioSO, float fadeDuration = DefaultMusicFade)
		{
			if (_musicManager == null) return;
			_musicManager.Play(audioSO, fadeDuration);
		}

		/// Phát nhạc nền từ AudioClip trần (nhạc level load lúc runtime); trả về AudioSource thật.
		public AudioSource PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = DefaultMusicFade)
		{
			if (clip == null || _musicManager == null) return null;
			AudioSO wrapper = GetWrapper(clip, loop, musicConfig, _musicWrappers);
			wrapper.loop = loop;
			_musicManager.Play(wrapper, fadeDuration);
			return _musicManager.CurrentAudioSource;
		}

		/// Dừng hẳn nhạc nền với fade out; vị trí phát bị mất, muốn phát tiếp phải PlayMusic lại từ đầu.
		public void StopMusic(float fadeDuration = DefaultMusicFade)
		{
			if (_musicManager != null) _musicManager.Stop(fadeDuration);
		}

		/// Tạm dừng nhạc nền giữ nguyên vị trí (gameplay rewind/freeze); phát tiếp bằng ResumeMusic.
		public void PauseMusic()
		{
			if (_musicManager != null) _musicManager.Pause();
		}

		/// Phát tiếp bài đã PauseMusic từ đúng vị trí cũ.
		public void ResumeMusic()
		{
			if (_musicManager != null) _musicManager.Resume();
		}

		/// Bật/tắt nhạc cho nút setting: tắt là đặt MusicVolume = 0, bật là trả lại mức cũ (hoặc mức mặc định nếu mức cũ đã là 0).
		public void SetMusicEnabled(bool enabled)
		{
			MusicVolume = enabled
				? (_musicVolume > 0.01f ? _musicVolume : defaultMusicVolume)
				: 0f;
		}

		/// Bọc AudioClip trần thành AudioSO runtime, cache tách riêng SFX/nhạc để cờ loop và
		/// config không ghi đè chéo khi cùng một clip được dùng cho cả hai đường.
		private static AudioSO GetWrapper(AudioClip clip, bool loop, AudioConfigSO config, Dictionary<AudioClip, AudioSO> cache)
		{
			if (cache.TryGetValue(clip, out AudioSO wrapper) && wrapper != null)
			{
				wrapper.loop = loop;
				return wrapper;
			}

			wrapper = ScriptableObject.CreateInstance<AudioSO>();
			wrapper.name = clip.name;
			wrapper.loop = loop;
			wrapper.audioConfigSO = config;
			wrapper.audioClipGroup = new AudioClipGroup
			{
				sequence = AudioClipGroup.SequenceMode.Sequential,
				audioClips = new[] { clip }
			};
			cache[clip] = wrapper;
			return wrapper;
		}
	}
}
