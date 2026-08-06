using UnityEngine;
using System.Collections;
using Thnguyet.GameFeel;
using UnityEditor;

namespace Thnguyet.GameFeel
{	
	/// <summary>
	/// This class adds a Thnguyet GameFeel entry in Unity's top menu, allowing to enable/disable the help texts from the engine's inspectors
	/// </summary>
	public static class MenuHelp
	{
		public static bool SettingCached = false;
		public static bool CachedHelpEnabled = false;
		
		[MenuItem("Tools/Thnguyet/GameFeel/Enable Help in Inspectors", false,0)]
		/// <summary>
		/// Adds a menu item to enable help
		/// </summary>
		private static void EnableHelpInInspectors()
		{
			SetHelpEnabled(true);
		}

		[MenuItem("Tools/Thnguyet/GameFeel/Enable Help in Inspectors", true)]
		/// <summary>
		/// Conditional method to determine if the "enable help" entry should be greyed or not
		/// </summary>
		private static bool EnableHelpInInspectorsValidation()
		{
			return !HelpEnabled;
		}

		[MenuItem("Tools/Thnguyet/GameFeel/Disable Help in Inspectors", false,1)]
		/// <summary>
		/// Adds a menu item to disable help
		/// </summary>
		private static void DisableHelpInInspectors()
		{
			SetHelpEnabled(false);
		}
		 
		[MenuItem("Tools/Thnguyet/GameFeel/Disable Help in Inspectors", true)]
		/// <summary>
		/// Conditional method to determine if the "disable help" entry should be greyed or not
		/// </summary>
		private static bool DisableHelpInInspectorsValidation()
		{
			return HelpEnabled;
		}

		/// <summary>
		/// Checks editor prefs to see if help is enabled or not
		/// </summary>
		/// <returns><c>true</c>, if enabled was helped, <c>false</c> otherwise.</returns>
		public static bool HelpEnabled
		{
			get
			{
				if (SettingCached)
				{
					return CachedHelpEnabled;
				}
			
				if (EditorPrefs.HasKey("ShowHelpInInspectors"))
				{
					CachedHelpEnabled = EditorPrefs.GetBool("ShowHelpInInspectors");
					SettingCached = true;
					return CachedHelpEnabled;
				}
				else
				{
					EditorPrefs.SetBool("ShowHelpInInspectors",true);
					CachedHelpEnabled = true;
					SettingCached = true;
					return CachedHelpEnabled;
				}
			}
		}

		/// <summary>
		/// Sets the help enabled editor pref.
		/// </summary>
		/// <param name="status">If set to <c>true</c> status.</param>
		private static void SetHelpEnabled(bool status)
		{
			EditorPrefs.SetBool("ShowHelpInInspectors",status);
			CachedHelpEnabled = status;
			SettingCached = true;
			SceneView.RepaintAll();
		}
	}
}