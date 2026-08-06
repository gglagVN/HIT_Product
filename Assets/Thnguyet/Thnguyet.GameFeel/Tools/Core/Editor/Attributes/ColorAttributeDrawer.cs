using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thnguyet.GameFeel
{
	[CustomPropertyDrawer(typeof(ColorAttribute))]
	public class ColorAttributeDrawer : PropertyDrawer
	{
        
		#if  UNITY_EDITOR
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Color color = (attribute as ColorAttribute).color;
			Color prev = GUI.color;
			GUI.color = color;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.color = prev;
		}
		#endif
	}
}