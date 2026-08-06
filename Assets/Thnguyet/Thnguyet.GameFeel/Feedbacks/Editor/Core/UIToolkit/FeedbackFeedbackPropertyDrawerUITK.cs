using System;
using System.Collections.Generic;
using System.Reflection;
using Thnguyet.GameFeel;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Thnguyet.GameFeel.Feedbacks
{
	public class FeedbackInspectorDrawData
	{
		public int Index;
		public Foldout FeedbackFoldout;
		public SerializedProperty CurrentProperty;
		public FeedbackFeedback Feedback;
		public FeedbackPlayerEditorUITK PlayerEditor;
		public Action<SerializedProperty> OnAnyValueChanged;
		public Action<SerializedProperty, FeedbackInspectorGroupData, FeedbackFeedback> OnFeedbackFieldValueChanged;
		public Dictionary<FeedbackInspectorGroupData, FeedbackPlayerEditorUITK.FeedbackFeedbackGroupExtrasContainerData> FeedbackGroupsDictionary;
		public Dictionary<string, FeedbackInspectorGroupData> GroupDataDictionary;
		public List<SerializedProperty> PropertiesList;
		public Sprite SetupRequiredIcon;
	}
	
	public class FeedbackInspectorGroupData
	{
		public bool GroupIsOpen;
		public FeedbackInspectorGroupAttribute GroupAttribute;
		public List<SerializedProperty> PropertiesList = new List<SerializedProperty>();
		public HashSet<string> GroupHashSet = new HashSet<string>();
		public Color GroupColor;
		public bool Initialized = false;

		public void ClearGroup()
		{
			GroupAttribute = null;
			GroupHashSet.Clear();
			PropertiesList.Clear();
			Initialized = false;
		}
	}
	
	public class FeedbackFeedbackPropertyDrawerUITK 
	{
		private const string _channelFieldName = "Channel";
		private const string _channelModeFieldName = "ChannelMode";
		private const string _channelDefinitionFieldName = "ChannelDefinition";
		private const string _automatedTargetAcquisitionName = "AutomatedTargetAcquisition";
		private const string _timingFieldName = "Timing";
		protected const string _customInspectorButtonPropertyName = "FeedbackButton";

		protected const string _mmfInspectorClassName = "mm-mmf-inspector";
		protected const string _mmfContainerClassName = "mm-mmf-container";
		protected const string _mmfGroupClassName = "mm-mmf-group";
		protected const string _mmfFieldClassName = "mm-mmf-field";
		protected const string _feedbackGroupHeaderExtrasClassName = "mm-feedback-group-header-extras";
		
		public static VisualElement DrawInspector(FeedbackInspectorDrawData drawData)
		{
			// create our inspector root
			VisualElement root = new VisualElement();
			root.RegisterCallback<PointerDownEvent>(evt => { evt.StopPropagation(); });
			root.AddToClassList(_mmfInspectorClassName); 
			
			// initialize our data containers
			Dictionary<string, FeedbackInspectorGroupData> groupDataDictionary = new Dictionary<string, FeedbackInspectorGroupData>();
			List<SerializedProperty> propertiesList = new List<SerializedProperty>();
			drawData.GroupDataDictionary = groupDataDictionary;
			drawData.PropertiesList = propertiesList;
			
			Initialization(drawData);
			root.Add(DrawContainer(drawData));
			
			// we initialize our groupdata with a delay to make sure OnFeedbackFieldValueChanged isn't called on the first render of each field
			root.schedule.Execute(() =>
			{
				foreach (var keyPair in groupDataDictionary)
				{
					keyPair.Value.Initialized = true;
				}
			}).StartingIn(1000);
			
			return root;
		}
		
		protected static void Initialization(FeedbackInspectorDrawData drawData)
		{
			List<FieldInfo> fieldInfoList;
			FeedbackInspectorGroupAttribute previousGroupAttribute = default;
			int fieldInfoLength = FeedbackFieldInfo.GetFieldInfo(drawData.Feedback, out fieldInfoList);
            
			for (int i = 0; i < fieldInfoLength; i++)
			{
				FeedbackInspectorGroupAttribute group = Attribute.GetCustomAttribute(fieldInfoList[i], typeof(FeedbackInspectorGroupAttribute)) as FeedbackInspectorGroupAttribute;

				FeedbackInspectorGroupData groupData;
				if (group == null)
				{
					if (previousGroupAttribute != null && previousGroupAttribute.GroupAllFieldsUntilNextGroupAttribute)
					{
						if (!drawData.GroupDataDictionary.TryGetValue(previousGroupAttribute.GroupName, out groupData))
						{
							if (!ShouldSkipGroup(previousGroupAttribute.GroupName, drawData.Feedback))
							{
								drawData.GroupDataDictionary.Add(previousGroupAttribute.GroupName, new FeedbackInspectorGroupData
								{
									GroupAttribute = previousGroupAttribute,
									GroupHashSet = new HashSet<string> { fieldInfoList[i].Name },
									GroupColor = FeedbacksColors.GetColorAt(previousGroupAttribute.GroupColorIndex)
								});
							}
						}
						else
						{
							groupData.GroupColor = FeedbacksColors.GetColorAt(previousGroupAttribute.GroupColorIndex);
							groupData.GroupHashSet.Add(fieldInfoList[i].Name);
						}
					}

					continue;
				}
                
				previousGroupAttribute = group;

				if (!drawData.GroupDataDictionary.TryGetValue(group.GroupName, out groupData))
				{
					bool fallbackOpenState = true;
					if (group.ClosedByDefault) { fallbackOpenState = false; }
					bool groupIsOpen = EditorPrefs.GetBool(string.Format($"{group.GroupName}{fieldInfoList[i].Name}{drawData.Feedback.UniqueID}"), fallbackOpenState);

					if (!ShouldSkipGroup(previousGroupAttribute.GroupName, drawData.Feedback))
					{
						drawData.GroupDataDictionary.Add(group.GroupName, new FeedbackInspectorGroupData
						{
							GroupAttribute = group,
							GroupColor = FeedbacksColors.GetColorAt(previousGroupAttribute.GroupColorIndex),
							GroupHashSet = new HashSet<string> { fieldInfoList[i].Name }, GroupIsOpen = groupIsOpen 
						});	
					}
				}
				else
				{
					groupData.GroupHashSet.Add(fieldInfoList[i].Name);
					groupData.GroupColor = FeedbacksColors.GetColorAt(previousGroupAttribute.GroupColorIndex);
				}
			}

			if (drawData.CurrentProperty.NextVisible(true))
			{
				do
				{
					FillPropertiesList(drawData.CurrentProperty, drawData.GroupDataDictionary, drawData.PropertiesList);
				} while (drawData.CurrentProperty.NextVisible(false));
			}
		}
		
		protected static bool ShouldSkipGroup(string groupName, FeedbackFeedback feedback)
		{
			bool skip = false;
            
			if (groupName == FeedbackFeedback._randomnessGroupName && !feedback.HasRandomness)
			{
				skip = true;
			}

			if (groupName == FeedbackFeedback._rangeGroupName && !feedback.HasRange)
			{
				skip = true;
			}

			if (groupName == FeedbackFeedback._automaticSetupGroupName && !feedback.HasAutomaticShakerSetup)
			{
				skip = true;
			}

			return skip;
		}
        
		public static void FillPropertiesList(SerializedProperty serializedProperty, Dictionary<string, FeedbackInspectorGroupData> groupDataDictionary, List<SerializedProperty> propertiesList)
		{
			bool shouldClose = false;

			foreach (KeyValuePair<string, FeedbackInspectorGroupData> pair in groupDataDictionary)
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
				propertiesList.Add(property);
			}
		}
        
		protected static VisualElement DrawContainer(FeedbackInspectorDrawData drawData)
		{
			VisualElement root = new VisualElement();
			root.AddToClassList(_mmfContainerClassName);
			
			if (drawData.PropertiesList.Count == 0)
			{
				return root;
			}
            
			foreach (KeyValuePair<string, FeedbackInspectorGroupData> pair in drawData.GroupDataDictionary)
			{
				VisualElement group = DrawGroup(pair.Value, drawData);
				root.Add(group);
			}

			return root;
		}
		
		protected static VisualElement DrawGroup(FeedbackInspectorGroupData groupData, FeedbackInspectorDrawData drawData)
		{
			VisualElement root = new VisualElement();
			root.RegisterCallback<PointerDownEvent>(evt => { evt.StopPropagation(); }); 
			root.AddToClassList(_mmfGroupClassName);
			
			Foldout foldout = new Foldout();
			foldout.text = groupData.GroupAttribute.GroupName;
			foldout.value = groupData.GroupIsOpen;
			foldout.AddToClassList(FeedbackPlayerEditorUITK._foldoutClassName);
			foldout.style.borderLeftColor = groupData.GroupColor;
			foldout.viewDataKey = drawData.Feedback.UniqueID + "-" + drawData.Feedback.Label + "-" + groupData.GroupAttribute.GroupName;
			root.Add(foldout);
			
			var toggleElement = foldout.Q<Toggle>();
			toggleElement.AddToClassList(FeedbackPlayerEditorUITK._foldoutToggleClassName);

			VisualElement headerExtrasContainer = new VisualElement();
			headerExtrasContainer.AddToClassList(_feedbackGroupHeaderExtrasClassName);
			headerExtrasContainer.pickingMode = PickingMode.Ignore;
			foldout.parent.Insert(1, headerExtrasContainer);
				
			FeedbackPlayerEditorUITK.FeedbackFeedbackGroupExtrasContainerData feedbackGroupExtrasContainerData = new FeedbackPlayerEditorUITK.FeedbackFeedbackGroupExtrasContainerData();
			feedbackGroupExtrasContainerData.HeaderExtrasContainer = headerExtrasContainer;
			feedbackGroupExtrasContainerData.GroupData = groupData;
			feedbackGroupExtrasContainerData.DrawData = drawData;
			drawData.FeedbackGroupsDictionary.Add(groupData, feedbackGroupExtrasContainerData);
			DrawGroupExtrasContainer(feedbackGroupExtrasContainerData);
			
			// foldout contents
			foldout.schedule.Execute(() =>
			{
				if (foldout.value) { DrawFoldoutContents(); }
			}).ExecuteLater(1);
				
			EventCallback<ChangeEvent<bool>> callback = null;
			callback = evt =>
			{
				if (evt.newValue) 
				{
					if (foldout.childCount == 0) 
					{
						DrawFoldoutContents();
						foldout.UnregisterValueChangedCallback(callback);
					}
				}
			};
			foldout.RegisterValueChangedCallback(callback);

			void DrawFoldoutContents()
			{
				for (int i = 0; i < groupData.PropertiesList.Count; i++)
				{
					DrawChild(i, foldout, root);
				}
			}

			void DrawChild(int i, Foldout foldout, VisualElement root)
			{
				if (!drawData.Feedback.HasChannel 
				    && (groupData.PropertiesList[i].name == _channelFieldName
				        || groupData.PropertiesList[i].name == _channelModeFieldName
				        || groupData.PropertiesList[i].name == _channelDefinitionFieldName))
				{
					return;
				}
				
				bool shouldDraw = !((groupData.PropertiesList[i].name == _automatedTargetAcquisitionName) && (!drawData.Feedback.HasAutomatedTargetAcquisition));
				if (!shouldDraw)
				{
					return;
				}
				
				if (!DrawCustomInspectors(groupData.PropertiesList[i], drawData.Feedback, foldout))
				{
					PropertyField field = new PropertyField(groupData.PropertiesList[i]);
					field.label = ObjectNames.NicifyVariableName(groupData.PropertiesList[i].name);
					field.name = groupData.PropertiesList[i].name;
					field.tooltip = groupData.PropertiesList[i].tooltip;
					field.AddToClassList(_mmfFieldClassName);
					field.Bind(groupData.PropertiesList[i].serializedObject);
					field.TrackPropertyValue(groupData.PropertiesList[i], drawData.OnAnyValueChanged);

					if (field.name == "Label")
					{
						field?.RegisterCallback<ChangeEvent<string>>(evt =>
						{
							drawData.FeedbackFoldout.text = drawData.PlayerEditor.DetermineFeedbackLabel(drawData.Index, drawData.Feedback.GetType());
						});
					}
					
					field.RegisterValueChangeCallback(evt =>
					{
						if (groupData.Initialized)
						{
							drawData.OnFeedbackFieldValueChanged(groupData.PropertiesList[i], groupData, drawData.Feedback);
						}
					});
					foldout.Add(field);
					
					// we register callbacks for all the nested fields under Timing
					if (field.name == _timingFieldName) 
					{
						RegisterNestedCallbacks(field, groupData.PropertiesList[i], groupData, drawData); 
					}
				}
			}

			return root;
		}
		
		private static void RegisterNestedCallbacks(VisualElement field, SerializedProperty property, FeedbackInspectorGroupData groupData, FeedbackInspectorDrawData drawData)
		{
			field.schedule.Execute(() => // we delay the execution to avoid calling the callback before the Timing foldout is fully built 
			{
				foreach (var child in field.Children())
				{
					if (child is PropertyField genericField) 
					{
						genericField.RegisterValueChangeCallback(evt => drawData.OnFeedbackFieldValueChanged(property, groupData, drawData.Feedback));
					}
					if (child.childCount > 0)
					{
						RegisterNestedCallbacks(child, property, groupData, drawData);
					}
				}
			}).StartingIn(100);
		}

		public static void DrawGroupExtrasContainer(FeedbackPlayerEditorUITK.FeedbackFeedbackGroupExtrasContainerData groupExtrasContainerData)
		{
			groupExtrasContainerData.HeaderExtrasContainer.Clear();
			
			if (groupExtrasContainerData.GroupData.GroupAttribute.RequiresSetup && groupExtrasContainerData.DrawData.Feedback.RequiresSetup)
			{
				VisualElement setupRequiredIcon = new VisualElement();
				setupRequiredIcon.AddToClassList(FeedbackPlayerEditorUITK._iconClassName);
				setupRequiredIcon.AddToClassList(FeedbackPlayerEditorUITK._setupRequiredIconClassName);
				setupRequiredIcon.style.backgroundImage = new StyleBackground(groupExtrasContainerData.DrawData.SetupRequiredIcon);
				groupExtrasContainerData.HeaderExtrasContainer.Add(setupRequiredIcon);
			}
		}
		
		protected static bool DrawCustomInspectors(SerializedProperty currentProperty, FeedbackFeedback feedback, Foldout foldout)
		{
			if (feedback.HasCustomInspectors)
			{
				switch (currentProperty.type)
				{
					case _customInspectorButtonPropertyName:
						FeedbackButton myButton = (FeedbackButton)(currentProperty.FGetObjectValue());
						
						Button newButton = new Button(() => myButton.TargetMethod());
						newButton.text = myButton.ButtonText;
						foldout.Add(newButton);
						
						return true;
				}
			}

			return false;
		}
    }
}
