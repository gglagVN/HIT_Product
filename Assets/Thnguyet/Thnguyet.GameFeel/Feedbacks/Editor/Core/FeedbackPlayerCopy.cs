using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// A helper class to copy and paste feedback properties
	/// </summary>
	static class FeedbackPlayerCopy
	{
		static public System.Type Type { get; private set; }
		public static readonly List<FeedbackFeedback> CopiedFeedbacks = new List<FeedbackFeedback>();
		public static readonly Dictionary<FeedbackPlayer, List<FeedbackFeedback>> RuntimeChanges = new Dictionary<FeedbackPlayer, List<FeedbackFeedback>>();

		static string[] IgnoreList = new string[]
		{
			"m_ObjectHideFlags",
			"m_CorrespondingSourceObject",
			"m_PrefabInstance",
			"m_PrefabAsset",
			"m_GameObject",
			"m_Enabled",
			"m_EditorHideFlags",
			"m_Script",
			"m_Name",
			"m_EditorClassIdentifier"
		};
		
		static FeedbackPlayerCopy()
		{
			EditorApplication.playModeStateChanged += ModeChanged;
		}

		private static void ModeChanged(PlayModeStateChange playModeState)
		{
			switch (playModeState)
			{
				case PlayModeStateChange.ExitingPlayMode:
					StoreRuntimeChanges();
					break;
        
				case PlayModeStateChange.EnteredEditMode:
					ApplyRuntimeChanges();
					break;
			}
		}

		private static void StoreRuntimeChanges()
		{
			foreach (FeedbackPlayer player in Object.FindObjectsByType<FeedbackPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(p => p.KeepPlayModeChanges))
			{
				FeedbackPlayerCopy.StoreRuntimeChanges(player);
			}
		}

		private static void ApplyRuntimeChanges()
		{
			foreach (FeedbackPlayer player in Object.FindObjectsByType<FeedbackPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(FeedbackPlayerCopy.RuntimeChanges.ContainsKey))
			{
				FeedbackPlayerCopy.ApplyRuntimeChanges(player);
			}
		}

		static public bool HasCopy()
		{
			return CopiedFeedbacks != null && CopiedFeedbacks.Count == 1;
		}

		static public bool HasMultipleCopies()
		{
			return CopiedFeedbacks != null && CopiedFeedbacks.Count > 1;
		}

		static public void Copy(FeedbackFeedback feedback)
		{
			Type feedbackType = feedback.GetType();
			FeedbackFeedback newFeedback = (FeedbackFeedback)Activator.CreateInstance(feedbackType);
			EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
			CopiedFeedbacks.Clear();
			CopiedFeedbacks.Add(newFeedback);
		}
        
		static public void CopyAll(FeedbackPlayer sourceFeedbacks)
		{
			CopiedFeedbacks.Clear();
			foreach (FeedbackFeedback feedback in sourceFeedbacks.FeedbacksList)
			{
				Type feedbackType = feedback.GetType();
				FeedbackFeedback newFeedback = (FeedbackFeedback)Activator.CreateInstance(feedbackType);
				EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
				CopiedFeedbacks.Add(newFeedback);    
			}
		}

		// Multiple Copy ----------------------------------------------------------

		static public void PasteAll(FeedbackPlayerEditorUITK targetEditor)
		{
			foreach (FeedbackFeedback feedback in FeedbackPlayerCopy.CopiedFeedbacks)
			{
				targetEditor.TargetMmfPlayer.AddFeedback(feedback);
			}
			CopiedFeedbacks.Clear();
		}
		
		// Runtime Changes

		static public void StoreRuntimeChanges(FeedbackPlayer player)
		{
			RuntimeChanges[player] = new List<FeedbackFeedback>();
			foreach (FeedbackFeedback feedback in player.FeedbacksList)
			{
				Type feedbackType = feedback.GetType();
				FeedbackFeedback newFeedback = (FeedbackFeedback)Activator.CreateInstance(feedbackType);
				EditorUtility.CopySerializedManagedFieldsOnly(feedback, newFeedback);
				RuntimeChanges[player].Add(newFeedback);    
			}
		}

		static public void ApplyRuntimeChanges(FeedbackPlayer player)
		{
			SerializedObject playerSerialized = new SerializedObject(player);
			playerSerialized.Update();
			Undo.RecordObject(player, "Replace all feedbacks");
			player.FeedbacksList.Clear();
			foreach (FeedbackFeedback feedback in FeedbackPlayerCopy.RuntimeChanges[player])
			{
				player.AddFeedback(feedback, true);
			}
			playerSerialized.ApplyModifiedProperties();
			PrefabUtility.RecordPrefabInstancePropertyModifications(player);
			if (FeedbackPlayerConfiguration.Instance.AutoDisableKeepPlaymodeChanges)
			{
				playerSerialized.Update();
				player.KeepPlayModeChanges = false;    
				playerSerialized.ApplyModifiedProperties();
			}
			player.RefreshCache();
		}
	}
}