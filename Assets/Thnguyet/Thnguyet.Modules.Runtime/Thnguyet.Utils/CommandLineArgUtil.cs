using System;

namespace Thnguyet.Utils
{
	public static class CommandLineArgUtil
	{
		/// Lay gia tri dung ngay sau tham so ten name, vi du "-env staging" tra ve "staging".
		/// Khong tim thay hoac khong con doi so nao phia sau thi tra ve null.
		public static string GetArgValue(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 0; i < args.Length; i++)
			{
				if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
				{
					return (i + 1 < args.Length) ? args[i + 1] : null;
				}
			}
			return null;
		}

		/// Kiem tra dong lenh co chua tham so ten name khong.
		public static bool ContainArg(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 0; i < args.Length; i++)
			{
				if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
	}
}
