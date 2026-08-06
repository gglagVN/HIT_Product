using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	public enum SoundManagerAllSoundsControlEventTypes
	{
		Pause, Play, Stop, Free, FreeAllButPersistent, FreeAllLooping
	}
    
	/// <summary>
	/// This event will let you pause/play/stop/free all sounds playing through the FeelSoundManager at once
	///
	/// Example : SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.Stop);
	/// will stop all sounds playing at once
	/// </summary>
	public struct SoundManagerAllSoundsControlEvent
	{
		public SoundManagerAllSoundsControlEventTypes EventType;
        
		public SoundManagerAllSoundsControlEvent(SoundManagerAllSoundsControlEventTypes eventType)
		{
			EventType = eventType;
		}

		static SoundManagerAllSoundsControlEvent e;
		public static void Trigger(SoundManagerAllSoundsControlEventTypes eventType)
		{
			e.EventType = eventType;
			FeelEventManager.TriggerEvent(e);
		}
	}
}