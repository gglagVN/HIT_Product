using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if GAMEFEEL_UI
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Add this class to a UI slider and it'll let you trigger events when the cursor moves past a certain step
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/GUI/Slider Step")]
	[RequireComponent(typeof(Slider))]
	public class SliderStep : MonoBehaviour
	{
		[Header("Slider Step")]
		/// the threshold to trigger steps at
		public float StepThreshold = 0.1f;
		/// the event to trigger when a step is met
		public UnityEvent OnStep;

		protected Slider _slider;
		protected float _lastStep = 0f;

		/// <summary>
		/// On enable, starts listening for value change events
		/// </summary>
		protected virtual void OnEnable()
		{
			_slider = this.gameObject.GetComponent<Slider>();
			_slider.onValueChanged.AddListener(ValueChangeCheck);
		}

		/// <summary>
		/// On disable, stops listening for value change events
		/// </summary>
		protected virtual void OnDisable()
		{
			_slider.onValueChanged.RemoveListener(ValueChangeCheck);
		}

		/// <summary>
		/// when a value change is met, we trigger an event
		/// </summary>
		/// <param name="value"></param>
		public virtual void ValueChangeCheck(float value)
		{
			if (Mathf.Abs(_slider.value - _lastStep) > StepThreshold)
			{
				_lastStep = _slider.value;
				OnStep?.Invoke();
			}
		}
	}
}
#endif