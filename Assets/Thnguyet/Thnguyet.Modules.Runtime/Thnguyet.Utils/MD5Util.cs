using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Thnguyet.Utils
{
	public static class MD5Util
	{
		/// Tra ve chuoi MD5 dang hex chu thuong cua mot file; file khong ton tai thi tra ve null.
		public static string GetHashFromFile(string filePath, int bufferSize = 1048576)
		{
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				return null;
			}
			using (MD5 md5 = MD5.Create())
			{
				using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
				{
					byte[] hash = md5.ComputeHash(stream);
					StringBuilder builder = new StringBuilder(hash.Length * 2);
					for (int i = 0; i < hash.Length; i++)
					{
						builder.Append(hash[i].ToString("x2"));
					}
					return builder.ToString();
				}
			}
		}

		/// Kiem tra file co dung ma bam mong doi khong, khong phan biet chu hoa chu thuong.
		public static bool IsFileHash(string filePath, string hash)
		{
			if (string.IsNullOrEmpty(hash))
			{
				return false;
			}
			string fileHash = GetHashFromFile(filePath);
			return fileHash != null && string.Equals(fileHash, hash, StringComparison.OrdinalIgnoreCase);
		}
	}
}
