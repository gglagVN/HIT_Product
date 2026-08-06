using Thnguyet.GameFeel;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Thnguyet.GameFeel.Feedbacks
{
	[CustomPropertyDrawer(typeof(FeelSpringColor))]
	class SpringColorPropertyDrawer : PropertyDrawer
	{
		protected float _lastTarget;
		protected float _max;
		
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			
			SerializedProperty _colorSpring = property.FindPropertyRelative("ColorSpring");
			root.Add(new PropertyField(_colorSpring));
			
			return root;
		}
		
	}
}


