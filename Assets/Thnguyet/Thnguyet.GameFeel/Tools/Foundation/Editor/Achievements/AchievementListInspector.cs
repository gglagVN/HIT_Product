using UnityEngine;
using UnityEditor;

namespace Thnguyet.GameFeel
{
	[CustomEditor(typeof(AchievementList),true)]
	/// <summary>
	/// Custom inspector for the AchievementList scriptable object. 
	/// </summary>
	public class AchievementListInspector : Editor 
	{
		/// <summary>
		/// When drawing the GUI, adds a "Reset Achievements" button, that does exactly what you think it does.
		/// </summary>
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector ();
			AchievementList achievementList = (AchievementList)target;
			if(GUILayout.Button("Reset Achievements"))
			{
				achievementList.ResetAchievements();
			}	
			EditorUtility.SetDirty (achievementList);
		}
	}
}