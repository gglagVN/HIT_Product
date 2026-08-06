using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Thnguyet.GameFeel
{
	[Serializable]
	public class DSliderValueChangedEvent : UnityEvent<float> { }

	/// <summary>
	/// A class used to listen to slider events from a DebugMenu
	/// </summary>
	public class DebugMenuSliderEventListener : MonoBehaviour
	{
		[Header("Events")]
		/// the name of the slider event to listen to
		public string SliderEventName = "SliderEventName";
		/// an event fired when the slider's value changes
		public DSliderValueChangedEvent DValueChangedEvent;

		[Header("Test")]
		[Range(0f, 1f)]
		public float TestValue = 1f;
		[InspectorButton("TestSetValue")]
		public bool TestSetValueButton;

		/// <summary>
		/// This test methods will send a set event to all sliders bound to the SliderEventName
		/// </summary>
		protected virtual void TestSetValue()
		{
			DebugMenuSliderEvent.Trigger(SliderEventName, TestValue, DebugMenuSliderEvent.EventModes.SetSlider);
		}

		/// <summary>
		/// When we get a slider event, we trigger an event if needed 
		/// </summary>
		/// <param name="sliderEventName"></param>
		/// <param name="value"></param>
		protected virtual void OnMMDebugMenuSliderEvent(string sliderEventName, float value, DebugMenuSliderEvent.EventModes eventMode)
		{
			if ( (eventMode == DebugMenuSliderEvent.EventModes.FromSlider) 
			     && (sliderEventName == SliderEventName))
			{
				if (DValueChangedEvent != null)
				{
					DValueChangedEvent.Invoke(value);
				}
			}
		}

		/// <summary>
		/// Starts listening for events
		/// </summary>
		public virtual void OnEnable()
		{
			DebugMenuSliderEvent.Register(OnMMDebugMenuSliderEvent);
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public virtual void OnDisable()
		{
			DebugMenuSliderEvent.Unregister(OnMMDebugMenuSliderEvent);
		}
	}
}