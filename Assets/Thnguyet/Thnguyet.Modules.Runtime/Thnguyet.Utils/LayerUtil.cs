using UnityEngine;

namespace Thnguyet.Utils
{
	public static class LayerUtil
	{
		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			foreach (Transform item in gameObject.transform)
			{
				item.gameObject.SetLayerRecursively(layer);
			}
		}
	}
}
