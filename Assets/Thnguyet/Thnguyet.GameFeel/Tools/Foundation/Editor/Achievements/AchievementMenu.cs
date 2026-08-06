using UnityEngine;
using System.Collections;
using Thnguyet.GameFeel;
using UnityEditor;

namespace Thnguyet.GameFeel
{	
	public static class AchievementMenu 
	{
		[MenuItem("Tools/Thnguyet/GameFeel/Reset all achievements", false,21)]
		/// <summary>
		/// Adds a menu item to enable help
		/// </summary>
		private static void EnableHelpInInspectors()
		{
			AchievementManager.ResetAllAchievements ();
		}
	}
}