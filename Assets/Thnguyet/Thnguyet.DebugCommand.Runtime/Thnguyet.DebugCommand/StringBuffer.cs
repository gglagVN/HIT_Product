using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Thnguyet.DebugCommand
{
	internal sealed class StringBuffer
	{
		private readonly Queue<string> _lines;

		private readonly StringBuilder _stringBuilder;

		public int MaxLines
		{
			[CompilerGenerated]
			get
			{
				return default(int);
			}
			[CompilerGenerated]
			set
			{
			}
		}

		public StringBuffer(int maxLines = 100)
		{
		}

		public void AppendLine(params string[] values)
		{
		}

		public string GetContent()
		{
			return default;
		}

		public void Clear()
		{
		}

		public override string ToString()
		{
			return default;
		}
	}
}
