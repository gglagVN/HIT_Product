using System.Collections;
using Thnguyet.GameFeel.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback will let you rotate an element on a target UI Document
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you rotate an element on a target UI Document")]
	[System.Serializable]
	[FeedbackPath("UI Toolkit/UITK Rotate")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.UIToolkit")]
	public class FeedbackUIToolkitRotate : FeedbackUIToolkitFloatBase
	{
		protected StyleRotate _styleRotate;
		
		protected override void SetValue(float newValue)
		{
			foreach (VisualElement element in _visualElements)
			{
				_styleRotate = new Rotate(newValue);
				element.style.rotate = _styleRotate;
				HandleMarkDirty(element);
			}
		}

		protected override float GetInitialValue()
		{
			return _visualElements[0].resolvedStyle.rotate.angle.value;
		}
	}
}