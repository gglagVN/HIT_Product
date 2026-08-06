using UnityEngine;
using System.Collections;

namespace Thnguyet.GameFeel
{	
	/// <summary>
	/// Image helpers
	/// </summary>

	public class FeelImage  
	{
		/// <summary>
		/// Coroutine used to make the character's sprite flicker (when hurt for example).
		/// </summary>
		public static IEnumerator Flicker(Renderer renderer, Color initialColor, Color flickerColor, float flickerSpeed, float flickerDuration)
		{
			if (renderer==null)
			{
				yield break;
			}

			if (!renderer.material.HasProperty("_Color"))
			{
				yield break;
			}

			if (initialColor == flickerColor)
			{
				yield break;
			}

			float flickerStop = Time.time + flickerDuration;

			while (Time.time<flickerStop)
			{
				renderer.material.color = flickerColor;
				yield return FeelCoroutine.WaitFor(flickerSpeed);
				renderer.material.color = initialColor;
				yield return FeelCoroutine.WaitFor(flickerSpeed);
			}

			renderer.material.color = initialColor;        
		}
	}
}