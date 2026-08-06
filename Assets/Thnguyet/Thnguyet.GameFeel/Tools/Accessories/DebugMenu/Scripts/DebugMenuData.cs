using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

#if GAMEFEEL_UI

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to store and display a reorderable list of menu items
	/// </summary>
	[Serializable]
	public class DebugMenuItemList : ReorderableArray<DebugMenuItem>
	{

	}

	[Serializable]
	public class DebugMenuTabData
	{
		public string Name = "TabName";
		public bool Active = true;
		[ReorderableAttribute]
		public DebugMenuItemList MenuItems;
	}

	/// <summary>
	/// A class used to store a menu item
	/// </summary>
	[Serializable]
	public class DebugMenuItem
	{
		// EDITOR NAME
		public string Name;
		public bool Active = true;
		public enum DebugMenuItemTypes { Title, Spacer, Button, Checkbox, Slider, Text, Value, Choices }

		public DebugMenuItemTypes Type = DebugMenuItemTypes.Title;

		// TITLE
		[EnumCondition("Type", (int)DebugMenuItemTypes.Title)]
		public string TitleText = "Title text";

		// TEXT
		public enum DebugMenuItemTextTypes { Tiny, Small, Long }
		[EnumCondition("Type", (int)DebugMenuItemTypes.Text)]
		public DebugMenuItemTextTypes TextType = DebugMenuItemTextTypes.Tiny;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Text)]
		public string TextContents = "Lorem ipsum dolor sit amet";

		// CHOICES 
		public enum DebugMenuItemChoicesTypes { TwoChoices, ThreeChoices }
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public DebugMenuItemChoicesTypes ChoicesType = DebugMenuItemChoicesTypes.TwoChoices;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public string ChoiceOneText;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public string ChoiceOneEventName = "ChoiceOneEvent";
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public string ChoiceTwoText;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public string ChoiceTwoEventName = "ChoiceTwoEvent";
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public string ChoiceThreeText;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public string ChoiceThreeEventName = "ChoiceThreeEvent";
		[EnumCondition("Type", (int)DebugMenuItemTypes.Choices)]
		public int SelectedChoice = 0;

		// VALUE
		[EnumCondition("Type", (int)DebugMenuItemTypes.Value)]
		public string ValueLabel = "Value Label";
		[EnumCondition("Type", (int)DebugMenuItemTypes.Value)]
		public string ValueInitialValue = "255";
		[EnumCondition("Type", (int)DebugMenuItemTypes.Value)]
		public int ValueMMRadioReceiverChannel = 0;

		// BUTTON
		public enum DebugMenuItemButtonTypes { Border, Full }
		[EnumCondition("Type", (int)DebugMenuItemTypes.Button)]
		public string ButtonText = "Button text";
		[EnumCondition("Type", (int)DebugMenuItemTypes.Button)]
		public DebugMenuItemButtonTypes ButtonType = DebugMenuItemButtonTypes.Border;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Button)]
		public string ButtonEventName = "Button";

		// SPACER
		public enum DebugMenuItemSpacerTypes { Small, Big }
		[EnumCondition("Type", (int)DebugMenuItemTypes.Spacer)]
		public DebugMenuItemSpacerTypes SpacerType = DebugMenuItemSpacerTypes.Small;

		// CHECKBOX
		[EnumCondition("Type", (int)DebugMenuItemTypes.Checkbox)]
		public string CheckboxText;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Checkbox)]
		public bool CheckboxInitialState = false;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Checkbox)]
		public string CheckboxEventName = "CheckboxEventName";

		// SLIDER
		[EnumCondition("Type", (int)DebugMenuItemTypes.Slider)]
		public DebugMenuItemSlider.Modes SliderMode = DebugMenuItemSlider.Modes.Float;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Slider)]
		public string SliderText;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Slider)]
		public float SliderRemapZero = 0f;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Slider)]
		public float SliderRemapOne = 1f;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Slider)]
		public float SliderInitialValue = 0f;
		[EnumCondition("Type", (int)DebugMenuItemTypes.Slider)]
		public string SliderEventName = "Slider";

		[Hidden]
		public DebugMenuItemSlider TargetSlider;
		[Hidden]
		public DebugMenuItemButton TargetButton;
		[Hidden]
		public DebugMenuItemCheckbox TargetCheckbox;
	}

	/// <summary>
	/// A data class used to store the contents of a debug menu
	/// </summary>
	[CreateAssetMenu(fileName = "DebugMenuData", menuName = "Thnguyet/GameFeel/DebugMenu/DebugMenuData")]
	public class DebugMenuData : ScriptableObject
	{
		[Header("Prefabs")]
		public DebugMenuItemTitle TitlePrefab;
		public DebugMenuItemButton ButtonPrefab;
		public DebugMenuItemButton ButtonBorderPrefab;
		public DebugMenuItemCheckbox CheckboxPrefab;
		public DebugMenuItemSlider SliderPrefab;
		public GameObject SpacerSmallPrefab;
		public GameObject SpacerBigPrefab;
		public DebugMenuItemText TextTinyPrefab;
		public DebugMenuItemText TextSmallPrefab;
		public DebugMenuItemText TextLongPrefab;
		public DebugMenuItemValue ValuePrefab;
		public DebugMenuItemChoices TwoChoicesPrefab;
		public DebugMenuItemChoices ThreeChoicesPrefab;
		public DebugMenuTab TabPrefab;
		public DebugMenuTabContents TabContentsPrefab;
		public RectTransform TabSpacerPrefab;
		public DebugMenuDebugTab DebugTabPrefab;
		public string DebugTabName = "Logs";

		[Header("Tabs")]
		public List<DebugMenuTabData> Tabs;
		public bool DisplayDebugTab = true;
		public int MaxTabs = 5;
		public int InitialActiveTabIndex = 0;
        
		[Header("Toggle")]
		public DebugMenu.ToggleDirections ToggleDirection = DebugMenu.ToggleDirections.RightToLeft;
		public float ToggleDuration = 0.2f;
		public FeelTween.TweenCurve ToggleCurve = FeelTween.TweenCurve.EaseInCubic;
        
		#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
			public Key ToggleKey = Key.Backquote;
		#else
		public KeyCode ToggleShortcut = KeyCode.Quote;
		#endif

		[Header("Style")]
		public Font RegularFont;
		public Font BoldFont;
		public Color BackgroundColor = Color.black;
		public Color AccentColor = FeelColors.ReunoYellow;
		public Color TextColor = Color.white;
	}
}

#endif
