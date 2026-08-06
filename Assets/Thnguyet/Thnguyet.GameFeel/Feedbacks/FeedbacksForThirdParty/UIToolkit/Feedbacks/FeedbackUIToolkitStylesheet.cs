using System.Collections;
using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback will let you change the stylesheet on a target UI Document
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the stylesheet on a target UI Document")]
	[System.Serializable]
	[FeedbackPath("UI Toolkit/UITK Stylesheet")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.UIToolkit")]
	public class FeedbackUIToolkitStylesheet : FeedbackUIToolkit
	{
		[Header("Stylesheet")] 
		/// the new stylesheet to apply to the document
		[Tooltip("the new stylesheet to apply to the document")]
		public StyleSheet NewStylesheet;
		
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			foreach (VisualElement element in _visualElements)
			{
				element.styleSheets.Add(NewStylesheet);
				HandleMarkDirty(element);
			}
		}
	}
}