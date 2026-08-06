using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	#if GAMEFEEL_PHYSICS2D
	/// <summary>
	/// Custom editor for the TilemapGenerator, handles generate button and reorderable layers
	/// </summary>
	[CustomEditor(typeof(TilemapGenerator), true)]
	[CanEditMultipleObjects]
	public class TilemapGeneratorEditor : Editor
	{
    
		protected FeelReorderableList _list;

		protected virtual void OnEnable()
		{
			_list = new FeelReorderableList(serializedObject.FindProperty("Layers"));
			_list.elementNameProperty = "Layer";
			_list.elementDisplayType = FeelReorderableList.ElementDisplayType.Expandable;
		}
        
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
            
			DrawPropertiesExcluding(serializedObject,  "Layers");
			EditorGUILayout.Space(10);
			_list.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
            
			if (GUILayout.Button("Generate"))
			{
				(target as TilemapGenerator).Generate();
			}
		}
	}
	#endif
}
