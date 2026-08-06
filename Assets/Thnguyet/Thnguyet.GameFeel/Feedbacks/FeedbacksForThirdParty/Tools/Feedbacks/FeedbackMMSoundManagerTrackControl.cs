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
	/// This feedback will let you control all sounds playing on a specific track (master, UI, music, sfx), and play, pause, mute, unmute, resume, stop, free them all at once. You will need a FeelSoundManager in your scene for this to work.
	/// </summary>
	[AddComponentMenu("")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Audio/FeelSoundManager Track Control")]
	[FeedbackHelp("This feedback will let you control all sounds playing on a specific track (master, UI, music, sfx), and play, pause, mute, unmute, resume, stop, free them all at once. You will need a FeelSoundManager in your scene for this to work.")]
	public class FeedbackMMSoundManagerTrackControl : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SoundsColor; } }
		public override string RequiredTargetText { get { return Track.ToString() + " " + ControlMode.ToString();  } }
		#endif
        
		/// the possible modes you can use to interact with the track. Free will stop all sounds and return them to the pool
		public enum ControlModes { Mute, UnMute, SetVolume, Pause, Play, Stop, Free }
        
		[FeedbackInspectorGroup("FeelSoundManager Track Control", true, 30)]
		/// the track to mute/unmute/pause/play/stop/free/etc
		[Tooltip("the track to mute/unmute/pause/play/stop/free/etc")]
		public FeelSoundManager.SoundManagerTracks Track;
		/// the selected control mode to interact with the track. Free will stop all sounds and return them to the pool
		[Tooltip("the selected control mode to interact with the track. Free will stop all sounds and return them to the pool")]
		public ControlModes ControlMode = ControlModes.Pause;
		/// if setting the volume, the volume to assign to the track 
		[Tooltip("if setting the volume, the volume to assign to the track")]
		[EnumCondition("ControlMode", (int) ControlModes.SetVolume)]
		public float Volume = 0.5f;

		/// <summary>
		/// On play, orders the entire track to follow the specific command, via a FeelSoundManager event
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			switch (ControlMode)
			{
				case ControlModes.Mute:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.MuteTrack, Track);
					break;
				case ControlModes.UnMute:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.UnmuteTrack, Track);
					break;
				case ControlModes.SetVolume:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.SetVolumeTrack, Track, Volume);
					break;
				case ControlModes.Pause:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.PauseTrack, Track);
					break;
				case ControlModes.Play:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.PlayTrack, Track);
					break;
				case ControlModes.Stop:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.StopTrack, Track);
					break;
				case ControlModes.Free:
					SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.FreeTrack, Track);
					break;
			}
		}
	}
}