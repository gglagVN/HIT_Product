using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Thnguyet.GameFeel;
using UnityEngine.Audio;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback will let you trigger save, load, and reset on FeelSoundManager settings. You will need a FeelSoundManager in your scene for this to work.
	/// </summary>
	[AddComponentMenu("")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Audio/FeelSoundManager Save and Load")]
	[FeedbackHelp("This feedback will let you trigger save, load, and reset on FeelSoundManager settings. You will need a FeelSoundManager in your scene for this to work.")]
	public class FeedbackMMSoundManagerSaveLoad : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SoundsColor; } }
		public override string RequiredTargetText { get { return Mode.ToString();  } }
		#endif

		/// the possible modes you can use to interact with save settings
		public enum Modes { Save, Load, Reset }

		[FeedbackInspectorGroup("FeelSoundManager Save and Load", true, 30)]
		/// the selected mode to interact with save settings on the FeelSoundManager
		[Tooltip("the selected mode to interact with save settings on the FeelSoundManager")]
		public Modes Mode = Modes.Save;
        
		/// <summary>
		/// On Play, saves, loads or resets settings
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			switch (Mode)
			{
				case Modes.Save:
					SoundManagerEvent.Trigger(SoundManagerEventTypes.SaveSettings);
					break;
				case Modes.Load:
					SoundManagerEvent.Trigger(SoundManagerEventTypes.LoadSettings);
					break;
				case Modes.Reset:
					SoundManagerEvent.Trigger(SoundManagerEventTypes.ResetSettings);
					break;
			}
		}
	}
}