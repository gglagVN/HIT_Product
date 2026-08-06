using UnityEngine;
using UnityEngine.Audio;

namespace Thnguyet.AudioManagement
{
	[CreateAssetMenu(fileName = "New AudioConfig", menuName = "Audio/New AudioConfig")]
	public class AudioConfigSO : ScriptableObject
	{
		public enum PriorityLevel
		{
			Highest = 0,
			High = 64,
			Standard = 128,
			Low = 194,
			VeryLow = 256
		}

		public AudioMixerGroup outputAudioMixerGroup;

		public PriorityLevel priority = PriorityLevel.Standard;

		[Range(0f, 1f)]
		[Header("Spatial")]
		public float spatialBlend;

		[Range(0f, 1.1f)]
		public float reverbZoneMix = 1f;

		public AudioRolloffMode rolloffMode;

		[Range(0f, 360f)]
		public int spread;

		[Range(0f, 5f)]
		public float dopplerLevel = 1f;

		public float minDistance = 1f;

		public float maxDistance = 500f;

		[Header("Ignores")]
		public bool bypassEffects;

		public bool bypassListenerEffects;

		public bool bypassReverbZones;

		public bool ignoreListenerVolume;

		public bool ignoreListenerPause;

		private void OnValidate()
		{
			if (maxDistance <= minDistance)
			{
				maxDistance = minDistance + 0.01f;
			}
		}

		public void ApplyTo(AudioSource audioSource)
		{
			audioSource.outputAudioMixerGroup = outputAudioMixerGroup;
			audioSource.priority = (int)priority;
			audioSource.spatialBlend = spatialBlend;
			audioSource.reverbZoneMix = reverbZoneMix;
			audioSource.rolloffMode = rolloffMode;
			audioSource.spread = spread;
			audioSource.dopplerLevel = dopplerLevel;
			audioSource.minDistance = minDistance;
			audioSource.maxDistance = maxDistance;
			audioSource.bypassEffects = bypassEffects;
			audioSource.bypassListenerEffects = bypassListenerEffects;
			audioSource.bypassReverbZones = bypassReverbZones;
			audioSource.ignoreListenerVolume = ignoreListenerVolume;
			audioSource.ignoreListenerPause = ignoreListenerPause;
		}

		public AudioConfigSO()
		{
		}
	}
}
