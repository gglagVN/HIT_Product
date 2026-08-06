#if GAMEFEEL_UI
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to describe tab contents
	/// </summary>
	public class DebugMenuTabContents : MonoBehaviour
	{
		/// the index of the tab, setup by DebugMenu
		public int Index = 0;
		/// the parent of the tab, setup by DebugMenu
		public Transform Parent;
		/// if this is true, scale will be forced to one on init
		public bool ForceScaleOne = true;

		/// <summary>
		/// On Start we initialize this tab contents
		/// </summary>
		protected virtual void Start()
		{
			Initialization();
		}

		/// <summary>
		/// On init we force the scale to one
		/// </summary>
		protected virtual void Initialization()
		{
			if (ForceScaleOne)
			{
				this.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
			}            
		}
	}
}
#endif