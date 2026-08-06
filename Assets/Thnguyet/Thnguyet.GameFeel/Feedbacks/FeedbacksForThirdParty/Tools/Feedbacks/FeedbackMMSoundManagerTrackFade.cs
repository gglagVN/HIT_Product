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
	/// This feedback will let you fade all the sounds on a specific track at once. You will need a FeelSoundManager in your scene for this to work.
	/// </summary>
	[AddComponentMenu("")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Audio/FeelSoundManager Track Fade")]
	[FeedbackHelp("This feedback will let you fade all the sounds on a specific track at once. You will need a FeelSoundManager in your scene for this to work.")]
	public class FeedbackMMSoundManagerTrackFade : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SoundsColor; } }
		public override string RequiredTargetText { get { return Track.ToString();  } }
		#endif

		/// the duration of this feedback is the duration of the fade
		public override float FeedbackDuration { get { return FadeDuration; } }
        
		[FeedbackInspectorGroup("FeelSoundManager Track Fade", true, 30)]
		/// the track to fade the volume on
		[Tooltip("the track to fade the volume on")]
		public FeelSoundManager.SoundManagerTracks Track;
		/// the duration of the fade, in seconds
		[Tooltip("the duration of the fade, in seconds")]
		public float FadeDuration = 1f;
		/// the volume to reach at the end of the fade
		[Tooltip("the volume to reach at the end of the fade")]
		[Range(SoundManagerSettings._minimalVolume,SoundManagerSettings._maxVolume)]
		public float FinalVolume = SoundManagerSettings._minimalVolume;
		/// the tween to operate the fade on
		[Tooltip("the tween to operate the fade on")]
		public TweenType FadeTween = new TweenType(FeelTween.TweenCurve.EaseInOutQuartic);
        
		/// <summary>
		/// On Play, triggers a fade event, meant to be caught by the FeelSoundManager
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			SoundManagerTrackFadeEvent.Trigger(SoundManagerTrackFadeEvent.Modes.PlayFade, Track, FadeDuration, FinalVolume, FadeTween);
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
            
			SoundManagerTrackFadeEvent.Trigger(SoundManagerTrackFadeEvent.Modes.StopFade, Track, FadeDuration, FinalVolume, FadeTween);
		}
	}
}