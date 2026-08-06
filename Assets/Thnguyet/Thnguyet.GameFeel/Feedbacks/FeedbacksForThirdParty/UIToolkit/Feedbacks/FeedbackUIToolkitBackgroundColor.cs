using System.Collections;
using Thnguyet.GameFeel.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback will let you change the background color of an element on a target UI Document
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the background color of an element on a target UI Document")]
	[System.Serializable]
	[FeedbackPath("UI Toolkit/UITK Background Color")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.UIToolkit")]
	public class FeedbackUIToolkitBackgroundColor : FeedbackUIToolkitColorBase
	{
		protected override void ApplyColor(Color newColor)
		{
			foreach (VisualElement element in _visualElements)
			{
				element.style.backgroundColor = newColor;
				HandleMarkDirty(element);
			}
		}

		protected override Color GetInitialColor()
		{
			return _visualElements[0].resolvedStyle.backgroundColor;
		}
	}
}