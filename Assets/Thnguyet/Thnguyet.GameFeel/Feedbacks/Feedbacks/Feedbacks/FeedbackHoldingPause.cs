using System.Collections;
using System.Collections.Generic;
using Thnguyet.GameFeel;
using UnityEngine;using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// this feedback will "hold", or wait, until all previous feedbacks have been executed, and will then pause the execution of your FeelFeedbacks sequence, for the specified duration
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will 'hold', or wait, until all previous feedbacks have been executed, and will then pause the execution of your FeelFeedbacks sequence, for the specified duration.")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks")]
	[System.Serializable]
	[FeedbackPath("Pause/Holding Pause")]
	public class FeedbackHoldingPause : FeedbackPause
	{
		/// sets the color of this feedback in the inspector
		#if UNITY_EDITOR
		public override Color FeedbackColor { get => FeedbacksInspectorColors.HoldingPauseColor; }
		public override Color DisplayColor => FeedbacksInspectorColors.HoldingPauseColor.Darken(0.35f);
		#endif
		public override bool HoldingPause => true;

		/// the duration of this feedback is the duration of the pause
		public override float FeedbackDuration { get { return ApplyTimeMultiplier(PauseDuration); } set { PauseDuration = value; } }
		
		/// <summary>
		/// On custom play we just play our pause
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (Active)
			{
				ProcessNewPauseDuration();
				_pauseCoroutine = Owner.StartCoroutine(PlayPause());
			}
		}

		/// <summary>
		/// On Stop, we stop our pause
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			if (_pauseCoroutine != null)
			{
				Owner.StopCoroutine(_pauseCoroutine);
			}
		}
	}
}
