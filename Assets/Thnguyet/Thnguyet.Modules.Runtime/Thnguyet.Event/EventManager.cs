using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.Event
{
	/// <summary>
	/// Event bus gõ theo kiểu: mỗi struct là một loại sự kiện, khoá là tên đầy đủ của kiểu đó.
	/// </summary>
	/// <remarks>
	/// KHÔNG phải singleton và KHÔNG static — game tự tạo và tự giữ instance, mỗi instance là một bus riêng.
	/// Chỉ nhận struct (ràng buộc `where T : struct`) để tránh cấp phát khi raise.
	/// Listener ném exception sẽ bị bắt và log, không làm gãy các listener còn lại.
	/// </remarks>
	public class EventManager
	{
		private const int EVENT_CAPACITY = 100;

		private const int LISTENER_CAPACITY = 10;

		private readonly Dictionary<string, List<Delegate>> _listeners;

		public EventManager()
		{
			_listeners = new Dictionary<string, List<Delegate>>(100);
		}

		/// Đăng ký nhận sự kiện kiểu T. Ném ArgumentNullException nếu action null.
		/// KHÔNG chống trùng — đăng ký hai lần thì listener chạy hai lần; luôn có RemoveListener đối xứng khi huỷ object.
		public void AddListener<T>(Action<T> action) where T : struct
		{
			if (action == null)
			{
				throw new ArgumentNullException();
			}
			string eventKey = GetEventKey<T>();
			if (_listeners.TryGetValue(eventKey, out var value))
			{
				value.Add(action);
				return;
			}
			value = new List<Delegate>(10);
			value.Add(action);
			_listeners.Add(eventKey, value);
		}

		/// Huỷ đăng ký; an toàn khi gọi ngay trong lúc Raise đang chạy (chỉ đánh dấu null, dọn sau khi phát xong).
		/// Đăng ký trùng nhiều lần thì mỗi lần gọi chỉ gỡ được một bản.
		public void RemoveListener<T>(Action<T> action) where T : struct
		{
			if (_listeners.TryGetValue(GetEventKey<T>(), out var value))
			{
				int num = value.IndexOf(action);
				if (num >= 0)
				{
					value[num] = null;
				}
			}
		}

		/// Phát sự kiện tới mọi listener, gọi NGƯỢC thứ tự đăng ký (mới nhất chạy trước).
		/// Listener đăng ký thêm ngay trong lúc Raise sẽ không nhận được lần phát này.
		public void Raise<T>(T evt) where T : struct
		{
			if (!_listeners.TryGetValue(GetEventKey<T>(), out var value))
			{
				return;
			}
			for (int num = value.Count - 1; num >= 0; num--)
			{
				Delegate @delegate = value[num];
				if (@delegate == null)
				{
					continue;
				}
				try
				{
					((Action<T>)@delegate)(evt);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			value.RemoveAll((Delegate e) => e == null);
		}

		private static string GetEventKey<T>()
		{
			return typeof(T).FullName;
		}
	}
}
