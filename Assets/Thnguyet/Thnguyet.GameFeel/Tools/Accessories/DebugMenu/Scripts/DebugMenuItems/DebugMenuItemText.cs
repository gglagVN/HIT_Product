using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GAMEFEEL_UI
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to bind a text item to a DebugMenu
	/// </summary>
	public class DebugMenuItemText : MonoBehaviour
	{
		[Header("Bindings")]
		/// a text comp used to display the text
		[TextArea]
		public Text ContentText;
	}
}
#endif