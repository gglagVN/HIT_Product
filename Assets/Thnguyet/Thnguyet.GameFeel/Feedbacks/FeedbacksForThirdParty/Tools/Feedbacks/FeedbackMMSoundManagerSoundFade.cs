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
	/// This feedback lets you trigger fades on a specific sound via the FeelSoundManager. You will need a FeelSoundManager in your scene for this to work.
	/// </summary>
	[AddComponentMenu("")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Audio/FeelSoundManager Sound Fade")]
	[FeedbackHelp("This feedback lets you trigger fades on a specific sound via the FeelSoundManager. You will need a FeelSoundManager in your scene for this to work.")]
	public class FeedbackMMSoundManagerSoundFade : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SoundsColor; } }
		public override string RequiredTargetText { get { return "ID "+SoundID;  } }
		#endif

		[FeedbackInspectorGroup("FeelSoundManager Sound Fade", true, 30)]
		/// the ID of the sound you want to fade. Has to match the ID you specified when playing the sound initially
		[Tooltip("the ID of the sound you want to fade. Has to match the ID you specified when playing the sound initially")]
		public int SoundID = 0;
		/// the duration of the fade, in seconds
		[Tooltip("the duration of the fade, in seconds")]
		public float FadeDuration = 1f;
		/// the volume towards which to fade
		[Tooltip("the volume towards which to fade")]
		[Range(SoundManagerSettings._minimalVolume,SoundManagerSettings._maxVolume)]
		public float FinalVolume = SoundManagerSettings._minimalVolume;
		/// the tween to apply over the fade
		[Tooltip("the tween to apply over the fade")]
		public TweenType FadeTween = new TweenType(FeelTween.TweenCurve.EaseInOutQuartic);
        
		protected AudioSource _targetAudioSource;
        
		/// <summary>
		/// On play, we start our fade via a fade event
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			SoundManagerSoundFadeEvent.Trigger(SoundManagerSoundFadeEvent.Modes.PlayFade, SoundID, FadeDuration, FinalVolume, FadeTween);
		}
        
		/// <summary>
		/// On stop, we stop our fade via a fade event
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			SoundManagerSoundFadeEvent.Trigger(SoundManagerSoundFadeEvent.Modes.StopFade, SoundID, FadeDuration, FinalVolume, FadeTween);
		}
	}
}