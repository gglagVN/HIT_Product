using System;

namespace Thnguyet.Util
{
	public static class ObfuscatorXor
	{
		/// XOR tung byte cua data voi key lap vong, ghi de tai cho.
		public static void XOR(byte[] data, byte[] key)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (key == null || key.Length == 0)
			{
				throw new ArgumentNullException("key");
			}
			for (int i = 0; i < data.Length; i++)
			{
				data[i] ^= key[i % key.Length];
			}
		}

		/// Xao text bang key, tra ve chuoi Base64 an toan de ghi xuong PlayerPrefs/JSON/file.
		public static string Obfuscate(string text, string key)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
			XOR(data, System.Text.Encoding.UTF8.GetBytes(RequireKey(key)));
			return Convert.ToBase64String(data);
		}

		/// Nguoc cua Obfuscate; nem FormatException neu chuoi vao khong phai Base64.
		public static string Deobfuscate(string obfuscated, string key)
		{
			if (obfuscated == null)
			{
				throw new ArgumentNullException("obfuscated");
			}
			byte[] data = Convert.FromBase64String(obfuscated);
			XOR(data, System.Text.Encoding.UTF8.GetBytes(RequireKey(key)));
			return System.Text.Encoding.UTF8.GetString(data);
		}

		private static string RequireKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException("Key rong — khong xao duoc gi.", "key");
			}
			return key;
		}
	}
}
