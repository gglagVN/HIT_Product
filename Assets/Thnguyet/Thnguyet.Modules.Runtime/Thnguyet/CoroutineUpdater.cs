using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet
{
	public class CoroutineUpdater : MonoBehaviour, IDisposable
	{
		private const string INACTIVE_NAME = "[Inactive]";

		[SerializeField]
		private CoroutineUpdater _instancePrefab;

		private static CoroutineUpdater _factory;

		private static Transform _parent;

		private static readonly Stack<CoroutineUpdater> _pool = new Stack<CoroutineUpdater>(10);

		private readonly List<Action> _actions = new List<Action>();

		private readonly List<Action> _actionsToExecute = new List<Action>();

		private bool _isActionsEmpty = true;

		private static Transform Parent => _parent;

		/// Dang ky object "[CoroutineUpdater]" dat san trong scene lam goc va lam nguon prefab.
		private void Awake()
		{
			if (_factory != null || _instancePrefab == null)
			{
				return;
			}
			_factory = this;
			_parent = transform;
			if (transform.parent != null)
			{
				Debug.LogWarning("[CoroutineUpdater] \"" + name + "\" khong phai root GameObject nen chi song trong scene hien tai."
					+ " Muon giu qua cac scene thi dua no ra ngoai cung hierarchy.");
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		public static CoroutineUpdater Get(string name)
		{
			// Pool la static nen song qua scene, con instance trong do thi khong: bo cac instance da bi huy.
			while (_pool.Count > 0)
			{
				CoroutineUpdater pooled = _pool.Pop();
				if (pooled == null)
				{
					continue;
				}
				pooled.name = name;
				return pooled;
			}
			if (_factory == null || _factory._instancePrefab == null)
			{
				Debug.LogError("[CoroutineUpdater] Khong tim thay object \"[CoroutineUpdater]\" co gan _instancePrefab trong scene khoi dong.");
				return null;
			}
			CoroutineUpdater coroutineUpdater2 = UnityEngine.Object.Instantiate(_factory._instancePrefab, Parent);
			coroutineUpdater2.name = name;
			return coroutineUpdater2;
		}

		private static void Release(CoroutineUpdater coroutineUpdater)
		{
			// Object da bi huy cung scene thi khong con gi de tra ve pool.
			if (coroutineUpdater == null)
			{
				return;
			}
			coroutineUpdater.StopAllCoroutines();
			if (Parent != null && coroutineUpdater.transform.parent != Parent)
			{
				coroutineUpdater.transform.SetParent(Parent);
			}
			coroutineUpdater.gameObject.name = INACTIVE_NAME;
			_pool.Push(coroutineUpdater);
		}

		/// Xoa trang thai static khi factory bi huy, de scene sau dang ky lai tu dau.
		private void OnDestroy()
		{
			if (_factory != this)
			{
				return;
			}
			_factory = null;
			_parent = null;
			_pool.Clear();
		}

		public void InvokeOnUnityMainThread(Action action)
		{
			lock (_actions)
			{
				_actions.Add(action);
				_isActionsEmpty = false;
			}
		}

		private void Update()
		{
			if (_isActionsEmpty)
			{
				return;
			}
			lock (_actions)
			{
				_actionsToExecute.AddRange(_actions);
				_actions.Clear();
				_isActionsEmpty = true;
			}
			foreach (Action item in _actionsToExecute)
			{
				try
				{
					item();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			_actionsToExecute.Clear();
		}

		public void Dispose()
		{
			Release(this);
		}

		public CoroutineUpdater()
		{
		}
	}
}
