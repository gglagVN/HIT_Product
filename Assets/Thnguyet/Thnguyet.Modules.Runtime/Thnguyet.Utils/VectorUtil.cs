using System;
using UnityEngine;

namespace Thnguyet.Utils
{
	public static class VectorUtil
	{
		public static bool Approximately(Vector2 a, Vector2 b)
		{
			if (Mathf.Approximately(a.x, b.x))
			{
				return Mathf.Approximately(a.y, b.y);
			}
			return false;
		}

		public static bool Approximately(Vector3 a, Vector3 b)
		{
			if (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y))
			{
				return Mathf.Approximately(a.z, b.z);
			}
			return false;
		}

		public static float SqrDistance(Vector2 a, Vector2 b)
		{
			return (float)(Math.Pow(a.x - b.x, 2.0) + Math.Pow(a.y - b.y, 2.0));
		}

		public static float SqrDistance(Vector3 a, Vector3 b)
		{
			return (float)(Math.Pow(a.x - b.x, 2.0) + Math.Pow(a.y - b.y, 2.0) + Math.Pow(a.z - b.z, 2.0));
		}
	}
}
