using System;
using UnityEngine;

namespace Thnguyet.AudioManagement
{
	[Serializable]
	public class AudioClipGroup
	{
		public enum SequenceMode
		{
			Random,
			RandomNoImmediateRepeat,
			Sequential
		}

		public SequenceMode sequence = SequenceMode.RandomNoImmediateRepeat;

		public AudioClip[] audioClips;

		private int _lastPlayedIndex = -1;

		public AudioClip GetNextClip()
		{
			if (audioClips == null || audioClips.Length == 0)
			{
				return null;
			}
			if (audioClips.Length == 1)
			{
				return audioClips[0];
			}
			int index;
			if (_lastPlayedIndex == -1)
			{
				index = ((sequence == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length));
			}
			else
			{
				switch (sequence)
				{
				case SequenceMode.Random:
					index = UnityEngine.Random.Range(0, audioClips.Length);
					break;
				case SequenceMode.RandomNoImmediateRepeat:
					index = UnityEngine.Random.Range(0, audioClips.Length);
					if (index == _lastPlayedIndex)
					{
						index = (index + 1) % audioClips.Length;
					}
					break;
				case SequenceMode.Sequential:
					index = (_lastPlayedIndex + 1) % audioClips.Length;
					break;
				default:
					index = -1;
					break;
				}
			}
			_lastPlayedIndex = index;
			return audioClips[index];
		}

		public AudioClipGroup()
		{
		}
	}
}
