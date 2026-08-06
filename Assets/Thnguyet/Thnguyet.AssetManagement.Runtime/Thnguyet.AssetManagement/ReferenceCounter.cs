using System;
using System.Collections;
using System.Collections.Generic;

namespace Thnguyet.AssetManagement
{
	internal class ReferenceCounter : IEnumerable<KeyValuePair<NormalizedPath, int>>, IEnumerable
	{
		private readonly Dictionary<NormalizedPath, int> _dictionary;

		public ReferenceCounter(int capacity)
		{
			_dictionary = new Dictionary<NormalizedPath, int>(capacity);
		}

		public void Increase(NormalizedPath path)
		{
			if (_dictionary.TryGetValue(path, out var count))
			{
				count++;
				_dictionary[path] = count;
			}
			else
			{
				_dictionary[path] = 1;
			}
		}

		public void Decrease(NormalizedPath path)
		{
			if (!_dictionary.TryGetValue(path, out var count) || count <= 0)
			{
				throw new Exception(string.Format("reference count already non-positive of {0}", path));
			}
			count--;
			if (count == 0)
			{
				_dictionary.Remove(path);
			}
			else
			{
				_dictionary[path] = count;
			}
		}

		public IEnumerator<KeyValuePair<NormalizedPath, int>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
