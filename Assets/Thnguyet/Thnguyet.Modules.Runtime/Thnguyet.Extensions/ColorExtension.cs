using UnityEngine;

namespace Thnguyet.Extensions
{
	public static class ColorExtension
	{
		public static Color NewR(this Color color, float r)
		{
			return new Color(r, color.g, color.b, color.a);
		}

		public static Color NewG(this Color color, float g)
		{
			return new Color(color.r, g, color.b, color.a);
		}

		public static Color NewB(this Color color, float b)
		{
			return new Color(color.r, color.g, b, color.a);
		}

		public static Color NewA(this Color color, float a)
		{
			return new Color(color.r, color.g, color.b, a);
		}
	}
}
