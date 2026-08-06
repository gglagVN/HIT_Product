using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Thnguyet.UI
{
	public class UIManager
	{
		[CompilerGenerated]
		private Action<UIBase> onUIOpen;

		[CompilerGenerated]
		private Action<UIBase> onUIClose;

		[CompilerGenerated]
		private Action<UIBase> onUIOpenComplete;

		[CompilerGenerated]
		private Action<UIBase> onUICloseComplete;

		private readonly List<UIBase> _uiOpenList = new List<UIBase>(20);

		private readonly List<UIBase> _registeredInstances = new List<UIBase>(8);

		private readonly IUICreator _uiCreatorImpl;

		public UIManager(IUICreator uiCreatorImpl)
		{
			_uiCreatorImpl = uiCreatorImpl;
		}

		/// Dung khi moi man UI da dat san trong hierarchy va duoc dang ky qua Register.
		public UIManager()
		{
		}

		/// Dang ky mot man UI dat san trong hierarchy de Open/Find tim thay ma khong can tao moi.
		public void Register(UIBase ui)
		{
			if (ui == null || _registeredInstances.Contains(ui))
			{
				return;
			}
			_registeredInstances.Add(ui);
			ui.onCloseComplete += DeactivateOnCloseComplete;
		}

		/// Go mot man UI khoi danh sach dang ky, thuong goi khi canh chua no bi huy.
		public void Unregister(UIBase ui)
		{
			if (ui != null && _registeredInstances.Remove(ui))
			{
				ui.onCloseComplete -= DeactivateOnCloseComplete;
			}
		}

		public T Open<T>(params object[] args) where T : UIBase
		{
			T registered = FindRegistered<T>();
			if (registered != null)
			{
				registered.gameObject.SetActive(true);
				registered.Open(args);
				return registered;
			}
			if (_uiCreatorImpl == null)
			{
				UnityEngine.Debug.LogError("[UIManager] Chua dang ky instance " + typeof(T).Name
					+ " va cung khong co IUICreator de tao moi.");
				return null;
			}
			T val = GetUIInstance<T>(null);
			val.Open(args);
			return val;
		}

		public bool Exist<T>() where T : UIBase
		{
			return Find<T>() != null;
		}

		public bool TryFind<T>(out T ui) where T : UIBase
		{
			Type type = typeof(T);
			ui = (T)_uiOpenList.Find((UIBase u) => type.IsAssignableFrom(u.GetType()));
			return ui != null;
		}

		public T Find<T>() where T : UIBase
		{
			Type type = typeof(T);
			T open = (T)_uiOpenList.Find((UIBase u) => type.IsAssignableFrom(u.GetType()));
			return open != null ? open : FindRegistered<T>();
		}

		private T FindRegistered<T>() where T : UIBase
		{
			Type type = typeof(T);
			_registeredInstances.RemoveAll((UIBase u) => u == null);
			return (T)_registeredInstances.Find((UIBase u) => type.IsAssignableFrom(u.GetType()));
		}

		private void DeactivateOnCloseComplete(UIBase ui)
		{
			ui.gameObject.SetActive(false);
		}

		private T GetUIInstance<T>([Optional] string assetPath) where T : UIBase
		{
			T uIInstance = _uiCreatorImpl.GetUIInstance<T>(assetPath);
			uIInstance.onOpen += OnUIOpen;
			uIInstance.onClose += OnUIClose;
			uIInstance.onOpenComplete += OnUIOpenComplete;
			uIInstance.onCloseComplete += OnUICloseComplete;
			return uIInstance;
		}

		private void ReleaseUIInstance(UIBase ui)
		{
			ui.onOpen -= OnUIOpen;
			ui.onClose -= OnUIClose;
			ui.onOpenComplete -= OnUIOpenComplete;
			ui.onCloseComplete -= OnUICloseComplete;
			_uiCreatorImpl.ReleaseUIInstance(ui);
		}

		private void OnUIOpen(UIBase ui)
		{
			_uiOpenList.Add(ui);
			onUIOpen?.Invoke(ui);
		}

		private void OnUIClose(UIBase ui)
		{
			_uiOpenList.Remove(ui);
			onUIClose?.Invoke(ui);
		}

		private void OnUIOpenComplete(UIBase ui)
		{
			onUIOpenComplete?.Invoke(ui);
		}

		private void OnUICloseComplete(UIBase ui)
		{
			onUICloseComplete?.Invoke(ui);
			ReleaseUIInstance(ui);
		}
	}
}
