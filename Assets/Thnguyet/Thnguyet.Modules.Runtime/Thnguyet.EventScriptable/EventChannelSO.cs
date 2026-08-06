using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.EventScriptable
{
	[CreateAssetMenu(menuName = "EventChannelSO/Void EventChannel")]
	public class EventChannelSO : ScriptableObject
	{
		private readonly HashSet<Action> _listeners;

		public void AddListener(Action action)
		{
		}

		public void RemoveListener(Action action)
		{
		}

		public void Raise()
		{
		}

		public EventChannelSO()
		{
		}
	}
	public abstract class EventChannelSO<T> : ScriptableObject
	{
		private readonly HashSet<Action<T>> _listeners;

		public void AddListener(Action<T> action)
		{
		}

		public void RemoveListener(Action<T> action)
		{
		}

		public void Raise(T param)
		{
		}

		protected EventChannelSO()
		{
		}
	}
}
