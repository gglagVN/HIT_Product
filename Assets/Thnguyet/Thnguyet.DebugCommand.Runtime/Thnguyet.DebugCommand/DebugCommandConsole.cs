using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Thnguyet.DebugCommand
{
	public sealed class DebugCommandConsole : IDisposable
	{
		private class DebugCommandInfo
		{
			public DebugCommandAttribute DebugCommandAttribute
			{
				[CompilerGenerated]
				get
				{
					return default;
				}
			}

			public MethodInfo MethodInfo
			{
				[CompilerGenerated]
				get
				{
					return default;
				}
			}

			public DebugCommandInfo(DebugCommandAttribute debugCommandAttribute, MethodInfo methodInfo)
			{
			}
		}

		private const string TEXT_COLOR_SUCCESS = "#00FF00";

		private const string TEXT_COLOR_FAILURE = "#FF0000";

		private const string BUILD_IN_COMMAND_HELP = "help";

		private const string BUILD_IN_COMMAND_CLEAR = "clear";

		private Dictionary<string, DebugCommandInfo> _commands;

		private ConsoleGUI _consoleGUI;

		private readonly StringBuffer _stringBuffer;

		public bool IsShowGUI
		{
			get
			{
				return default(bool);
			}
		}

		public void Dispose()
		{
		}

		public void ShowGUI()
		{
		}

		public void HideGUI()
		{
		}

		public void AppendMessage(params string[] values)
		{
		}

		private void CommandCollect()
		{
		}

		private string HandleInputValidate(string input)
		{
			return default;
		}

		private void HandleInputSubmit(string input)
		{
		}

		private void AppendOutputLine(params string[] values)
		{
		}

		private void SetGUIOutput(StringBuffer stringBuffer)
		{
		}

		private bool TryExecuteBuildInCommand(string command, out string result)
		{
			result = default;
			return default(bool);
		}

		private bool TryExecuteCommand(string command, out string result)
		{
			result = default;
			return default(bool);
		}

		private DebugCommandInfo[] GetCommandsStartWith(string str, out string longestCommonStr)
		{
			longestCommonStr = default;
			return default;
		}

		private static string GetLongestCommonString(IList<string> strings, int startIndex)
		{
			return default;
		}

		private static IEnumerable<string> GetSubstringsFromStart(string str, int startIndex)
		{
			return default;
		}

		private string GetCommandsDisplayString(IEnumerable<DebugCommandInfo> commands)
		{
			return default;
		}

		private static ConsoleGUI CreateGUIInstance()
		{
			return default;
		}

		private static bool TrySplitCommandNameAndParameters(string command, out string name, out string[] parameters)
		{
			name = default;
			parameters = default;
			return default(bool);
		}

		private static string GetCommandDisplayString(DebugCommandInfo commandInfo)
		{
			return default;
		}

		public DebugCommandConsole()
		{
			throw new System.NotImplementedException(NotImplementedMessage);
		}

		internal const string NotImplementedMessage =
			"Thnguyet.DebugCommand CHUA DUOC CAI DAT (toan bo module bi boc than ham trong ban decompile). "
			+ "Dung mot console co san cua project, vi du Assets/Plugins/IngameDebugConsole. "
			+ "Xem muc 4 trong Assets/Thnguyet/README.md.";
	}
}
