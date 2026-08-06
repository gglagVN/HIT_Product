using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Thnguyet.Utils
{
	public static class RandomUtil
	{
		/// Tra ve mot phan tu ngau nhien trong danh sach.
		public static T Select<T>(IReadOnlyList<T> array)
		{
			if (array == null || array.Count == 0)
			{
				throw new ArgumentException("null or empty", "array");
			}
			return array[UnityEngine.Random.Range(0, array.Count)];
		}

		/// Tra ve numRequired phan tu khac nhau lay ngau nhien tu danh sach, khong lap lai phan tu.
		public static T[] Select<T>(IReadOnlyList<T> array, int numRequired)
		{
			if (array == null || array.Count == 0)
			{
				throw new ArgumentException("null or empty", "array");
			}
			if (numRequired < 0 || numRequired > array.Count)
			{
				throw new ArgumentOutOfRangeException("numRequired");
			}
			T[] pool = array.ToArray();
			for (int i = 0; i < numRequired; i++)
			{
				int picked = UnityEngine.Random.Range(i, pool.Length);
				T temp = pool[i];
				pool[i] = pool[picked];
				pool[picked] = temp;
			}
			T[] result = new T[numRequired];
			Array.Copy(pool, result, numRequired);
			return result;
		}

		/// Chon mot chi so theo trong so, trong so cang lon cang de duoc chon.
		public static int SelectWeighted(IReadOnlyList<float> weights)
		{
			if (weights == null || weights.Count <= 0)
			{
				throw new ArgumentException("null or empty", "weights");
			}
			float value = UnityEngine.Random.Range(0f, weights.Sum());
			for (int i = 0; i < weights.Count; i++)
			{
				if (value < weights[i])
				{
					return i;
				}
				value -= weights[i];
			}
			return weights.Count - 1;
		}

		/// Xao tron danh sach tai cho theo Fisher-Yates.
		public static void Shuffle<T>(IList<T> array)
		{
			if (array == null)
			{
				throw new ArgumentException("null", "array");
			}
			for (int i = array.Count - 1; i > 0; i--)
			{
				int picked = UnityEngine.Random.Range(0, i + 1);
				T temp = array[i];
				array[i] = array[picked];
				array[picked] = temp;
			}
		}
	}
}
