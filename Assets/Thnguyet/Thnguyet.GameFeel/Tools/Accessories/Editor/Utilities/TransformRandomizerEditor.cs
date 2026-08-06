using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Custom editor for the TransformRandomizer class
	/// </summary>
	[CustomEditor(typeof(TransformRandomizer), true)]
	[CanEditMultipleObjects]
	public class TransformRandomizerEditor : Editor
	{
		/// <summary>
		/// On inspector we handle undo and display a test button
		/// </summary>
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			Undo.RecordObject(target, "Modified TransformRandomizer");
			DrawDefaultInspector();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Test", EditorStyles.boldLabel);

			if (GUILayout.Button("Randomize"))
			{
				foreach (TransformRandomizer randomizer in targets)
				{
					randomizer.Randomize();
				}
			}
		}
	}
}