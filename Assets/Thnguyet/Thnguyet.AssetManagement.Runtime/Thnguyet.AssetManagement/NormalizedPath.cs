using System;

namespace Thnguyet.AssetManagement
{
	public readonly struct NormalizedPath : IEquatable<NormalizedPath>
	{
		private readonly string _path;

		public NormalizedPath(string path)
		{
			_path = path;
		}

		public override string ToString()
		{
			return _path;
		}

		public bool Equals(NormalizedPath other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (obj is NormalizedPath other)
			{
				return this == other;
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (_path == null)
			{
				return 0;
			}
			int hash = 17;
			for (int i = 0; i < _path.Length; i++)
			{
				char c = _path[i];
				if (c == '\\')
				{
					c = '/';
				}
				hash = hash * 23 + c.GetHashCode();
			}
			return hash;
		}

		public static bool operator ==(NormalizedPath a, NormalizedPath b)
		{
			if (a._path == null || b._path == null)
			{
				return a._path == b._path;
			}
			return ComparePaths(a._path, b._path);
		}

		public static bool operator !=(NormalizedPath a, NormalizedPath b)
		{
			return !(a == b);
		}

		private static bool ComparePaths(string path1, string path2)
		{
			if (path1.Length != path2.Length)
			{
				return false;
			}
			for (int i = 0; i < path1.Length; i++)
			{
				char c1 = ((path1[i] == '\\') ? '/' : path1[i]);
				char c2 = ((path2[i] == '\\') ? '/' : path2[i]);
				if (c1 != c2)
				{
					return false;
				}
			}
			return true;
		}

		public static implicit operator string(NormalizedPath normalizedPath)
		{
			return normalizedPath._path;
		}

		public static implicit operator NormalizedPath(string path)
		{
			return new NormalizedPath(path);
		}
	}
}
