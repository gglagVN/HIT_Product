using UnityEngine;
using System.Collections;
using Thnguyet.GameFeel;
using UnityEditor;

namespace Thnguyet.GameFeel
{	
	/// <summary>
	/// Adds a dedicated Tools menu into the top bar More Mountains entry to delete all saved data
	/// </summary>
	public static class SaveLoadMenu 
	{
		[MenuItem("Tools/Thnguyet/GameFeel/Delete all saved data",false,31)]
		/// <summary>
		/// Adds a menu item to reset all data saved by the SaveLoadManager. No turning back.
		/// </summary>
		private static void ResetAllSavedInventories()
		{
			SaveLoadManager.DeleteAllSaveFiles();
			Debug.LogFormat ("All Save Files Deleted");
		}
	}
}