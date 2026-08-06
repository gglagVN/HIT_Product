using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// An event type used to broadcast the fact that an achievement has been unlocked
	/// </summary>
	public struct AchievementUnlockedEvent
	{
		/// the achievement that has been unlocked
		public FeelAchievement Achievement;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="newAchievement">New achievement.</param>
		public AchievementUnlockedEvent(FeelAchievement newAchievement)
		{
			Achievement = newAchievement;
		}

		static AchievementUnlockedEvent e;
		public static void Trigger(FeelAchievement newAchievement)
		{
			e.Achievement = newAchievement;
			FeelEventManager.TriggerEvent(e);
		}
	}
	
	public struct AchievementChangedEvent
	{
		/// the achievement that has been unlocked
		public FeelAchievement Achievement;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="newAchievement">New achievement.</param>
		public AchievementChangedEvent(FeelAchievement newAchievement)
		{
			Achievement = newAchievement;
		}

		static AchievementChangedEvent e;
		public static void Trigger(FeelAchievement newAchievement)
		{
			e.Achievement = newAchievement;
			FeelEventManager.TriggerEvent(e);
		}
	}
}