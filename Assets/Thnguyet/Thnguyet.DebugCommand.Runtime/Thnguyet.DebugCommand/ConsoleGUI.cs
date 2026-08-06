using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Thnguyet.Pool.Extension;

namespace Thnguyet.DebugCommand
{
	public sealed class ConsoleGUI : MonoBehaviour
	{
		private const int INPUT_CACHE_MAX_COUNT = 10;

		[SerializeField]
		private CanvasScaler _canvasScaler;

		[SerializeField]
		private ScrollRect _outputScrollRect;

		[SerializeField]
		private ScrollRect _quickInputButtonScrollRect;

		[SerializeField]
		private InputField _outputField;

		[SerializeField]
		private InputField _inputField;

		[SerializeField]
		private Button _buttonSubmit;

		[SerializeField]
		private ConsoleGUIQuickInputButton _quickInputButtonPrefab;

		private ComponentPool<ConsoleGUIQuickInputButton> _quickInputButtonPool;

		private readonly List<string> _inputCache;

		private int _inputCacheIndex;

		public event Func<string, string> onInputValidate
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public event Action<string> onInputSubmit
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void Awake()
		{
			UnityEngine.Debug.LogError(DebugCommandConsole.NotImplementedMessage, this);
		}

		private void UpdateCanvasScale()
		{
		}

		private void OnSubmitClick()
		{
		}

		private void OnValueChanged(string input)
		{
		}

		private void SubmitInput(string input)
		{
		}

		public void SetOutput(string text)
		{
		}

		public void SetInput(string input)
		{
		}

		public void FocusOnInput()
		{
		}

		public void ClearInputButtons()
		{
		}

		public void DisplayInputButtons(IReadOnlyList<string> inputs)
		{
		}

		private void OnInputButtonClick(ConsoleGUIQuickInputButton quickInputButton)
		{
		}

		private void Update()
		{
		}

		public ConsoleGUI()
		{
		}
	}
}
