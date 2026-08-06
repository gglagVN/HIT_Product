using System.Collections;
using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback will let you change the font size of an element on a target UI Document
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the font size of an element on a target UI Document")]
	[System.Serializable]
	[FeedbackPath("UI Toolkit/UITK Font Size")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.UIToolkit")]
	public class FeedbackUIToolkitFontSize : FeedbackUIToolkitFloatBase
	{
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement element in _visualElements)
			{
				int newSize = Mathf.FloorToInt(newValue);
				element.style.fontSize = newSize; 
				HandleMarkDirty(element);
			}
		}

		protected override float GetInitialValue()
		{
			return _visualElements[0].resolvedStyle.fontSize;
		}
	}
}