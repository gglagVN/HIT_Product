using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.DebugCommand
{
	public sealed class ConsoleGUIQuickInputButton : MonoBehaviour
	{
		[SerializeField]
		private Button _button;

		[SerializeField]
		private Text _text;

		private string _input;

		public string Input
		{
			get
			{
				return default;
			}
		}

		public event Action<ConsoleGUIQuickInputButton> onClick
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

		private void OnButtonClick()
		{
		}

		public void Init(string input)
		{
		}

		public ConsoleGUIQuickInputButton()
		{
		}
	}
}
