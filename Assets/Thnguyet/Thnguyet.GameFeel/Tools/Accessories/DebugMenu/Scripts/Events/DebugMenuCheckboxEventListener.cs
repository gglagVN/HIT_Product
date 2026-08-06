using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Thnguyet.GameFeel
{
	[Serializable]
	public class DCheckboxPressedEvent : UnityEvent<bool> { }
	[Serializable]
	public class DCheckboxTrueEvent : UnityEvent { }
	[Serializable]
	public class DCheckboxFalseEvent : UnityEvent { }

	/// <summary>
	/// A class used to listen to events from a DebugMenu's checkbox
	/// </summary>
	public class DebugMenuCheckboxEventListener : MonoBehaviour
	{
		[Header("Events")]
		/// the name of the event to listen to
		public string CheckboxEventName = "CheckboxEventName";
		/// an event fired when the checkbox gets pressed
		public DCheckboxPressedEvent DPressedEvent;
		/// an event fired when the checkbox is pressed and becomes true/checked
		public DCheckboxTrueEvent DTrueEvent;
		/// an event fired when the checkbox is pressed and becomes false/unchecked
		public DCheckboxFalseEvent DFalseEvent;

		[Header("Test")]
		public bool TestValue = true;
		[InspectorButton("TestSetValue")]
		public bool TestSetValueButton;

		/// <summary>
		/// This test methods will send a set event to all checkboxes bound to the CheckboxEventName
		/// </summary>
		protected virtual void TestSetValue()
		{
			DebugMenuCheckboxEvent.Trigger(CheckboxEventName, TestValue, DebugMenuCheckboxEvent.EventModes.SetCheckbox);
		}

		/// <summary>
		/// When get a checkbox event, we invoke our events if needed
		/// </summary>
		/// <param name="checkboxNameEvent"></param>
		/// <param name="value"></param>
		protected virtual void OnMMDebugMenuCheckboxEvent(string checkboxNameEvent, bool value, DebugMenuCheckboxEvent.EventModes eventMode)
		{
			if ((eventMode == DebugMenuCheckboxEvent.EventModes.FromCheckbox) && (checkboxNameEvent == CheckboxEventName))
			{
				if (DPressedEvent != null)
				{
					DPressedEvent.Invoke(value);
				}

				if (value)
				{
					if (DTrueEvent != null)
					{
						DTrueEvent.Invoke();
					}
				}
				else
				{
					if (DFalseEvent != null)
					{
						DFalseEvent.Invoke();
					}
				}
			}
		}

		/// <summary>
		/// Starts listening for events
		/// </summary>
		public virtual void OnEnable()
		{
			DebugMenuCheckboxEvent.Register(OnMMDebugMenuCheckboxEvent);
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public virtual void OnDisable()
		{
			DebugMenuCheckboxEvent.Unregister(OnMMDebugMenuCheckboxEvent);
		}
	}
}