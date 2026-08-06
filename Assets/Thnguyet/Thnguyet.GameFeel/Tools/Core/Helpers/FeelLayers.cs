using UnityEngine;
using System.Collections;

namespace Thnguyet.GameFeel
{	
	public class FeelLayers  
	{
		public static bool LayerInLayerMask(int layer, LayerMask layerMask)
		{
			if(((1 << layer) & layerMask) != 0)	
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
	}
}
