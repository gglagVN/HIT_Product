using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet
{
	[Serializable]
	public sealed class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[Serializable]
		private struct KeyValuePair
		{
			public TKey key;

			public TValue value;
		}

		[SerializeField]
		private List<KeyValuePair> _keyValuePairs;

		public SerializedDictionary()
			: this(0)
		{
		}

		public SerializedDictionary(int capacity)
			: base(capacity)
		{
			_keyValuePairs = new List<KeyValuePair>(capacity);
		}

		/// Them cap key-value vao ca dictionary lan danh sach duoc serialize.
		public void AddToList(TKey key, TValue value)
		{
			if (TryAdd(key, value))
			{
				_keyValuePairs.Add(new KeyValuePair
				{
					key = key,
					value = value
				});
			}
		}

		public void ClearList()
		{
			Clear();
			_keyValuePairs.Clear();
		}

		/// Dung lai dictionary tu danh sach da serialize, bo qua va bao loi neu trung key.
		void UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Clear();
			foreach (KeyValuePair keyValuePair in _keyValuePairs)
			{
				if (ContainsKey(keyValuePair.key))
				{
					UnityEngine.Debug.LogError(string.Format("Duplicate key({0}) in {1} on deserialize", keyValuePair.key, typeof(SerializedDictionary<TKey, TValue>)));
					continue;
				}
				base[keyValuePair.key] = keyValuePair.value;
			}
		}

		/// Dồn dictionary ngược về danh sách trước khi serialize — nếu không, mọi cặp thêm lúc chạy
		/// bằng Add() thông thường (không qua AddToList) sẽ mất khi Unity lưu.
		void UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if (_keyValuePairs == null)
			{
				_keyValuePairs = new List<KeyValuePair>(Count);
			}

			// Ngoai play mode, DANH SACH moi la ban goc: nguoi dung dang go truc tiep vao Inspector,
			// va dong "+" vua bam luon trung key nen chua co mat trong dictionary.
			if (!Application.isPlaying)
			{
				return;
			}

			_keyValuePairs.Clear();

			foreach (System.Collections.Generic.KeyValuePair<TKey, TValue> pair in this)
			{
				_keyValuePairs.Add(new KeyValuePair
				{
					key = pair.Key,
					value = pair.Value
				});
			}
		}
	}
}
