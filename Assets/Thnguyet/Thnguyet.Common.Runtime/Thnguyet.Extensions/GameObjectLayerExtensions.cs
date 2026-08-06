using UnityEngine;

namespace Thnguyet.Extensions
{
	public static class GameObjectLayerExtensions
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
