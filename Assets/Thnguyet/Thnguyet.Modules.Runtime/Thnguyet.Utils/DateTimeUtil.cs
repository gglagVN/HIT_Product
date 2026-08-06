using System;

namespace Thnguyet.Utils
{
	public static class DateTimeUtil
	{
		private static readonly DateTime UtcOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		public static long UtcNowTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - UtcOrigin).TotalSeconds);
		}

		public static long DateTimeToTimeStamp(DateTime dateTime)
		{
			return Convert.ToInt64((dateTime - UtcOrigin).TotalSeconds);
		}

		public static DateTime TimeStampToDateTimeUtc(long timeStamp)
		{
			return UtcOrigin.AddSeconds(timeStamp);
		}
	}
}
