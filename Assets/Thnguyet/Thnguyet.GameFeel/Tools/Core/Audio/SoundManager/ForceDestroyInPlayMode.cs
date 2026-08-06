using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// This class will automatically destroy the object when entering play mode, and will destroy it again when exiting play mode.
	/// This is used for instance by the sound feedbacks to ensure that test audio sources created outside of play mode don't persist in your scene
	/// </summary>
	public class ForceDestroyInPlayMode : MonoBehaviour
	{
		#if UNITY_EDITOR
		static ForceDestroyInPlayMode()
		{
			EditorApplication.playModeStateChanged += ModeChanged;
		}

		private static void ModeChanged(PlayModeStateChange playModeState)
		{
			switch (playModeState)
			{
				case PlayModeStateChange.EnteredEditMode:
					DeleteAll();
					break;
			}
		}

		static void DeleteAll()
		{
			ForceDestroyInPlayMode[] sounds = FindObjectsByType<ForceDestroyInPlayMode>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			foreach (var sound in sounds)
			{
				sound.Delete();
			}
		}
		#endif
	
		void Awake()
		{
			if (Application.isPlaying)
			{
				Delete();
			}
		}

		void Delete()
		{
			DestroyImmediate(this.gameObject);
		}
	}
}

