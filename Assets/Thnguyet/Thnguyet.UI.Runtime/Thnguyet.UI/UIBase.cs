using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.UI
{
	[RequireComponent(typeof(GraphicRaycaster))]
	[RequireComponent(typeof(Canvas))]
	/// <summary>
	/// Lớp cha của mọi màn hình do <see cref="UIManager"/> quản lý (hệ UI thứ hai của framework).
	/// </summary>
	/// <remarks>
	/// Dùng hệ này khi cần vòng đời rõ ràng và mở theo KIỂU thay vì theo chuỗi: <c>uiManager.Open&lt;MyScreen&gt;(args)</c>,
	/// có state máy (<see cref="UIState"/>), event onOpen/onClose/onOpenComplete/onCloseComplete và hook animation mở/đóng ngắt được giữa chừng.
	/// Mỗi màn hình tự mang Canvas + GraphicRaycaster riêng nên tự quyết sorting order — KHÔNG có khái niệm tầng chung như hệ kia.
	/// Đối trọng là hệ thứ nhất <see cref="CanvasManager"/> + <see cref="BaseUIMenu"/> — chọn một hệ cho mỗi màn hình, đừng trộn.
	/// Lớp con bắt buộc cài <see cref="OnInit"/>, <see cref="OnOpen"/>, <see cref="OnClose"/>.
	/// </remarks>
	public abstract class UIBase : MonoBehaviour
	{
		[SerializeField]
		protected Canvas _canvas;

		[SerializeField]
		protected GraphicRaycaster _graphicRaycaster;

		private UIState _state;

		private bool _init;

		private object[] _args;

		public Camera Camera
		{
			get
			{
				return _canvas.rootCanvas.worldCamera;
			}
		}

		public Canvas Canvas
		{
			get
			{
				return _canvas;
			}
		}

		public GraphicRaycaster GraphicRaycaster
		{
			get
			{
				return _graphicRaycaster;
			}
		}

		public UIState State
		{
			get
			{
				return _state;
			}
		}

		public IReadOnlyList<object> Args
		{
			get
			{
				return _args;
			}
		}

		public event Action<UIBase> onOpen;

		public event Action<UIBase> onClose;

		public event Action<UIBase> onOpenComplete;

		public event Action<UIBase> onCloseComplete;

		private void Init()
		{
			_init = true;
			OnInit();
		}

		public void Open(params object[] args)
		{
			if (_state < UIState.Closing)
			{
				return;
			}
			if (!_init)
			{
				Init();
			}
			if (_state == UIState.Closing)
			{
				AbortClose();
			}
			_args = args;
			_state = UIState.Opening;
			OnOpen();
			InvokeCallback(onOpen);
			if (_state == UIState.Opening)
			{
				OnOpenAnimation(delegate
				{
					_state = UIState.Opened;
					OpenComplete();
				});
			}
		}

		public void Close()
		{
			if (_state >= UIState.Closing)
			{
				return;
			}
			if (_state == UIState.Opening)
			{
				AbortOpen();
			}
			_state = UIState.Closing;
			OnClose();
			InvokeCallback(onClose);
			if (_state == UIState.Closing)
			{
				OnCloseAnimation(delegate
				{
					_state = UIState.Closed;
					CloseComplete();
				});
			}
		}

		private void AbortOpen()
		{
			OpenComplete();
			OnOpenAbort();
		}

		private void AbortClose()
		{
			CloseComplete();
			OnCloseAbort();
		}

		private void InvokeCallback(Action<UIBase> callback)
		{
			callback?.Invoke(this);
		}

		protected bool TryGetParam<T>(int index, out T param)
		{
			param = default(T);
			if (index < 0 || index >= _args.Length)
			{
				return false;
			}
			if (!(_args[index] is T))
			{
				return false;
			}
			param = (T)_args[index];
			return true;
		}

		protected T GetParam<T>(int index)
		{
			if (index < 0 || index >= _args.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (_args[index] != null && !(_args[index] is T))
			{
				throw new InvalidCastException();
			}
			return (T)_args[index];
		}

		private void OpenComplete()
		{
			OnOpenComplete();
			InvokeCallback(onOpenComplete);
		}

		private void CloseComplete()
		{
			OnCloseComplete();
			InvokeCallback(onCloseComplete);
		}

		protected abstract void OnInit();

		protected abstract void OnOpen();

		protected abstract void OnClose();

		protected virtual void OnOpenComplete()
		{
		}

		protected virtual void OnCloseComplete()
		{
		}

		protected virtual void OnOpenAbort()
		{
		}

		protected virtual void OnCloseAbort()
		{
		}

		protected virtual void OnOpenAnimation(Action callback)
		{
			callback?.Invoke();
		}

		protected virtual void OnCloseAnimation(Action callback)
		{
			callback?.Invoke();
		}

		protected UIBase()
		{
			_state = UIState.Closed;
		}
	}
}
