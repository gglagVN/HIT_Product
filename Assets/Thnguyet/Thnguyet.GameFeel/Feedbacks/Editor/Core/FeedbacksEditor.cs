using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thnguyet.GameFeel;
using UnityEditor;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// A custom editor displaying a foldable list of FeelFeedbacks, a dropdown to add more, as well as test buttons to test your feedbacks at runtime
	/// </summary>
	[CustomEditor(typeof(FeelFeedbacks))]
	public class FeedbacksEditor : Editor
	{
		/// <summary>
		/// Draws the inspector, complete with helpbox, init mode selection, list of feedbacks, feedback selection and test buttons 
		/// </summary>
		public override void OnInspectorGUI()
		{
			
				EditorGUILayout.HelpBox("The FeelFeedbacks component got deprecated with the introduction of the Feedback Player, in v3.0.\n\n" +
				                        "The Feedback Player improves performance, lets you keep runtime changes, and much more! And it works just like FeelFeedbacks. " +
				                        "With the release of v4.0, the FeelFeedbacks is now completely removed from Feel and phased out.\n\n" +
				                        "If you've tried adding this component, maybe you're watching an old tutorial, in that case, fear not, all you're watching is still valid, " +
				                        "just replace FeelFeedbacks with Feedback Player and you'll be good to go! Have fun with Feel!", MessageType.Warning);  
		}
	}
}
