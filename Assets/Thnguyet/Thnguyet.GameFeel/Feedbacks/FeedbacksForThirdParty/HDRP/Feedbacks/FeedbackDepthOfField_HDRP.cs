using UnityEngine;
using Thnguyet.GameFeel.Feedbacks;
using UnityEngine.Scripting.APIUpdating;
#if GAMEFEEL_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback allows you to control HDRP Depth of Field focus distance or near/far ranges over time.
	/// It requires you have in your scene an object with a Volume 
	/// with Depth of Field active, and a DepthOfFieldShaker_HDRP component.
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	#if GAMEFEEL_HDRP
	[FeedbackPath("PostProcess/Depth of Field HDRP")]
	#endif
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.HDRP")]
	[FeedbackHelp("This feedback allows you to control HDRP Depth of Field focus distance or near/far ranges over time." +
	              "It requires you have in your scene an object with a Volume " +
	              "with Depth of Field active, and a DepthOfFieldShaker_HDRP component.")]
	public class FeedbackDepthOfField_HDRP : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.PostProcessColor; } }
		public override bool HasCustomInspectors => true;
		public override bool HasAutomaticShakerSetup => true;
		#endif

		/// the duration of this feedback is the duration of the shake
		public override float FeedbackDuration { get { return ApplyTimeMultiplier(Duration); } set { Duration = value; } }
		public override bool HasChannel => true;
		public override bool HasRandomness => true;

		[FeedbackInspectorGroup("Depth of Field", true, 28)]
		/// the duration of the shake, in seconds
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;
		/// whether or not to reset shaker values after shake
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;
		/// whether or not to reset the target's values after shake
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;
		
		[FeedbackInspectorGroup("Focus Distance", true, 53)]
		/// whether or not to animate the focus distance
		[Tooltip("whether or not to animate the focus distance")]
		public bool AnimateFocusDistance = true;
		/// the curve used to animate the focus distance value on
		[Tooltip("the curve used to animate the focus distance value on")]
		[FeedbackCondition("AnimateFocusDistance", true)]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackCondition("AnimateFocusDistance", true)]
		public float RemapFocusDistanceZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackCondition("AnimateFocusDistance", true)]
		public float RemapFocusDistanceOne = 3f;
		
		
		[FeedbackInspectorGroup("Near Range", true, 52)]
		
		[Header("Near Range Start")]
		/// whether or not to animate the near range start
		[Tooltip("whether or not to animate the near range start")]
		public bool AnimateNearRangeStart = false;
		/// the curve used to animate the near range start on
		[Tooltip("the curve used to animate the near range start on")]
		[FeedbackCondition("AnimateNearRangeStart", true)]
		public AnimationCurve ShakeNearRangeStart = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackCondition("AnimateNearRangeStart", true)]
		public float RemapNearRangeStartZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackCondition("AnimateNearRangeStart", true)]
		public float RemapNearRangeStartOne = 3f;
		
		[Header("Near Range End")]
		/// whether or not to animate the near range end
		[Tooltip("whether or not to animate the near range end")]
		public bool AnimateNearRangeEnd = false;
		/// the curve used to animate the near range end on
		[Tooltip("the curve used to animate the near range end on")]
		[FeedbackCondition("AnimateNearRangeEnd", true)]
		public AnimationCurve ShakeNearRangeEnd = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackCondition("AnimateNearRangeEnd", true)]
		public float RemapNearRangeEndZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackCondition("AnimateNearRangeEnd", true)]
		public float RemapNearRangeEndOne = 3f;
		
		[FeedbackInspectorGroup("Far Range", true, 51)]
		
		[Header("Far Range Start")]
		/// whether or not to animate the far range start
		[Tooltip("whether or not to animate the far range start")]
		public bool AnimateFarRangeStart = false;
		/// the curve used to animate the far range start on
		[Tooltip("the curve used to animate the far range start on")]
		[FeedbackCondition("AnimateFarRangeStart", true)]
		public AnimationCurve ShakeFarRangeStart = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackCondition("AnimateFarRangeStart", true)]
		public float RemapFarRangeStartZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackCondition("AnimateFarRangeStart", true)]
		public float RemapFarRangeStartOne = 3f;
		
		[Header("Far Range End")]
		/// whether or not to animate the far range end
		[Tooltip("whether or not to animate the far range end")]
		public bool AnimateFarRangeEnd = false;
		/// the curve used to animate the far range end on
		[Tooltip("the curve used to animate the far range end on")]
		[FeedbackCondition("AnimateFarRangeEnd", true)]
		public AnimationCurve ShakeFarRangeEnd = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackCondition("AnimateFarRangeEnd", true)]
		public float RemapFarRangeEndZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackCondition("AnimateFarRangeEnd", true)]
		public float RemapFarRangeEndOne = 3f;

		/// <summary>
		/// Triggers a vignette shake
		/// </summary>
		/// <param name="position"></param>
		/// <param name="attenuation"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			float intensityMultiplier = ComputeIntensity(feedbacksIntensity, position);
			DepthOfFieldShakeEvent_HDRP.Trigger(Duration, intensityMultiplier, ChannelData, ResetShakerValuesAfterShake, 
				ResetTargetValuesAfterShake, NormalPlayDirection, ComputedTimescaleMode, false, false, 
				AnimateFocusDistance, ShakeFocusDistance, RemapFocusDistanceZero, RemapFocusDistanceOne,
				AnimateNearRangeStart, ShakeNearRangeStart, RemapNearRangeStartZero, RemapNearRangeStartOne,
				AnimateNearRangeEnd, ShakeNearRangeEnd, RemapNearRangeEndZero, RemapNearRangeEndOne,
				AnimateFarRangeStart, ShakeFarRangeStart, RemapFarRangeStartZero, RemapFarRangeStartOne,
				AnimateFarRangeEnd, ShakeFarRangeEnd,RemapFarRangeEndZero,RemapFarRangeEndOne);
		}
        
		/// <summary>
		/// On stop we stop our transition
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			DepthOfFieldShakeEvent_HDRP.Trigger(Duration, channelData: ChannelData, stop:true);
		}
		
		/// <summary>
		/// On restore, we put our object back at its initial position
		/// </summary>
		protected override void CustomRestoreInitialValues()
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			
			DepthOfFieldShakeEvent_HDRP.Trigger(Duration, channelData: ChannelData, restore:true);
		}
		
		/// <summary>
		/// Automaticall sets up the post processing profile and shaker
		/// </summary>
		public override void AutomaticShakerSetup()
		{
			#if GAMEFEEL_HDRP && UNITY_EDITOR
			HDRPHelpers.GetOrCreateVolume<DepthOfField, DepthOfFieldShaker_HDRP>(Owner, "DepthOfField");
			#endif
		}
	}
}