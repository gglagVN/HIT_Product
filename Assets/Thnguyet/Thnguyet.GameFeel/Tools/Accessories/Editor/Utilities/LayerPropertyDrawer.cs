using UnityEditor;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	[CustomPropertyDrawer(typeof(FeelLayer))]
	public class LayerPropertyDrawer : PropertyDrawer
	{
		#if  UNITY_EDITOR
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, GUIContent.none, property);
			SerializedProperty layerIndex = property.FindPropertyRelative("_layerIndex");
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			if (layerIndex != null)
			{
				layerIndex.intValue = EditorGUI.LayerField(position, layerIndex.intValue);
			}
			EditorGUI.EndProperty();
		}
		#endif
	}
}
