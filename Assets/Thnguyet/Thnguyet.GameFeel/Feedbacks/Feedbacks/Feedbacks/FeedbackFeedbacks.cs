using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback allows you to trigger a target FeedbackPlayer, or any FeedbackPlayer on the specified Channel within a certain range. You'll need an FeedbacksShaker on them.
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to trigger a target FeedbackPlayer, or any FeedbackPlayer on the specified Channel within a certain range. You'll need an FeedbacksShaker on them.")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks")]
	[System.Serializable]
	[FeedbackPath("Feedbacks/Feedbacks Player")]
	public class FeedbackFeedbacks : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.FeedbacksColor; } }
		public override string RequiredTargetText => RequiredChannelText;
		#endif
		/// the duration of this feedback is the duration of our target feedback
		public override float FeedbackDuration 
		{
			get
			{
				if (TargetFeedbacks == Owner)
				{
					return 0f;
				}
				if ((Mode == Modes.PlayTargetFeedbacks) && (TargetFeedbacks != null))
				{
					return TargetFeedbacks.TotalDuration;
				}
				else
				{
					return 0f;    
				}
			} 
		}
		public override bool HasChannel => true;
        
		public enum Modes { PlayFeedbacksInArea, PlayTargetFeedbacks, TriggerMMF_PlayerEvent }
        
		[FeedbackInspectorGroup("Feedbacks", true, 79)]
        
		/// the selected mode for this feedback
		[Tooltip("the selected mode for this feedback")]
		public Modes Mode = Modes.PlayFeedbacksInArea;
        
		/// a specific FeelFeedbacks / FeedbackPlayer to play
		[FeedbackEnumCondition("Mode", (int)Modes.PlayTargetFeedbacks)]
		[Tooltip("a specific FeelFeedbacks / FeedbackPlayer to play")]
		public FeelFeedbacks TargetFeedbacks;
        
		/// whether or not to use a range
		[FeedbackEnumCondition("Mode", (int)Modes.PlayFeedbacksInArea)]
		[Tooltip("whether or not to use a range")]
		public bool OnlyTriggerPlayersInRange = false;
		/// the range of the event, in units
		[FeedbackEnumCondition("Mode", (int)Modes.PlayFeedbacksInArea)]
		[Tooltip("the range of the event, in units")]
		public float EventRange = 100f;
		/// the transform to use to broadcast the event as origin point
		[FeedbackEnumCondition("Mode", (int)Modes.PlayFeedbacksInArea)]
		[Tooltip("the transform to use to broadcast the event as origin point")]
		public Transform EventOriginTransform;

		/// <summary>
		/// On init we turn the light off if needed
		/// </summary>
		/// <param name="owner"></param>
		protected override void CustomInitialization(FeedbackPlayer owner)
		{
			base.CustomInitialization(owner);
            
			if (EventOriginTransform == null)
			{
				EventOriginTransform = owner.transform;
			}
		}

		/// <summary>
		/// On Play we trigger our target feedback or trigger a feedback shake event to shake feedbacks in the area
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (TargetFeedbacks == Owner)
			{
				return;
			}
			
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}

			if (Mode == Modes.PlayFeedbacksInArea)
			{
				FeedbacksShakeEvent.Trigger(ChannelData, OnlyTriggerPlayersInRange, EventRange, EventOriginTransform.position);    
			}
			else if (Mode == Modes.PlayTargetFeedbacks)
			{
				TargetFeedbacks?.PlayFeedbacks(position, feedbacksIntensity);
			}
			else if (Mode == Modes.TriggerMMF_PlayerEvent)
			{
				FeedbackPlayerEvent.Trigger(ChannelData, true, Owner.transform.position, FeedbackPlayerEvent.Modes.PlayFeedbacks, feedbacksIntensity, false);	
			}
		}
	}
}
