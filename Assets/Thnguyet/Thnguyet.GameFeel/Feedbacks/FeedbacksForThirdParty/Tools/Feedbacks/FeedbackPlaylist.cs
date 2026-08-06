using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thnguyet.GameFeel;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback will let you pilot a FeelPlaylist
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you pilot a FeelPlaylist")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Audio/FeelPlaylist")]
	public class FeedbackPlaylist : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SoundsColor; } }
		public override string RequiredTargetText { get => Mode.ToString(); }
		public override bool HasChannel => true;
		#endif
		
		public enum Modes { Play, PlayNext, PlayPrevious, Stop, Pause, PlaySongAt, SetVolumeMultiplier, ChangePlaylist }
 
		[FeedbackInspectorGroup("FeelPlaylist", true, 13)]
		/// the action to call on the playlist
		[Tooltip("the action to call on the playlist")]
		public Modes Mode = Modes.PlayNext;
		/// the index of the song to play
		[Tooltip("the index of the song to play")]
		[EnumCondition("Mode", (int)Modes.PlaySongAt)]
		public int SongIndex = 0;
		/// the volume multiplier to apply
		[Tooltip("the volume multiplier to apply")]
		[EnumCondition("Mode", (int)Modes.SetVolumeMultiplier)]
		public float VolumeMultiplier = 1f;
		/// whether to apply the volume multiplier instantly (true) or only when the next song starts playing (false)
		[Tooltip("whether to apply the volume multiplier instantly (true) or only when the next song starts playing (false)")]
		[EnumCondition("Mode", (int)Modes.SetVolumeMultiplier)]
		public bool ApplyVolumeMultiplierInstantly = false;
		/// in change playlist mode, the playlist to which to switch to. Only works with SMPlaylistManager
		[Tooltip("in change playlist mode, the playlist to which to switch to. Only works with SMPlaylistManager")]
		[EnumCondition("Mode", (int)Modes.ChangePlaylist)]
		public SMPlaylist NewPlaylist;
		/// in change playlist mode, whether or not to play the new playlist after the switch. Only works with SMPlaylistManager
		[Tooltip("in change playlist mode, whether or not to play the new playlist after the switch. Only works with SMPlaylistManager")]
		[EnumCondition("Mode", (int)Modes.ChangePlaylist)]
		public bool ChangePlaylistAndPlay = true;
        
		protected Coroutine _coroutine;

		/// <summary>
		/// On Play we change the values of our fog
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
				case Modes.Play:
					PlaylistPlayEvent.Trigger(Channel);
					break;
				case Modes.PlayNext:
					PlaylistPlayNextEvent.Trigger(Channel);
					break;
				case Modes.PlayPrevious:
					PlaylistPlayPreviousEvent.Trigger(Channel);
					break;
				case Modes.Stop:
					PlaylistStopEvent.Trigger(Channel);
					break;
				case Modes.Pause:
					PlaylistPauseEvent.Trigger(Channel);
					break;
				case Modes.PlaySongAt:
					PlaylistPlayIndexEvent.Trigger(Channel, SongIndex);
					break;
				case Modes.SetVolumeMultiplier:
					PlaylistVolumeMultiplierEvent.Trigger(Channel, VolumeMultiplier, ApplyVolumeMultiplierInstantly);
					break;
				case Modes.ChangePlaylist:
					PlaylistChangeEvent.Trigger(Channel, NewPlaylist, ChangePlaylistAndPlay);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
            
		}
	}
}
