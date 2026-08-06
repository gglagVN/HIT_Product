using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	public enum SoundManagerEventTypes
	{
		SaveSettings,
		LoadSettings,
		ResetSettings,
		SettingsLoaded
	}
    
	/// <summary>
	/// This event will let you trigger a save/load/reset on the FeelSoundManager settings
	///
	/// Example : SoundManagerEvent.Trigger(SoundManagerEventTypes.SaveSettings);
	/// will save settings. 
	/// </summary>
	public struct SoundManagerEvent
	{
		public SoundManagerEventTypes EventType;
        
		public SoundManagerEvent(SoundManagerEventTypes eventType)
		{
			EventType = eventType;
		}

		static SoundManagerEvent e;
		public static void Trigger(SoundManagerEventTypes eventType)
		{
			e.EventType = eventType;
			FeelEventManager.TriggerEvent(e);
		}
	}
}