using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// An event fired when a button gets pressed in a DebugMenu
	/// </summary>
	[Serializable]
	public class DButtonPressedEvent : UnityEvent
	{
	}

	/// <summary>
	/// A class used to listen to button events from a DebugMenu
	/// </summary>
	public class DebugMenuButtonEventListener : MonoBehaviour
	{
		[Header("Event")]
		/// the name of the event to listen to
		public string ButtonEventName = "Button";
		/// an event to fire when the event is heard
		public DButtonPressedEvent DEvent;

		[Header("Test")]
		public bool TestValue = true;
		[InspectorButton("TestSetValue")]
		public bool TestSetValueButton;

		/// <summary>
		/// This test methods will send a set event to all buttons bound to the ButtonEventName
		/// </summary>
		protected virtual void TestSetValue()
		{
			DebugMenuButtonEvent.Trigger(ButtonEventName, TestValue, DebugMenuButtonEvent.EventModes.SetButton);
		}

		/// <summary>
		/// When we get a menu button event, we invoke
		/// </summary>
		/// <param name="buttonEventName"></param>
		protected virtual void OnMMDebugMenuButtonEvent(string buttonEventName, bool value, DebugMenuButtonEvent.EventModes eventMode)
		{
			if ((eventMode == DebugMenuButtonEvent.EventModes.FromButton) && (buttonEventName == ButtonEventName))
			{
				if (DEvent != null)
				{
					DEvent.Invoke();
				}
			}
		}

		/// <summary>
		/// Starts listening for events
		/// </summary>
		public virtual void OnEnable()
		{
			DebugMenuButtonEvent.Register(OnMMDebugMenuButtonEvent);
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public virtual void OnDisable()
		{
			DebugMenuButtonEvent.Unregister(OnMMDebugMenuButtonEvent);
		}
	}
}