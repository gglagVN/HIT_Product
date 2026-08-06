using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// This event will let you pause 
	///
	/// Example : SoundManagerSoundFadeEvent.Trigger(33, 2f, 0.3f, new TweenType(FeelTween.TweenCurve.EaseInElastic));
	/// will fade the sound with an ID of 33 towards a volume of 0.3, over 2 seconds, on an elastic curve
	/// </summary>
	public struct SoundManagerSoundFadeEvent
	{
		public enum Modes { PlayFade, StopFade }

		/// whether we are fading a sound, or stopping an existing fade
		public Modes Mode;
		/// the ID of the sound to fade
		public int SoundID;
		/// the duration of the fade (in seconds)
		public float FadeDuration;
		/// the volume towards which to fade this sound
		public float FinalVolume;
		/// the tween over which to fade this sound
		public TweenType FadeTween;
		
		
        
		public SoundManagerSoundFadeEvent(Modes mode, int soundID, float fadeDuration, float finalVolume, TweenType fadeTween)
		{
			Mode = mode;
			SoundID = soundID;
			FadeDuration = fadeDuration;
			FinalVolume = finalVolume;
			FadeTween = fadeTween;
		}

		static SoundManagerSoundFadeEvent e;
		public static void Trigger(Modes mode, int soundID, float fadeDuration, float finalVolume, TweenType fadeTween)
		{
			e.Mode = mode;
			e.SoundID = soundID;
			e.FadeDuration = fadeDuration;
			e.FinalVolume = finalVolume;
			e.FadeTween = fadeTween;
			FeelEventManager.TriggerEvent(e);
		}
	}
}