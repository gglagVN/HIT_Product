using UnityEngine;

namespace Thnguyet.AudioManagement
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioPlayer : MonoBehaviour
	{
		public enum Status
		{
			Stopped,
			Playing,
			Paused,
			Destroyed
		}

		private const int STOP_CHECK_FRAME_COUNT = 5;

		public AudioSO audioSO;

		public bool playOnAwake;

		private AudioSource _audioSource;

		private float _volume = 1f;

		private Status _status;

		private int _stopCheckFrameCounter;

		public float Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				_volume = Mathf.Clamp01(value);
				ApplyVolume(_volume);
			}
		}

		public bool Loop
		{
			get
			{
				return _audioSource.loop;
			}
		}

		public Status CurrentStatus
		{
			get
			{
				return _status;
			}
		}

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
			if (_audioSource == null)
			{
				throw new MissingReferenceException("AudioSource");
			}
			if (playOnAwake)
			{
				Play();
			}
		}

		private void OnDestroy()
		{
			_status = Status.Destroyed;
		}

		public void Play()
		{
			if (_status == Status.Destroyed || audioSO == null)
			{
				return;
			}
			Stop();
			AudioClip clip = audioSO.GetNextClip();
			if (clip != null)
			{
				audioSO.ApplyConfigToSource(_audioSource);
				ApplyVolume(_volume);
				_audioSource.time = 0f;
				_audioSource.clip = clip;
				_audioSource.Play();
				_status = Status.Playing;
			}
			else
			{
				Debug.LogError("AudioPlayer: " + audioSO.name + " get null audio clip!");
			}
		}

		public void Stop()
		{
			if (_status != Status.Destroyed)
			{
				_audioSource.Stop();
				_status = Status.Stopped;
			}
		}

		public void Pause()
		{
			if (_status != Status.Destroyed)
			{
				_audioSource.Pause();
				_status = Status.Paused;
			}
		}

		public void Resume()
		{
			if (_status != Status.Destroyed)
			{
				_audioSource.UnPause();
				_status = Status.Playing;
			}
		}

		private void ApplyVolume(float value)
		{
			if (audioSO != null)
			{
				_audioSource.volume = audioSO.volume * value;
			}
		}

		private void Update()
		{
			if (_status == Status.Playing && !_audioSource.isPlaying && !AudioListener.pause)
			{
				_stopCheckFrameCounter++;
				if (_stopCheckFrameCounter > STOP_CHECK_FRAME_COUNT)
				{
					_status = Status.Stopped;
				}
			}
			else
			{
				_stopCheckFrameCounter = 0;
			}
		}

		public AudioPlayer()
		{
		}
	}
}
