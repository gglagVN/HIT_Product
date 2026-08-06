using UnityEngine;

namespace Thnguyet.Extensions
{
	public static class VectorExtension
	{
		public static Vector3 NewX(this Vector3 v3, float x)
		{
			return new Vector3(x, v3.y, v3.z);
		}

		public static Vector3 NewY(this Vector3 v3, float y)
		{
			return new Vector3(v3.x, y, v3.z);
		}

		public static Vector3 NewZ(this Vector3 v3, float z)
		{
			return new Vector3(v3.x, v3.y, z);
		}

		public static Vector2 NewX(this Vector2 v2, float x)
		{
			return new Vector2(x, v2.y);
		}

		public static Vector2 NewY(this Vector2 v2, float y)
		{
			return new Vector2(v2.x, y);
		}
	}
}
