using System;
using System.Runtime.CompilerServices;

namespace Thnguyet.DebugCommand
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class DebugCommandAttribute : Attribute
	{
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return default;
			}
		}

		public string Desc
		{
			[CompilerGenerated]
			get
			{
				return default;
			}
		}

		public DebugCommandAttribute(string name)
		{
		}

		public DebugCommandAttribute(string name, string desc)
		{
		}
	}
}
