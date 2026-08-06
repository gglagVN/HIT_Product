using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Thnguyet.AudioManagement
{
	public class AudioMixerManager
	{
		private readonly AudioMixer _audioMixer;

		public AudioMixer AudioMixer
		{
			get
			{
				return _audioMixer;
			}
		}

		public AudioMixerManager(AudioMixer audioMixer)
		{
			if (audioMixer == null)
			{
				throw new ArgumentNullException("audioMixer");
			}
			_audioMixer = audioMixer;
		}

		public float GetMixerParamAsVolume(string name)
		{
			return AudioUtil.DBToNormalized(GetMixerParam(_audioMixer, name));
		}

		public void SetMixerParamAsVolume(string name, float value)
		{
			SetMixerParam(_audioMixer, name, AudioUtil.NormalizedToDB(Mathf.Clamp01(value)));
		}

		public void TransitionTo(string snapshotName, float duration)
		{
			AudioMixerSnapshot snapshot = _audioMixer.FindSnapshot(snapshotName);
			_audioMixer.TransitionToSnapshots(new AudioMixerSnapshot[1] { snapshot }, new float[1] { 1f }, duration);
		}

		private void SetMixerParam(AudioMixer mixer, string paramName, float value)
		{
			if (!mixer.SetFloat(paramName, value))
			{
				Debug.LogError("AudioMixerManager: " + mixer.name + " " + paramName + " not found");
			}
		}

		private float GetMixerParam(AudioMixer mixer, string paramName)
		{
			if (mixer.GetFloat(paramName, out var value))
			{
				return value;
			}
			Debug.LogError("AudioMixerManager: " + mixer.name + " " + paramName + " not found");
			return 0f;
		}
	}
}
