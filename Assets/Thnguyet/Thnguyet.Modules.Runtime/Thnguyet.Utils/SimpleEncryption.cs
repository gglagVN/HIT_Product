using System;

namespace Thnguyet.Utils
{
	public static class SimpleEncryption
	{
		public static string Encrypt(string text, string key)
		{
			return XOR(text, key);
		}

		public static string Decrypt(string text, string key)
		{
			return XOR(text, key);
		}

		/// XOR tung ky tu cua text voi key lap vong.
		private static string XOR(string text, string key)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			char[] result = new char[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				result[i] = (char)(text[i] ^ key[i % key.Length]);
			}
			return new string(result);
		}
	}
}
