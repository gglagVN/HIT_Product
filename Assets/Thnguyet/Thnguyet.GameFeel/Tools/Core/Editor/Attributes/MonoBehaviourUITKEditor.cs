using System;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

namespace Thnguyet.GameFeel
{
	public class InspectorGroupData
	{
		public bool GroupIsOpen;
		public InspectorGroupAttribute GroupAttribute;
		public List<SerializedProperty> PropertiesList = new List<SerializedProperty>();
		public HashSet<string> GroupHashSet = new HashSet<string>();
		public Color GroupColor;

		public void ClearGroup()
		{
			GroupAttribute = null;
			GroupHashSet.Clear();
			PropertiesList.Clear();
		}
	}
	
	[CanEditMultipleObjects]
	[CustomEditor(typeof(FeelMonoBehaviour), true)] 
	public class MonoBehaviourUITKEditor : Editor
	{
		public StyleSheet EditorStyleSheet;
		
		public bool DrawerInitialized;
		public Dictionary<string, InspectorGroupData> GroupData ;
		public List<SerializedProperty> PropertiesList;
		private bool _requiresConstantRepaint;
		private bool _requiresConstantRepaintOnlyWhenPlaying;
		private FeelMonoBehaviour _targetMonoBehaviourGameObject;
		private bool _targetMonoBehaviourIsNotNull;
		protected bool _shouldDrawBase = true;
		protected string _targetTypeName;
		private string[] _mmHiddenPropertiesToHide;
		private bool _hasMMHiddenProperties = false;

		public override bool RequiresConstantRepaint()
		{
			if (_requiresConstantRepaintOnlyWhenPlaying)
			{
				return Application.isPlaying && _targetMonoBehaviourIsNotNull && _targetMonoBehaviourGameObject.enabled;
			}
			else
			{
				return _requiresConstantRepaint;
			}
		}
		
		protected virtual void Initialization()
		{
			if (DrawerInitialized && PropertiesList != null)
			{
				return;
			}

			_shouldDrawBase = true;
			GroupData = new Dictionary<string, InspectorGroupData>();
			PropertiesList = new List<SerializedProperty>();
			_targetTypeName = target.GetType().Name;
			
			_targetMonoBehaviourGameObject = (FeelMonoBehaviour)target;
			if (_targetMonoBehaviourGameObject != null)
			{
				_targetMonoBehaviourIsNotNull = true;
			}
			
			_requiresConstantRepaint = serializedObject.targetObject.GetType().GetCustomAttribute<RequiresConstantRepaintAttribute>() != null;
			_requiresConstantRepaintOnlyWhenPlaying = serializedObject.targetObject.GetType().GetCustomAttribute<RequiresConstantRepaintOnlyWhenPlayingAttribute>() != null; 
			
			List<FieldInfo> fieldInfoList;
			InspectorGroupAttribute previousGroupAttribute = default;
			int fieldInfoLength = MonoBehaviourFieldInfo.GetFieldInfo(target, out fieldInfoList);

			for (int i = 0; i < fieldInfoLength; i++)
			{
				InspectorGroupAttribute group = Attribute.GetCustomAttribute(fieldInfoList[i], typeof(InspectorGroupAttribute)) as InspectorGroupAttribute;
				InspectorGroupData groupData;
				if (group == null)
				{
					if (previousGroupAttribute != null && previousGroupAttribute.GroupAllFieldsUntilNextGroupAttribute)
					{
						_shouldDrawBase = false;
						if (!GroupData.TryGetValue(previousGroupAttribute.GroupName, out groupData))
						{
							GroupData.Add(previousGroupAttribute.GroupName, new InspectorGroupData
							{
								GroupAttribute = previousGroupAttribute,
								GroupHashSet = new HashSet<string> { fieldInfoList[i].Name },
								GroupColor = FeelColors.GetColorAt(previousGroupAttribute.GroupColorIndex)
							});
						}
						else
						{
							groupData.GroupColor = FeelColors.GetColorAt(previousGroupAttribute.GroupColorIndex);
							groupData.GroupHashSet.Add(fieldInfoList[i].Name);
						}
					}

					continue;
				}
                
				previousGroupAttribute = group;

				if (!GroupData.TryGetValue(group.GroupName, out groupData))
				{
					bool fallbackOpenState = true;
					if (group.ClosedByDefault) { fallbackOpenState = false; }
					bool groupIsOpen = EditorPrefs.GetBool(string.Format($"{group.GroupName}{fieldInfoList[i].Name}{target.GetInstanceID()}"), fallbackOpenState);
					GroupData.Add(group.GroupName, new InspectorGroupData
					{
						GroupAttribute = group,
						GroupColor = FeelColors.GetColorAt(previousGroupAttribute.GroupColorIndex),
						GroupHashSet = new HashSet<string> { fieldInfoList[i].Name }, GroupIsOpen = groupIsOpen });
				}
				else
				{
					groupData.GroupHashSet.Add(fieldInfoList[i].Name);
					groupData.GroupColor = FeelColors.GetColorAt(previousGroupAttribute.GroupColorIndex);
				}
			}

			SerializedProperty iterator = serializedObject.GetIterator();

			if (iterator.NextVisible(true))
			{
				do
				{
					FillPropertiesList(iterator);
				} while (iterator.NextVisible(false));
			}
			DrawerInitialized = true;
		}
		
