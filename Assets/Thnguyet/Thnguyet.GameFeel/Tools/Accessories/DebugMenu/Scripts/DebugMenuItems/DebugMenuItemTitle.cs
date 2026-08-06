using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GAMEFEEL_UI
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to bind a title item to a DebugMenu
	/// </summary>
	public class DebugMenuItemTitle : MonoBehaviour
	{
		[Header("Bindings")]
		/// the text comp used to display the title
		public Text TitleText;
		/// a line below the title
		public Image TitleLine;
	}
}
#endif