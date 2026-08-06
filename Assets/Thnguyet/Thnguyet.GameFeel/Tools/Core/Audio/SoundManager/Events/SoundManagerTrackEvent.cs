using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	public enum SoundManagerTrackEventTypes
	{
		MuteTrack,
		UnmuteTrack,
		SetVolumeTrack,
		PlayTrack,
		PauseTrack,
		StopTrack,
		FreeTrack
	}
    
	/// <summary>
	/// This feedback will let you mute, unmute, play, pause, stop, free or set the volume of a selected track
	///
	/// Example :  SoundManagerTrackEvent.Trigger(SoundManagerTrackEventTypes.PauseTrack,FeelSoundManager.SoundManagerTracks.UI);
	/// will pause the entire UI track
	/// </summary>
	public struct SoundManagerTrackEvent
	{
		/// the order to pass to the track
		public SoundManagerTrackEventTypes TrackEventType;
		/// the track to pass the order to
		public FeelSoundManager.SoundManagerTracks Track;
		/// if in SetVolume mode, the volume to which to set the track to
		public float Volume;
        
		public SoundManagerTrackEvent(SoundManagerTrackEventTypes trackEventType, FeelSoundManager.SoundManagerTracks track = FeelSoundManager.SoundManagerTracks.Master, float volume = 1f)
		{
			TrackEventType = trackEventType;
			Track = track;
			Volume = volume;
		}

		static SoundManagerTrackEvent e;
		public static void Trigger(SoundManagerTrackEventTypes trackEventType, FeelSoundManager.SoundManagerTracks track = FeelSoundManager.SoundManagerTracks.Master, float volume = 1f)
		{
			e.TrackEventType = trackEventType;
			e.Track = track;
			e.Volume = volume;
			FeelEventManager.TriggerEvent(e);
		}
	}
}