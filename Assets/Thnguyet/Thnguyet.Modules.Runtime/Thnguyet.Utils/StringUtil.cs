using System.Text;
using UnityEngine;

namespace Thnguyet.Utils
{
	public static class StringUtil
	{
		/// Chuoi dau cham chay vong theo thoi gian, dung cho hieu ung dang tai.
		public static string CycleDots(int maxDots = 3)
		{
			return new string('.', (int)(Time.realtimeSinceStartup % (float)maxDots));
		}

		public static string FormatTimeColon_Empty()
		{
			return "--:--:--";
		}

		public static string FormatTimeColon(int totalSeconds)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", totalSeconds / 3600, totalSeconds / 60 % 60, totalSeconds % 60);
		}

		/// Rut gon thoi gian ve mot don vi lon nhat: gio, phut hoac giay.
		public static string FormatTimeShort(int totalSeconds)
		{
			int minutes = totalSeconds / 60 % 60;
			if (totalSeconds >= 3600)
			{
				return string.Format("{0}h {1:00}m", totalSeconds / 3600, minutes);
			}
			if (minutes >= 1)
			{
				return string.Format("{0}m {1:00}s", minutes, totalSeconds % 60);
			}
			return string.Format("{0}s", totalSeconds % 60);
		}

		public static string FormatTimeColonShort(int totalSeconds)
		{
			int minutes = totalSeconds / 60 % 60;
			if (totalSeconds >= 3600)
			{
				return string.Format("{0:00}:{1:00}", totalSeconds / 3600, minutes);
			}
			return string.Format("{0:00}:{1:00}", minutes, totalSeconds % 60);
		}

		public static string FormatNumberComma(int num)
		{
			return string.Format("{0:N0}", num);
		}

		/// Chen dau cach lam dau phan cach hang nghin.
		public static string FormatNumberSpace(int num)
		{
			string text = num.ToString();
			StringBuilder stringBuilder = new StringBuilder(text);
			for (int i = text.Length - 3; i >= 0; i -= 3)
			{
				stringBuilder.Insert(i, ' ');
			}
			return stringBuilder.ToString();
		}
	}
}
