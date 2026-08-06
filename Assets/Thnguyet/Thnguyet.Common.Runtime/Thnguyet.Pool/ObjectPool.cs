using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Thnguyet.Pool
{
	/// <summary>
	/// Pool object thuần C# (không gắn Unity): tự gọi createFunc khi hết hàng, giữ lại object đã Release để tái dùng.
	/// </summary>
	/// <remarks>
	/// Muốn pool prefab/Component thì dùng <see cref="Extension.ComponentPool{T}"/> thay vì lớp này.
	/// KHÔNG thread-safe. Khi vượt maxSize thì object trả về bị destroyAction huỷ luôn thay vì giữ lại;
	/// không truyền destroyAction thì object bị bỏ rơi im lặng cho GC (với Unity Object là rò rỉ).
	/// Lưu ý maxSize so với <see cref="CountActive"/> — số đang MƯỢN RA, không phải số đang nằm chờ.
	/// </remarks>
	public sealed class ObjectPool<T> : IDisposable where T : class
	{
		private readonly bool _releaseCheck;

		private readonly List<T> _list;

		private readonly Func<T> _createFunc;

		private readonly Action<T> _releaseAction;

		private readonly Action<T> _destroyAction;

		private readonly int _maxSize;

		private int _countAll;

		/// Tổng số object pool đã tạo ra (đang dùng + đang nằm chờ).
		public int CountAll
		{
			get
			{
				return _countAll;
			}
		}

		/// Số object đang được mượn ra ngoài (chưa Release).
		public int CountActive
		{
			get
			{
				return _countAll - _list.Count;
			}
		}

		/// Số object đang nằm chờ trong pool, sẵn sàng cho lần Get kế tiếp.
		public int CountInactive
		{
			get
			{
				return _list.Count;
			}
		}

		/// Tạo pool. createFunc bắt buộc (null là ArgumentNullException); releaseAction chạy khi trả object về để dọn state;
		/// destroyAction chạy khi Clear/Dispose và khi vượt maxSize — nên luôn truyền nếu maxSize có thể chạm tới.
		/// releaseCheck = true thì Release kiểm tra trùng bằng cách quét list (O(n)); tắt đi để nhanh hơn nhưng mất lưới an toàn.
		public ObjectPool(Func<T> createFunc, [Optional] Action<T> destroyAction, [Optional] Action<T> releaseAction, bool releaseCheck = true, int capacity = 32, int maxSize = 10000)
		{
			if (createFunc == null)
			{
				throw new ArgumentNullException("createFunc");
			}
			_createFunc = createFunc;
			_releaseAction = releaseAction;
			_destroyAction = destroyAction;
			_releaseCheck = releaseCheck;
			_list = new List<T>(capacity);
			_maxSize = maxSize;
		}

		/// Mượn một object: lấy từ hàng chờ, hết thì gọi createFunc tạo mới. Object trả về giữ nguyên state lần dùng trước.
		public T Get()
		{
			int num = _list.Count - 1;
			if (num < 0)
			{
				T result = _createFunc();
				_countAll++;
				return result;
			}
			T item = _list[num];
			_list.RemoveAt(num);
			return item;
		}

		/// Mượn object kèm handle IDisposable để tự Release khi ra khỏi khối `using`.
		public PooledObject<T> Get(out T target)
		{
			target = Get();
			return new PooledObject<T>(target, this);
		}

		/// Trả object về pool. NÉM InvalidOperationException nếu object đã được trả trước đó (khi releaseCheck bật) —
		/// double-release là lỗi lập trình, đừng bọc try/catch mà hãy sửa chỗ gọi.
		public void Release(T element)
		{
			if (_releaseCheck && _list.Contains(element))
			{
				throw new InvalidOperationException("ObjectPool Trying to release an object that has already been released to the pool.");
			}
			_releaseAction?.Invoke(element);
			if (CountActive < _maxSize)
			{
				_list.Add(element);
				return;
			}
			_countAll--;
			// destroyAction là tham số tuỳ chọn lúc dựng pool; gọi thẳng sẽ ném NullReference
			// đúng lúc pool đầy — trường hợp chỉ xảy ra khi tải nặng nên rất khó gặp lúc test.
			_destroyAction?.Invoke(element);
		}

		/// Huỷ mọi object đang nằm chờ và reset bộ đếm. Object đang mượn ra ngoài KHÔNG bị đụng tới nhưng
		/// CountAll đã về 0, nên Release chúng sau đó sẽ làm CountActive âm.
		public void Clear()
		{
			if (_destroyAction != null)
			{
				foreach (T item in _list)
				{
					_destroyAction(item);
				}
			}
			_list.Clear();
			_countAll = 0;
		}

		/// Gọi thẳng <see cref="Clear"/>; pool vẫn dùng lại được sau Dispose.
		public void Dispose()
		{
			Clear();
		}
	}
}
