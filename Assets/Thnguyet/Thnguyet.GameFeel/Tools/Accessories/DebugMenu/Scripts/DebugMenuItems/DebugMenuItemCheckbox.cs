using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if GAMEFEEL_UI
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to bind a checkbox to a DebugMenu
	/// </summary>
	public class DebugMenuItemCheckbox : MonoBehaviour
	{
		[Header("Bindings")]
		/// the switch used to display the checkbox
		public DebugMenuSwitch Switch;
		/// the text used to display the checkbox's text
		public Text SwitchText;
		/// the name of the checkbox event
		public string CheckboxEventName = "Checkbox";

		protected bool _valueSetThisFrame = false;
		protected bool _listening = false;

		/// <summary>
		/// Triggers an event when the checkbox gets pressed
		/// </summary>
		public virtual void TriggerCheckboxEvent()
		{
			if (_valueSetThisFrame)
			{
				_valueSetThisFrame = false;
				return;
			}
			DebugMenuCheckboxEvent.Trigger(CheckboxEventName, Switch.SwitchState, DebugMenuCheckboxEvent.EventModes.FromCheckbox);
		}

		/// <summary>
		/// Triggers an event when the checkbox gets checked and becomes true
		/// </summary>
		public virtual void TriggerCheckboxEventTrue()
		{
			if (_valueSetThisFrame)
			{
				_valueSetThisFrame = false;
				return;
			}
			DebugMenuCheckboxEvent.Trigger(CheckboxEventName, true, DebugMenuCheckboxEvent.EventModes.FromCheckbox);
		}

		/// <summary>
		/// Triggers an event when the checkbox gets unchecked and becomes false
		/// </summary>
		public virtual void TriggerCheckboxEventFalse()
		{
			if (_valueSetThisFrame)
			{
				_valueSetThisFrame = false;
				return;
			}
			DebugMenuCheckboxEvent.Trigger(CheckboxEventName, false, DebugMenuCheckboxEvent.EventModes.FromCheckbox);
		}

		protected virtual void OnMMDebugMenuCheckboxEvent(string checkboxEventName, bool value, DebugMenuCheckboxEvent.EventModes eventMode)
		{
			if ((eventMode == DebugMenuCheckboxEvent.EventModes.SetCheckbox)
			    && (checkboxEventName == CheckboxEventName))
			{
				_valueSetThisFrame = true;
				if (value)
				{
					Switch.SetTrue();
				}
				else
				{
					Switch.SetFalse();
				}
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
				DebugMenuCheckboxEvent.Register(OnMMDebugMenuCheckboxEvent);
			}            
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public virtual void OnDestroy()
		{
			_listening = false;
			DebugMenuCheckboxEvent.Unregister(OnMMDebugMenuCheckboxEvent);
		}
	}
}
#endif