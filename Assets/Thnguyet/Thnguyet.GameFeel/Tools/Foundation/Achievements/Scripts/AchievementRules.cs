using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// That class is meant to be extended to implement the achievement rules specific to your game.
	/// </summary>
	public abstract class AchievementRules : MonoBehaviour, EventListener<FeelGameEvent>
	{
		public AchievementList AchievementList;
		[InspectorButton("PrintCurrentStatus")]
		public bool PrintCurrentStatusBtn;

		public virtual void PrintCurrentStatus()
		{
			foreach (FeelAchievement achievement in AchievementManager.AchievementsList)
			{
				string status = achievement.UnlockedStatus ? "unlocked" : "locked";
				FeelDebug.DebugLogInfo("["+achievement.AchievementID + "] "+achievement.Title+", status : "+status+", progress : "+achievement.ProgressCurrent+"/"+achievement.ProgressTarget);
			}	
		}
		
		/// <summary>
		/// On Awake, loads the achievement list and the saved file
		/// </summary>
		protected virtual void Awake()
		{
			// we load the list of achievements, stored in a ScriptableObject in our Resources folder.
			AchievementManager.LoadAchievementList (AchievementList);
			// we load our saved file, to update that list with the saved values.
			AchievementManager.LoadSavedAchievements ();
		}

		/// <summary>
		/// On enable, we start listening for GameEvents. You may want to extend that to listen to other types of events.
		/// </summary>
		protected virtual void OnEnable()
		{
			this.EventStartListening<FeelGameEvent>();
		}

		/// <summary>
		/// On disable, we stop listening for GameEvents. You may want to extend that to stop listening to other types of events.
		/// </summary>
		protected virtual void OnDisable()
		{
			this.EventStopListening<FeelGameEvent>();
		}

		/// <summary>
		/// When we catch an FeelGameEvent, we do stuff based on its name
		/// </summary>
		/// <param name="gameEvent">Game event.</param>
		public virtual void OnMMEvent(FeelGameEvent gameEvent)
		{
			switch (gameEvent.EventName)
			{
				case "Save":
					AchievementManager.SaveAchievements ();
					break;
				/*
				// These are just examples of how you could catch a GameStart FeelGameEvent and trigger the potential unlock of a corresponding achievement 
				case "GameStart":
					AchievementManager.UnlockAchievement("theFirestarter");
					break;
				case "LifeLost":
					AchievementManager.UnlockAchievement("theEndOfEverything");
					break;
				case "Pause":
					AchievementManager.UnlockAchievement("timeStop");
					break;
				case "Jump":
					AchievementManager.UnlockAchievement ("aSmallStepForMan");
					AchievementManager.AddProgress ("toInfinityAndBeyond", 1);
					break;*/
			}
		} 
	}
}