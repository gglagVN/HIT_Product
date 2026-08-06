using UnityEngine;
using Thnguyet.GameFeel.Feedbacks;
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
using Lofelt.NiceVibrations;
#endif
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// Use this feedback to play a preset haptic, limited but super simple predifined haptic patterns
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
	[FeedbackPath("Haptics/Haptic Preset")]
	#endif    
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.NiceVibrations")]
	[FeedbackHelp("Use this feedback to play a preset haptic, limited but super simple predifined haptic patterns")]
	public class FeedbackNVPreset : FeedbackFeedback
	{
		#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		#if UNITY_EDITOR
		public override bool HasCustomInspectors => true;
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.HapticsColor; } }
		public override string RequiredTargetText { get { return Preset.ToString();  } }
		#endif
    
		[FeedbackInspectorGroup("Haptic Preset", true, 21)]
		/// the preset to play with this feedback
		[Tooltip("the preset to play with this feedback")]
		public HapticPatterns.PresetType Preset = HapticPatterns.PresetType.LightImpact;
		/// a debug button that lets you test the haptic file from its inspector
		public FeedbackButton PlayPresetButton;

		[FeedbackInspectorGroup("Settings", true, 16)]
		/// a set of settings you can tweak to specify how and when exactly this haptic should play
		[Tooltip("a set of settings you can tweak to specify how and when exactly this haptic should play")]
		public FeedbackNVSettings HapticSettings;
		
		/// <summary>
		/// Initializes custom buttons
		/// </summary>
		public override void InitializeCustomAttributes()
		{
			base.InitializeCustomAttributes();
			PlayPresetButton = new FeedbackButton("Test Preset", PlayPreset);
		}
        
		/// <summary>
		/// On play we play our preset haptic
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized || HapticSettings == null || !HapticSettings.CanPlay())
			{
				return;
			}

			PlayPreset();
		}

		/// <summary>
		/// Plays the target preset
		/// </summary>
		protected virtual void PlayPreset()
		{
			HapticSettings.SetGamepad();
			HapticPatterns.PlayPreset(Preset);
		}
		
		#else
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f) { }
		#endif
	}    
}