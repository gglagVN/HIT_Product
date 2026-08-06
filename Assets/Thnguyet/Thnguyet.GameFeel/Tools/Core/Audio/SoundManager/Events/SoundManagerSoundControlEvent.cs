using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	public enum SoundManagerSoundControlEventTypes
	{
		Pause,
		Resume,
		Stop,
		Free
	}
    
	/// <summary>
	/// An event used to control a specific sound on the FeelSoundManager.
	/// You can either search for it by ID, or directly pass an audiosource if you have it.
	///
	/// Example : SoundManagerSoundControlEvent.Trigger(SoundManagerSoundControlEventTypes.Stop, 33);
	/// will cause the sound(s) with an ID of 33 to stop playing
	/// </summary>
	public struct SoundManagerSoundControlEvent
	{
		/// the ID of the sound to control (has to match the one used to play it)
		public int SoundID;
		/// the control mode
		public SoundManagerSoundControlEventTypes SoundManagerSoundControlEventType;
		/// the audiosource to control (if specified)
		public AudioSource TargetSource;
        
		public SoundManagerSoundControlEvent(SoundManagerSoundControlEventTypes eventType, int soundID, AudioSource source = null)
		{
			SoundID = soundID;
			TargetSource = source;
			SoundManagerSoundControlEventType = eventType;
		}

		static SoundManagerSoundControlEvent e;
		public static void Trigger(SoundManagerSoundControlEventTypes eventType, int soundID, AudioSource source = null)
		{
			e.SoundID = soundID;
			e.TargetSource = source;
			e.SoundManagerSoundControlEventType = eventType;
			FeelEventManager.TriggerEvent(e);
		}
	}
}