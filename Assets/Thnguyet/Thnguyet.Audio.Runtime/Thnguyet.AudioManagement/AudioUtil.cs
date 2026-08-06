using UnityEngine;

namespace Thnguyet.AudioManagement
{
	public static class AudioUtil
	{
		public static float NormalizedToDB(float normalizedValue)
		{
			return Mathf.Clamp(Mathf.Log10(normalizedValue) * 20f, -80f, 0f);
		}

		public static float DBToNormalized(float dB)
		{
			return Mathf.Clamp01(Mathf.Pow(10f, dB / 20f));
		}
	}
}
