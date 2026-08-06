using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.Pool.Extension
{
	/// <summary>
	/// Pool prefab Unity: Instantiate khi thiếu, SetActive(false) khi trả về, và theo dõi danh sách instance đang bật.
	/// </summary>
	/// <remarks>
	/// MỖI POOL GẮN CHẶT MỘT PREFAB và một Transform cha, truyền vào lúc dựng và không đổi được —
	/// nhiều loại prefab thì phải tạo nhiều pool.
	/// Không tự Instantiate trước; instance đầu tiên sinh ra ở lần <see cref="Get"/> đầu, cân nhắc warm-up thủ công nếu sợ khựng khung hình.
	/// Không có maxSize hữu hạn (dùng mặc định của <see cref="ObjectPool{T}"/>) nên pool chỉ phình chứ không tự co lại.
	/// </remarks>
	public class ComponentPool<T> where T : Component
	{
		private readonly Transform _parent;

		private readonly T _prefab;

		private readonly ObjectPool<T> _pool;

		private readonly List<T> _activeElements;

		/// Danh sách instance đang bật. Đây là list GỐC, không phải bản sao — sửa nó là phá state của pool,
		/// và duyệt xuôi rồi Release trong vòng lặp sẽ nhảy phần tử; hãy duyệt ngược hoặc dùng <see cref="ReleaseAll"/>.
		public List<T> ActiveElements
		{
			get
			{
				return _activeElements;
			}
		}

		/// Buộc pool vào ĐÚNG MỘT prefab và một Transform cha; cả hai cố định trọn đời pool.
		/// capacity chỉ là dung lượng list ban đầu, không phải giới hạn số instance.
		public ComponentPool(Transform parent, T prefab, int capacity)
		{
			_parent = parent;
			_prefab = prefab;
			_pool = new ObjectPool<T>(Create, Destroy, null, true, capacity);
			_activeElements = new List<T>(capacity);
		}

		private T Create()
		{
			return UnityEngine.Object.Instantiate(_prefab, _parent, false);
		}

		private void Destroy(T element)
		{
			UnityEngine.Object.Destroy(element.gameObject);
		}

		/// Lấy một instance đã SetActive(true) và ghi vào <see cref="ActiveElements"/>.
		/// Transform/state của instance giữ nguyên từ lần dùng trước — tự reset vị trí, scale, animation ở chỗ gọi.
		public T Get()
		{
			T val = _pool.Get();
			val.gameObject.SetActive(true);
			_activeElements.Add(val);
			return val;
		}

		/// Tắt instance và trả về pool. NÉM InvalidOperationException nếu instance đã được trả trước đó —
		/// double-release là lỗi lập trình, hãy sửa chỗ gọi thay vì bắt exception.
		public void Release(T instance)
		{
			instance.gameObject.SetActive(false);
			_activeElements.Remove(instance);
			_pool.Release(instance);
		}

		/// Trả toàn bộ instance đang bật về pool, duyệt ngược nên an toàn; actionBeforeRelease (có thể null)
		/// chạy trên từng instance trước khi nó bị tắt — chỗ để dọn tween, huỷ đăng ký event.
		public void ReleaseAll(Action<T> actionBeforeRelease)
		{
			for (int num = _activeElements.Count - 1; num >= 0; num--)
			{
				T val = _activeElements[num];
				actionBeforeRelease?.Invoke(val);
				Release(val);
			}
		}
	}
}
