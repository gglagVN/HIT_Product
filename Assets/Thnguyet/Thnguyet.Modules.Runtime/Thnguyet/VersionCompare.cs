
namespace Thnguyet
{
	public static class VersionCompare
	{
		private const char VERSION_SPLIT_CHAR = '.';

		/// So sanh hai chuoi phien ban dang "1.2.10"; tra ve -1, 0 hoac 1.
		/// Thieu doan thi coi nhu 0 nen "1.2" bang "1.2.0".
		public static int Compare(string ver1, string ver2)
		{
			string[] parts1 = (ver1 ?? string.Empty).Split(VERSION_SPLIT_CHAR);
			string[] parts2 = (ver2 ?? string.Empty).Split(VERSION_SPLIT_CHAR);
			int count = (parts1.Length > parts2.Length) ? parts1.Length : parts2.Length;
			for (int i = 0; i < count; i++)
			{
				int value1 = (i < parts1.Length) ? ParseStringToInt(parts1[i]) : 0;
				int value2 = (i < parts2.Length) ? ParseStringToInt(parts2[i]) : 0;
				if (value1 != value2)
				{
					return (value1 < value2) ? (-1) : 1;
				}
			}
			return 0;
		}

		/// Doc so o dau chuoi va bo phan duoi dang chu, vi du "3-beta" ra 3, "beta" ra 0.
		private static int ParseStringToInt(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			int value = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c < '0' || c > '9')
				{
					break;
				}
				value = value * 10 + (c - '0');
			}
			return value;
		}
	}
}
