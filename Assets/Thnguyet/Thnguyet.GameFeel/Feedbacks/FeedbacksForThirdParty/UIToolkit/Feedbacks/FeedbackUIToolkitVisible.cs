using System.Collections;
using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback will let you set the visibility of an element on a target UI Document
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you set the visibility of an element on a target UI Document")]
	[System.Serializable]
	[FeedbackPath("UI Toolkit/UITK Visible")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.UIToolkit")]
	public class FeedbackUIToolkitVisible : FeedbackUIToolkitBoolBase
	{
		public enum Modes { Set, Toggle }
		
		[Header("Visible")]
		/// the selected mode (set : sets the object visible or not, toggle : toggles the object's visibility)
		[Tooltip("the selected mode (set : sets the object visible or not, toggle : toggles the object's visibility)")]
		public Modes Mode = Modes.Set;
		/// whether to set the object visible (true) or not
		[Tooltip("whether to set the object visible (true) or not")]
		[FeedbackEnumCondition("Mode", (int)Modes.Set)]
		public bool Visible = false;
		
		protected override void SetValue()
		{
			foreach (VisualElement element in _visualElements)
			{
				switch (Mode)
				{
					case Modes.Set:
						element.visible = Visible;
						break;
					case Modes.Toggle:
						element.visible = !element.visible;
						break;
				}
				HandleMarkDirty(element);
			}
		}
		
		protected override void SetValue(bool newValue)
		{
			foreach (VisualElement element in _visualElements)
			{
				element.visible = newValue;
				HandleMarkDirty(element);
			}
		}

		protected override bool GetInitialValue()
		{
			return _visualElements[0].visible;
		}
	}
}