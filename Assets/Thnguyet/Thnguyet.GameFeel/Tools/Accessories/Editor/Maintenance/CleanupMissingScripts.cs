using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// This class lets you clean all missing scripts on a selection of gameobjects
	/// </summary>
	public class CleanupMissingScripts : MonoBehaviour
	{
		/// <summary>
		/// Processes the cleaning of gameobjects for all missing scripts on them
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Cleanup missing scripts on selected GameObjects", false, 504)]
		protected static void DoCleanupMissingScripts()
		{
			Object[] collectedDeepHierarchy = EditorUtility.CollectDeepHierarchy(Selection.gameObjects);
			int removedComponentsCounter = 0;
			int gameobjectsAffectedCounter = 0;
			foreach (Object targetObject in collectedDeepHierarchy)
			{
				if (targetObject is GameObject gameObject)
				{
					int amountOfMissingScripts = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject);
					if (amountOfMissingScripts > 0)
					{
						Undo.RegisterCompleteObjectUndo(gameObject, "Removing missing scripts");
						GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
						removedComponentsCounter += amountOfMissingScripts;
						gameobjectsAffectedCounter++;
					}
				}
			}
			FeelDebug.DebugLogInfo("[CleanupMissingScripts] Removed " + removedComponentsCounter + " missing scripts from " + gameobjectsAffectedCounter + " GameObjects");
		}
	}
}