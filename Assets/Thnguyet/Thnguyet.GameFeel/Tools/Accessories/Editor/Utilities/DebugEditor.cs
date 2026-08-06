using UnityEngine;
using System.Collections;
using Thnguyet.GameFeel;
using UnityEditor;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// An editor class used to display menu items 
	/// </summary>
	public class DebugEditor
	{
		/// <summary>
		/// Adds a menu item to enable debug logs
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Enable Debug Logs", false, 100)]
		private static void EnableDebugLogs()
		{
			FeelDebug.SetDebugLogsEnabled(true);
		}

		/// <summary>
		/// Conditional method to determine if the "enable debug log" entry should be greyed or not
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Enable Debug Logs", true)]
		private static bool EnableDebugLogsValidation()
		{
			return !FeelDebug.DebugLogsEnabled;
		}

		/// <summary>
		/// Adds a menu item to disable debug logs
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Disable Debug Logs", false, 101)]
		private static void DisableDebugLogs()
		{
			FeelDebug.SetDebugLogsEnabled(false);
		}

		/// <summary>
		/// Conditional method to determine if the "disable debug log" entry should be greyed or not
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Disable Debug Logs", true)]
		private static bool DisableDebugLogsValidation()
		{
			return FeelDebug.DebugLogsEnabled;
		}

		/// <summary>
		/// Adds a menu item to enable debug logs
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Enable Debug Draws", false, 102)]
		private static void EnableDebugDraws()
		{
			FeelDebug.SetDebugDrawEnabled(true);
		}

		[MenuItem("Tools/Thnguyet/GameFeel/Enable Debug Draws", true)]
		/// <summary>
		/// Conditional method to determine if the "enable debug log" entry should be greyed or not
		/// </summary>
		private static bool EnableDebugDrawsValidation()
		{
			return !FeelDebug.DebugDrawEnabled;
		}

		[MenuItem("Tools/Thnguyet/GameFeel/Disable Debug Draws", false, 103)]
		/// <summary>
		/// Adds a menu item to disable debug logs
		/// </summary>
		private static void DisableDebugDraws()
		{
			FeelDebug.SetDebugDrawEnabled(false);
		}

		[MenuItem("Tools/Thnguyet/GameFeel/Disable Debug Draws", true)]
		/// <summary>
		/// Conditional method to determine if the "disable debug log" entry should be greyed or not
		/// </summary>
		private static bool DisableDebugDrawsValidation()
		{
			return FeelDebug.DebugDrawEnabled;
		}

	}
}