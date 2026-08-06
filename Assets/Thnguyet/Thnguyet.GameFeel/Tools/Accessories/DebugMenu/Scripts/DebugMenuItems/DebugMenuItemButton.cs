#if GAMEFEEL_UI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to bind a button to a DebugMenu
	/// </summary>
	public class DebugMenuItemButton : MonoBehaviour
	{
		[Header("Bindings")]
		/// the associated button 
		public Button TargetButton;
		/// the button's text comp
		public Text ButtonText;
		/// the button's background image
		public Image ButtonBg;
		/// the name of the event bound to this button
		public string ButtonEventName = "Button";

		protected bool _listening = false;

		/// <summary>
		/// Triggers a button event using the button's event name
		/// </summary>
		public virtual void TriggerButtonEvent()
		{
			DebugMenuButtonEvent.Trigger(ButtonEventName);
		}

		protected virtual void OnMMDebugMenuButtonEvent(string checkboxEventName, bool active, DebugMenuButtonEvent.EventModes eventMode)
		{
			if ((eventMode == DebugMenuButtonEvent.EventModes.SetButton)
			    && (checkboxEventName == ButtonEventName)
			    && (TargetButton != null))
			{
				TargetButton.interactable = active;
			}
		}

		/// <summary>
		/// Starts listening for events
		/// </summary>
		public virtual void OnEnable()
		{
			if (!_listening)
			{
				_listening = true;
				DebugMenuButtonEvent.Register(OnMMDebugMenuButtonEvent);
			}
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public virtual void OnDestroy()
		{
			_listening = false;
			DebugMenuButtonEvent.Unregister(OnMMDebugMenuButtonEvent);
		}
	}
}
#endif