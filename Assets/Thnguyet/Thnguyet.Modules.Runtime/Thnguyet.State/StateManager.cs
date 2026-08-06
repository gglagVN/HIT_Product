using System;
using System.Collections.Generic;

namespace Thnguyet.State
{
	public class StateManager<T> : IDisposable where T : StateBase
	{
		private readonly Stack<T> _stack;

		private T _currentState;

		public T CurrentState
		{
			get
			{
				return _currentState;
			}
		}

		public void Dispose()
		{
			if (_currentState != null)
			{
				_currentState.Exit();
			}
		}

		public void Push(T state)
		{
			if (_currentState != null)
			{
				_currentState.Suspend();
			}
			_stack.Push(state);
			_currentState = state;
			_currentState.Enter();
		}

		public void Pop()
		{
			if (_currentState == null)
			{
				return;
			}
			_stack.Pop();
			_currentState.Exit();
			_currentState = ((_stack.Count > 0) ? _stack.Peek() : null);
			if (_currentState != null)
			{
				_currentState.Resume();
			}
		}

		public void TransitionTo(T state)
		{
			if (_stack.Count > 0)
			{
				_stack.Pop().Exit();
			}
			_stack.Push(state);
			_currentState = state;
			_currentState.Enter();
		}

		public void Update(float dt)
		{
			if (_currentState != null)
			{
				_currentState.Update(dt);
			}
		}

		public StateManager()
		{
			_stack = new Stack<T>(10);
		}
	}
}
