#if GAMEFEEL_UI
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Thnguyet.GameFeel;
using UnityEngine.EventSystems;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Add this helper to an object and focus will be set to it on Enable
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/GUI/Get Focus On Enable")]
	public class GetFocusOnEnable : MonoBehaviour
	{
		protected virtual void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(this.gameObject, null);
		}
	}
}
#endif