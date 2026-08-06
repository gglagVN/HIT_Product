using UnityEngine;
using System.Collections.Generic;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// List extensions
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Returns a random element from the list
		/// </summary>
		/// <param name="list"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T FeelRandom<T>(this IList<T> list)
		{
			return list[Random.Range(0, list.Count)];
		}
		
		/// <summary>
		/// Swaps two items in a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="i"></param>
		/// <param name="j"></param>
		public static void Swap<T>(this IList<T> list, int i, int j)
		{
			T temporary = list[i];
			list[i] = list[j];
			list[j] = temporary;
		}

		/// <summary>
		/// Shuffles a list randomly
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list.Swap(i, Random.Range(i, list.Count));
			}                
		}
	}
}