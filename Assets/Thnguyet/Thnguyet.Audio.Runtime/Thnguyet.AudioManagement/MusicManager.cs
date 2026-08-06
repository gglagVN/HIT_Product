using System;
using DG.Tweening;
using UnityEngine;

namespace Thnguyet.AudioManagement
{
	// Quản lý nhạc nền trên AudioManager: đổi bài crossfade bằng DOTween, mỗi bài cũ có tween
	// fade-out riêng nên đổi bài liên tiếp không làm sót bài nào kẹt lại ở volume lửng.
	public class MusicManager : IDisposable
	{
		private readonly AudioManager _audioManager;

		private AudioSO _currentAudioSO;

		private int _currentAudioId = -1;

		private Tweener _currentFadeIn;

		private float _masterVolume = 1f;

		/// Volume tổng của nhạc nền (0..1); áp ngay vào bài đang phát và mọi lần fade sau đó.
		public float MasterVolume
		{
			get
			{
				return _masterVolume;
			}
			set
			{
				_masterVolume = Mathf.Clamp01(value);
				if (_currentFadeIn != null && _currentFadeIn.IsActive() && _currentFadeIn.IsPlaying())
				{
					_currentFadeIn.ChangeEndValue(_masterVolume, true);
				}
				else if (_currentAudioId >= 0)
				{
					_audioManager.SetVolume(_currentAudioId, _masterVolume);
				}
			}
		}

		/// AudioSource thật của bài đang phát (cho gameplay cần seek/pitch/đồng bộ nhịp).
		public AudioSource CurrentAudioSource => _audioManager.GetAudioSource(_currentAudioId);

		public MusicManager(AudioManager audioManager)
		{
			_audioManager = audioManager;
		}

		public void Dispose()
		{
			KillFadeIn();
		}

		public void Play(AudioSO audioSO, float fadeDuration)
		{
			if (audioSO == null)
			{
				return;
			}
			if (_currentAudioSO == audioSO)
			{
				AudioSource current = _audioManager.GetAudioSource(_currentAudioId);
				if (current != null && current.isPlaying)
				{
					return;
				}
			}
			FadeOutAndStop(_currentAudioId, fadeDuration);
			_currentAudioSO = audioSO;
			_currentAudioId = _audioManager.Play(audioSO, null);
			KillFadeIn();
			if (_currentAudioId >= 0)
			{
				int id = _currentAudioId;
				_audioManager.SetVolume(id, 0f);
				_currentFadeIn = DOTween
					.To(() => _audioManager.GetVolume(id), v => _audioManager.SetVolume(id, v), _masterVolume, fadeDuration)
					.SetUpdate(true);
			}
		}

		/// Tạm dừng bài đang phát (giữ vị trí); player không bị pool thu hồi nhờ trạng thái Paused.
		public void Pause()
		{
			if (_currentAudioId >= 0) _audioManager.Pause(_currentAudioId);
		}

		/// Phát tiếp bài đang tạm dừng.
		public void Resume()
		{
			if (_currentAudioId >= 0) _audioManager.Resume(_currentAudioId);
		}

		public void Stop(float fadeDuration)
		{
			if (_currentAudioSO == null)
			{
				return;
			}
			KillFadeIn();
			FadeOutAndStop(_currentAudioId, fadeDuration);
			_currentAudioSO = null;
			_currentAudioId = -1;
		}

		private void KillFadeIn()
		{
			if (_currentFadeIn != null)
			{
				_currentFadeIn.Kill();
				_currentFadeIn = null;
			}
		}

		/// Fade bài chỉ định về 0 rồi Stop; tween độc lập nên không bị crossfade sau đó cắt ngang.
		private void FadeOutAndStop(int audioId, float duration)
		{
			if (audioId < 0)
			{
				return;
			}
			DOTween
				.To(() => _audioManager.GetVolume(audioId), v => _audioManager.SetVolume(audioId, v), 0f, duration)
				.SetUpdate(true)
				.OnComplete(() => _audioManager.Stop(audioId));
		}
	}
}
