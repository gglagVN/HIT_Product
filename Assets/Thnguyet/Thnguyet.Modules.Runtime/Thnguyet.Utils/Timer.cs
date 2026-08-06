using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.Utils
{
	public class Timer
	{
		private readonly struct Event
		{
			public readonly float time;

			public readonly Action callback;

			public Event(float time, Action callback)
			{
				this.time = time;
				this.callback = callback;
			}
		}

		private readonly float _target;

		private readonly List<Event> _events;

		private float _time;

		public float Target
		{
			get
			{
				return _target;
			}
		}

		public float Time
		{
			get
			{
				return _time;
			}
		}

		public bool IsComplete
		{
			get
			{
				return _time >= _target;
			}
		}

		public Timer(float target)
		{
			_target = target;
			_events = new List<Event>();
		}

		public void Seek(float time)
		{
			_time = time;
		}

		public void AddEvent(float time, Action callback)
		{
			_events.Add(new Event(time, callback));
		}

		public void Update(float dt)
		{
			float num = _time;
			if (num >= _target)
			{
				return;
			}
			_time = Mathf.Clamp(num + dt, 0f, _target);
			foreach (Event @event in _events)
			{
				if (@event.time > num && @event.time <= _time)
				{
					@event.callback?.Invoke();
				}
			}
		}
	}
}
