using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to pilot a FeelPlaylist
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/Audio/Playlist Remote")]
	public class PlaylistRemote : MonoBehaviour
	{
		public int Channel = 0;
		/// The track to play when calling PlaySelectedTrack
		public int TrackNumber = 0;

		[Header("Triggers")]
		/// if this is true, the selected track will be played on trigger enter (if you have a trigger collider on this)
		public bool PlaySelectedTrackOnTriggerEnter = true;
		/// if this is true, the selected track will be played on trigger exit (if you have a trigger collider on this)
		public bool PlaySelectedTrackOnTriggerExit = false;
		/// the tag to check for on trigger stuff
		public string TriggerTag = "Player";

		[Header("Test")]
		/// a play test button
		[InspectorButton("Play")]
		public bool PlayButton;
		/// a pause test button
		[InspectorButton("Pause")]
		public bool PauseButton;
		/// a stop test button
		[InspectorButton("Stop")]
		public bool StopButton;
		/// a next track test button
		[InspectorButton("PlayNextTrack")]
		public bool NextButton;
		/// a selected track test button
		[InspectorButton("PlaySelectedTrack")]
		public bool SelectedTrackButton;

		/// <summary>
		/// Plays the playlist
		/// </summary>
		public virtual void Play()
		{
			PlaylistPlayEvent.Trigger(Channel);
		}

		/// <summary>
		/// Pauses the current track
		/// </summary>
		public virtual void Pause()
		{
			PlaylistPauseEvent.Trigger(Channel);
		}

		/// <summary>
		/// Stops the playlist
		/// </summary>
		public virtual void Stop()
		{
			PlaylistStopEvent.Trigger(Channel);
		}

		/// <summary>
		/// Plays the next track in the playlist
		/// </summary>
		public virtual void PlayNextTrack()
		{
			PlaylistPlayNextEvent.Trigger(Channel);
		}

		/// <summary>
		/// Plays the track selected in the inspector
		/// </summary>
		public virtual void PlaySelectedTrack()
		{
			PlaylistPlayIndexEvent.Trigger(Channel, TrackNumber);
		}

		/// <summary>
		/// Plays the track set in parameters
		/// </summary>
		public virtual void PlayTrack(int trackIndex)
		{
			PlaylistPlayIndexEvent.Trigger(Channel, trackIndex);
		}

		/// <summary>
		/// On trigger enter, we play the selected track if needed
		/// </summary>
		/// <param name="collider"></param>
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if (PlaySelectedTrackOnTriggerEnter && (collider.CompareTag(TriggerTag)))
			{
				PlaySelectedTrack();
			}
		}

		/// <summary>
		/// On trigger exit, we play the selected track if needed
		/// </summary>
		protected virtual void OnTriggerExit(Collider collider)
		{
			if (PlaySelectedTrackOnTriggerExit && (collider.CompareTag(TriggerTag)))
			{
				PlaySelectedTrack();
			}
		}
		
		#if GAMEFEEL_PHYSICS2D

		/// <summary>
		/// On trigger enter 2D, we play the selected track if needed
		/// </summary>
		/// <param name="collider"></param>
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			if (PlaySelectedTrackOnTriggerEnter && (collider.CompareTag(TriggerTag)))
			{
				PlaySelectedTrack();
			}
		}

		/// <summary>
		/// On trigger exit 2D, we play the selected track if needed
		/// </summary>
		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			if (PlaySelectedTrackOnTriggerExit && (collider.CompareTag(TriggerTag)))
			{
				PlaySelectedTrack();
			}
		}

		#endif
	}
}
