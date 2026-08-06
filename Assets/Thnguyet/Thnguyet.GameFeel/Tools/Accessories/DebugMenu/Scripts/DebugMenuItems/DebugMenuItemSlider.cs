using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if GAMEFEEL_UI
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to bind a slider to a DebugMenu
	/// </summary>
	public class DebugMenuItemSlider : MonoBehaviour
	{
		/// the possible modes this slider can operate on
		public enum Modes { Float, Int }

		[Header("Bindings")]
		/// the selected mode for this slider
		public Modes Mode = Modes.Float;
		/// the Slider to use to change the value
		public Slider TargetSlider;
		/// the text comp used to display the slider's name
		public Text SliderText;
		/// the text comp used to display the slider's value
		public Text SliderValueText;
		/// the target knob
		public Image SliderKnob;
		/// the line behind the knob
		public Image SliderLine;
		/// the value to remap the slider's 0 to
		public float RemapZero = 0f;
		/// the value to remap the slider's 1 to
		public float RemapOne = 1f;
		/// the name of the event bound to this slider
		public string SliderEventName = "Checkbox";

		/// the current slider value
		[FeelReadOnly]
		public float SliderValue;
		/// the current slider int value
		[FeelReadOnly]
		public int SliderValueInt;

		protected bool _valueSetThisFrame = false;
		protected bool _listening = false;
        
		/// <summary>
		/// On Awake we start listening for slider changes 
		/// </summary>
		protected virtual void Awake()
		{
			TargetSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
		}

		/// <summary>
		/// Invoked when the slider value changes
		/// </summary>
		public void ValueChangeCheck()
		{
			if (_valueSetThisFrame)
			{
				_valueSetThisFrame = false;
				return;
			}

			bool valueChanged = true;

			SliderValue = FeelMaths.Remap(TargetSlider.value, 0f, 1f, RemapZero, RemapOne);

			if (Mode == Modes.Int)
			{
				SliderValue = Mathf.Round(SliderValue);
				if (SliderValue == SliderValueInt)
				{
					valueChanged = false;
				}
				SliderValueInt = (int)SliderValue;
			}

			if (valueChanged)
			{
				UpdateValue(SliderValue);
			}

			TriggerSliderEvent(SliderValue);
		}

		protected virtual void UpdateValue(float newValue)
		{
			SliderValueText.text = (Mode == Modes.Int) ? newValue.ToString() : newValue.ToString("F3");
		}

		/// <summary>
		/// Triggers a slider event
		/// </summary>
		/// <param name="value"></param>
		protected virtual void TriggerSliderEvent(float value)
		{
			DebugMenuSliderEvent.Trigger(SliderEventName, value, DebugMenuSliderEvent.EventModes.FromSlider);
		}

		/// <summary>
		/// When we get a set slider event, we set our value
		/// </summary>
		/// <param name="sliderEventName"></param>
		/// <param name="value"></param>
		protected virtual void OnMMDebugMenuSliderEvent(string sliderEventName, float value, DebugMenuSliderEvent.EventModes eventMode)
		{
			if ((eventMode == DebugMenuSliderEvent.EventModes.SetSlider)
			    && (sliderEventName == SliderEventName))
			{
				_valueSetThisFrame = true;
				TargetSlider.value = FeelMaths.Remap(value, RemapZero, RemapOne, 0f, 1f);
				UpdateValue(value);
			}
		}

		/// <summary>
		/// Starts listening for events
		/// </summary>
		public virtual void OnEnable()
		{
			if (!_listening)
			{
				DebugMenuSliderEvent.Register(OnMMDebugMenuSliderEvent);
				_listening = true;
			}            
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public virtual void OnDestroy()
		{
			_listening = false;
			DebugMenuSliderEvent.Unregister(OnMMDebugMenuSliderEvent);
		}
	}
}
#endif
