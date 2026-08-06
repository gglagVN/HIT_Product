using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	[Serializable]
	public struct FeelInterval<T> where T : struct, IComparable
	{
		public enum IntervalType { Inclusive, Exclusive }
		
		/// the lower bound of this interval
		[Tooltip("the lower bound of this interval")]
		public T LowerBound;
		/// the upper bound of this interval
		[Tooltip("the upper bound of this interval")]
		public T UpperBound;
		/// whether to include or exclude the lower bound in the interval
		[Tooltip("whether to include or exclude the lower bound in the interval")]
		public IntervalType LowerBoundIntervalType;
		/// whether to include or exclude the upper bound in the interval
		[Tooltip("whether to include or exclude the upper bound in the interval")]
		public IntervalType UpperBoundIntervalType;
		
		/// <summary>
		/// Creates an interval with the specified bounds 
		/// </summary>
		/// <param name="lowerBound"></param>
		/// <param name="upperBound"></param>
		/// <param name="lowerboundIntervalType"></param>
		/// <param name="upperboundIntervalType"></param>
		public FeelInterval(T lowerBound, T upperBound, IntervalType lowerboundIntervalType = IntervalType.Inclusive, IntervalType upperboundIntervalType = IntervalType.Inclusive) : this()
		{
			T a = lowerBound;
			T b = upperBound;
			int comparison = a.CompareTo(b);

			if (comparison > 0)
			{
				a = upperBound;
				b = lowerBound;
			}

			LowerBound = a;
			UpperBound = b;
			LowerBoundIntervalType = lowerboundIntervalType;
			UpperBoundIntervalType = upperboundIntervalType;
		}
		
		/// <summary>
		/// Returns true if the interval contains the specified value 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(T value)
		{
			bool lowerBoundCheck = LowerBoundIntervalType == IntervalType.Exclusive ? LowerBound.CompareTo(value) < 0 : LowerBound.CompareTo(value) <= 0;
			bool upperBoundCheck = UpperBoundIntervalType == IntervalType.Exclusive ? UpperBound.CompareTo(value) > 0 : UpperBound.CompareTo(value) >= 0;
			
			return lowerBoundCheck && upperBoundCheck;
		}
	}
}

