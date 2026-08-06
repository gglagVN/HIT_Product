using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thnguyet.GameFeel;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback will trigger a FeelGameEvent of the specified name when played
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will trigger a FeelGameEvent of the specified name when played")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Events/FeelGameEvent")]
	public class FeedbackMMGameEvent : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.EventsColor; } }
		public override bool EvaluateRequiresSetup() { return (GameEventName == ""); }
		public override string RequiredTargetText { get { return GameEventName;  } }
		public override string RequiresSetupText { get { return "This feedback requires that you specify a GameEventName below."; } }
		#endif

		[FeedbackInspectorGroup("FeelGameEvent", true, 57, true)]
		public string GameEventName;
		
		[FeedbackInspectorGroup("Optional Payload", true, 58, true)]
		public int IntParameter;
		public Vector2 Vector2Parameter;
		public Vector3 Vector3Parameter;
		public bool BoolParameter;
		public string StringParameter;

		/// <summary>
		/// On Play we change the values of our fog
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			FeelGameEvent.Trigger(GameEventName, IntParameter, Vector2Parameter, Vector3Parameter, BoolParameter, StringParameter);
		}
	}
}