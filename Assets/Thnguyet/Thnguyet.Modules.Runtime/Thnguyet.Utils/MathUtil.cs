using System;
using UnityEngine;

namespace Thnguyet.Utils
{
	public static class MathUtil
	{
		public static bool Eq(double a, double b)
		{
			return Math.Abs(a - b) < double.Epsilon;
		}

		public static bool Ne(double a, double b)
		{
			return !Eq(a, b);
		}

		public static bool Eq(float a, float b)
		{
			return Mathf.Approximately(a, b);
		}

		public static bool Ne(float a, float b)
		{
			return !Eq(a, b);
		}

		public static bool Gt(float a, float b)
		{
			return a > b && !Eq(a, b);
		}

		public static bool Lt(float a, float b)
		{
			return a < b && !Eq(a, b);
		}

		public static bool Ge(float a, float b)
		{
			return a > b || Eq(a, b);
		}

		public static bool Le(float a, float b)
		{
			return a < b || Eq(a, b);
		}

		public static int Gcd(int a, int b)
		{
			if (a < 1 || b < 1)
			{
				return 0;
			}
			while (b != 0)
			{
				int num = b;
				b = a % b;
				a = num;
			}
			return a;
		}

		public static double Clamp(double value, double min, double max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public static float Remap(float val, float in1, float in2, float out1, float out2)
		{
			return (val - in1) / (in2 - in1) * (out2 - out1) + out1;
		}
	}
}
