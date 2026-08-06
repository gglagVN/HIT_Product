using UnityEngine;

namespace Thnguyet.AudioManagement
{
	[CreateAssetMenu(fileName = "NewAudio", menuName = "Audio/New Audio")]
	public class AudioSO : ScriptableObject
	{
		public AudioConfigSO audioConfigSO;

		public bool loop;

		public bool mute;

		[Range(0f, 1f)]
		public float volume = 1f;

		[Range(-3f, 3f)]
		public float pitch = 1f;

		[Range(-1f, 1f)]
		public float panStereo;

		public AudioClipGroup audioClipGroup;

		public void ApplyConfigToSource(AudioSource audioSource)
		{
			if (audioConfigSO != null)
			{
				audioConfigSO.ApplyTo(audioSource);
			}
			audioSource.loop = loop;
			audioSource.mute = mute;
			audioSource.volume = volume;
			audioSource.pitch = pitch;
			audioSource.panStereo = panStereo;
		}

		public AudioClip GetNextClip()
		{
			return audioClipGroup.GetNextClip();
		}

		public AudioSO()
		{
		}
	}
}
