using System.Collections;
using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback will let you change the size an element on a target UI Document
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the size an element on a target UI Document")]
	[System.Serializable]
	[FeedbackPath("UI Toolkit/UITK Size")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.UIToolkit")]
	public class FeedbackUIToolkitSize : FeedbackUIToolkitVector2Base
	{
		protected override void SetValue(Vector2 newValue)
		{
			foreach (VisualElement element in _visualElements)
			{
				element.style.width = newValue.x;
				element.style.height = newValue.y;
				HandleMarkDirty(element);
			}
		}

		protected override Vector2 GetInitialValue()
		{
			return new Vector2(_visualElements[0].resolvedStyle.width, _visualElements[0].resolvedStyle.height);
		}
	}
}