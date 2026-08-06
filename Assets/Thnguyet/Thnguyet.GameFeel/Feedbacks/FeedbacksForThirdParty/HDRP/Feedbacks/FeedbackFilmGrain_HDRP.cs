using UnityEngine;
using Thnguyet.GameFeel.Feedbacks;
using UnityEngine.Scripting.APIUpdating;
#if GAMEFEEL_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback allows you to control HDRP Film Grain intensity over time.
	/// It requires you have in your scene an object with a Volume 
	/// with Film Grain active, and a FilmGrainShaker_HDRP component.
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	#if GAMEFEEL_HDRP
	[FeedbackPath("PostProcess/Film Grain HDRP")]
	#endif
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.HDRP")]
	[FeedbackHelp("This feedback allows you to control Film Grain intensity over time. " +
	              "It requires you have in your scene an object with a Volume " +
	              "with Film Grain active, and a FilmGrainShaker_HDRP component.")]
	public class FeedbackFilmGrain_HDRP : FeedbackFeedback
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

		[FeedbackInspectorGroup("Film Grain", true, 20)]
		/// the duration of the shake, in seconds
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;
		/// whether or not to reset shaker values after shake
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;
		/// whether or not to reset the target's values after shake
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		[FeedbackInspectorGroup("Intensity", true, 21)]
		/// the curve to animate the intensity on
		[Tooltip("the curve to animate the intensity on")]
		public AnimationCurve Intensity = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));        
		/// the value to remap the curve's zero to
		[Tooltip("the value to remap the curve's zero to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero = 0f;
		/// the value to remap the curve's one to
		[Tooltip("the value to remap the curve's one to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1.0f;
		/// whether or not to add to the initial intensity
		[Tooltip("whether or not to add to the initial intensity")]
		public bool RelativeIntensity = false;

		/// <summary>
		/// Triggers a Film Grain shake
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
			FilmGrainShakeEvent_HDRP.Trigger(Intensity, FeedbackDuration, RemapIntensityZero, RemapIntensityOne, RelativeIntensity, intensityMultiplier,
				ChannelData, ResetShakerValuesAfterShake, ResetTargetValuesAfterShake, NormalPlayDirection, ComputedTimescaleMode);
            
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
            
			FilmGrainShakeEvent_HDRP.Trigger(Intensity, FeedbackDuration, RemapIntensityZero, RemapIntensityOne, RelativeIntensity, channelData:ChannelData, stop:true);
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
			
			FilmGrainShakeEvent_HDRP.Trigger(Intensity, FeedbackDuration, RemapIntensityZero, RemapIntensityOne, RelativeIntensity, channelData:ChannelData, restore:true);
		}
		
		/// <summary>
		/// Automaticall sets up the post processing profile and shaker
		/// </summary>
		public override void AutomaticShakerSetup()
		{
			#if GAMEFEEL_HDRP && UNITY_EDITOR
			HDRPHelpers.GetOrCreateVolume<FilmGrain, FilmGrainShaker_HDRP>(Owner, "FilmGrain");
			#endif
		}
	}
}