using System.Collections;
using System.Collections.Generic;
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback will trigger a freeze frame event when played, pausing the game for the specified duration (usually short, but not necessarily)
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will freeze the timescale for the specified duration (in seconds). I usually go with 0.01s or 0.02s, but feel free to tweak it to your liking. It requires a TimeManager in your scene to work.")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks")]
	[System.Serializable]
	[FeedbackPath("Time/Freeze Frame")]
	public class FeedbackFreezeFrame : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.TimeColor; } }
		public override bool HasCustomInspectors => true;
		public override bool HasAutomaticShakerSetup => true;
		#endif

		[FeedbackInspectorGroup("Freeze Frame", true, 63)]
		/// the duration of the freeze frame
		[Tooltip("the duration of the freeze frame")]
		public float FreezeFrameDuration = 0.1f;
		/// the minimum value the timescale should be at for this freeze frame to happen. This can be useful to avoid triggering freeze frames when the timescale is already frozen. 
		[Tooltip("the minimum value the timescale should be at for this freeze frame to happen. This can be useful to avoid triggering freeze frames when the timescale is already frozen.")]
		public float MinimumTimescaleThreshold = 0.1f;

		/// the duration of this feedback is the duration of the freeze frame
		public override float FeedbackDuration { get { return ApplyTimeMultiplier(FreezeFrameDuration); } set { FreezeFrameDuration = value; } }

		/// <summary>
		/// On Play we trigger a freeze frame event
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			if (Time.timeScale < MinimumTimescaleThreshold)
			{
				return;
			}
            
			FreezeFrameEvent.Trigger(FeedbackDuration);
		}
		
		/// <summary>
		/// Automatically adds a TimeManager to the scene
		/// </summary>
		public override void AutomaticShakerSetup()
		{
			(TimeManager timeManager, bool createdNew) = Owner.gameObject.FindOrCreateObjectOfType<TimeManager>("TimeManager", null);
			if (createdNew)
			{
				FeelDebug.DebugLogInfo("Added a TimeManager to the scene. You're all set.");	
			}
		}
	}
}