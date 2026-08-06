using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Thnguyet.ScriptableConfig
{
	public abstract class ConfigTableSO<TKey, TValue> : ConfigSO, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		[SerializeField]
		private SerializedDictionary<TKey, TValue> _configs;

		public int Count
		{
			get
			{
				if (_configs == null)
				{
					return 0;
				}
				return _configs.Count;
			}
		}

		/// Them cap key-value vao bang, dong bo ca danh sach duoc serialize.
		public void Add(TKey key, TValue value)
		{
			if (_configs == null)
			{
				_configs = new SerializedDictionary<TKey, TValue>();
			}
			_configs.AddToList(key, value);
		}

		/// Xoa toan bo bang, ke ca danh sach duoc serialize.
		public void Clear()
		{
			if (_configs == null)
			{
				return;
			}
			_configs.ClearList();
		}

		public bool Contains(TKey key)
		{
			if (_configs == null || key == null)
			{
				return false;
			}
			return _configs.ContainsKey(key);
		}

		/// Lay config theo key, bao loi kem ten asset neu khong tim thay.
		public TValue Get(TKey key)
		{
			if (TryGet(key, out TValue value))
			{
				return value;
			}
			Debug.LogError($"Config key({key}) not found in {name} ({GetType().Name})", this);
			return default;
		}

		public bool TryGet(TKey key, out TValue value)
		{
			if (_configs == null || key == null)
			{
				value = default;
				return false;
			}
			return _configs.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (_configs == null)
			{
				return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
			}
			return _configs.GetEnumerator();
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected ConfigTableSO()
		{
		}
	}
}
