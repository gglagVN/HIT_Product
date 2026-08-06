#if GAMEFEEL_UI
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to keep track of tabs and their contents in a DebugMenu
	/// </summary>
	public class DebugMenuTabManager : MonoBehaviour
	{
		/// a list of all the tabs under that manager
		public List<DebugMenuTab> Tabs;
		/// a list of all the tabs contents under that manager
		public List<DebugMenuTabContents> TabsContents;

		/// <summary>
		/// Selects a tab, hides the others
		/// </summary>
		/// <param name="selected"></param>
		public virtual void Select(int selected)
		{
			foreach(DebugMenuTab tab in Tabs)
			{
				if (tab.Index != selected)
				{
					tab.Deselect();
				}
			}
			foreach(DebugMenuTabContents contents in TabsContents)
			{
				if (contents.Index == selected)
				{
					contents.gameObject.SetActive(true);
				}
				else
				{
					contents.gameObject.SetActive(false);
				}
			}
		}
	}
}
#endif