		public void FillPropertiesList(SerializedProperty serializedProperty)
		{
			bool shouldClose = false;

			foreach (KeyValuePair<string, InspectorGroupData> pair in GroupData)
			{
				if (pair.Value.GroupHashSet.Contains(serializedProperty.name))
				{
					SerializedProperty property = serializedProperty.Copy();
					shouldClose = true;
					pair.Value.PropertiesList.Add(property);
					break;
				}
			}

			if (!shouldClose)
			{
				SerializedProperty property = serializedProperty.Copy();
				PropertiesList.Add(property);
			}
		}
		
		public override VisualElement CreateInspectorGUI()
		{
			Initialization();
			
			VisualElement root = new VisualElement();
			root.styleSheets.Add(EditorStyleSheet);
			
			// Draw the script field
			SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");

			if (PropertiesList.Count == 0)
			{
				return root;
			}

			if (_shouldDrawBase)
			{
				VisualElement defaultInspector = new VisualElement();
				InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);
				root.Add(defaultInspector);
				return root;
			}
			
			PropertyField scriptField = new PropertyField(scriptProperty);
			scriptField.SetEnabled(false); 
			root.Add(scriptField);

			foreach (KeyValuePair<string, InspectorGroupData> pair in GroupData)
			{
				DrawGroup(pair.Value, root);
			}
			
			serializedObject.ApplyModifiedProperties();

			return root;
		}
		
		protected virtual void DrawGroup(InspectorGroupData groupData, VisualElement root)
		{
			Foldout foldout = new Foldout();
			foldout.text = groupData.GroupAttribute.GroupName;
			foldout.value = groupData.GroupIsOpen;
			foldout.AddToClassList("mm-foldout");
			foldout.style.borderLeftColor = groupData.GroupColor;
			foldout.viewDataKey = target.name + "-" + _targetTypeName + groupData.GroupAttribute.GroupName;
			root.Add(foldout);
			
			var toggleElement = foldout.Q<Toggle>();
			toggleElement.AddToClassList("mm-foldout-toggle");
			
			for (int i = 0; i < groupData.PropertiesList.Count; i++)
			{
				DrawChild(i, foldout, root);
			}

			void DrawChild(int i, Foldout foldout, VisualElement root)
			{
				if ((_hasMMHiddenProperties) && (_mmHiddenPropertiesToHide.Contains(groupData.PropertiesList[i].name)))
				{
					return;
				}
				PropertyField field = new PropertyField(groupData.PropertiesList[i]);
				field.label = ObjectNames.NicifyVariableName(groupData.PropertiesList[i].name);
				field.tooltip = groupData.PropertiesList[i].tooltip;
				foldout.Add(field);
			}
		}
	}
}
