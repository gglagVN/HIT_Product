using UnityEngine;
using Thnguyet.GameFeel.Feedbacks;
using UnityEngine.Scripting.APIUpdating;
#if GAMEFEEL_URP
using UnityEngine.Rendering.Universal;
#endif

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// This feedback allows you to control URP vignette intensity over time.
	/// It requires you have in your scene an object with a Volume 
	/// with Vignette active, and a VignetteShaker_URP component.
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	#if GAMEFEEL_URP
	[FeedbackPath("PostProcess/Vignette URP")]
	#endif
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.URP")]
	[FeedbackHelp("This feedback allows you to control vignette intensity over time. " +
	              "It requires you have in your scene an object with a Volume " +
	              "with Vignette active, and a VignetteShaker_URP component.")]
	public class FeedbackVignette_URP : FeedbackFeedback
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

		[FeedbackInspectorGroup("Vignette", true, 28)]
		/// the duration of the shake, in seconds
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.2f;
		/// whether or not to reset shaker values after shake
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;
		/// whether or not to reset the target's values after shake
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		[FeedbackInspectorGroup("Intensity", true, 29)]
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
		
		[Header("Color")]
		/// whether or not to also animate  the vignette's color
		[Tooltip("whether or not to also animate the vignette's color")]
		public bool InterpolateColor = false;
		/// the curve to animate the color on
		[Tooltip("the curve to animate the color on")]
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.05f, 1f), new Keyframe(0.95f, 1), new Keyframe(1, 0));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0, 1)]
		public float RemapColorZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapColorOne = 1f;
		/// the color to lerp towards
		[Tooltip("the color to lerp towards")]
		public Color TargetColor = Color.red;

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
			VignetteShakeEvent_URP.Trigger(Intensity, FeedbackDuration, RemapIntensityZero, RemapIntensityOne, RelativeIntensity, intensityMultiplier,
				ChannelData, ResetShakerValuesAfterShake, ResetTargetValuesAfterShake, NormalPlayDirection, ComputedTimescaleMode, false, false, InterpolateColor,
				ColorCurve, RemapColorZero, RemapColorOne, TargetColor);
            
		}
        
		/// <summary>
		/// On stop we stop our transition
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
            
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			VignetteShakeEvent_URP.Trigger(Intensity, FeedbackDuration, RemapIntensityZero, RemapIntensityOne, RelativeIntensity, stop: true, channelData: ChannelData);
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
			
			VignetteShakeEvent_URP.Trigger(Intensity, FeedbackDuration, RemapIntensityZero, RemapIntensityOne, RelativeIntensity, restore: true, channelData: ChannelData);
		}
		
		/// <summary>
		/// Automaticall sets up the post processing profile and shaker
		/// </summary>
		public override void AutomaticShakerSetup()
		{
			#if GAMEFEEL_URP && UNITY_EDITOR
			URPHelpers.GetOrCreateVolume<Vignette, VignetteShaker_URP>(Owner, "Vignette");
			#endif
		}
	}
}