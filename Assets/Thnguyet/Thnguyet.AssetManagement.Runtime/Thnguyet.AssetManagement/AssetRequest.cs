using System;
using UnityEngine;

namespace Thnguyet.AssetManagement
{
	public class AssetRequest : CustomYieldInstruction, IDisposable
	{
		private bool _isDone;

		private UnityEngine.Object _asset;

		private Action<AssetRequest> _onCompleteCallback;

		public override bool keepWaiting
		{
			get
			{
				return !_isDone;
			}
		}

		public bool IsDone
		{
			get
			{
				return _isDone;
			}
		}

		public UnityEngine.Object Asset
		{
			get
			{
				return _asset;
			}
		}

		public event Action<AssetRequest> onCompleted
		{
			add
			{
				if (_isDone)
				{
					value(this);
				}
				else
				{
					_onCompleteCallback = (Action<AssetRequest>)Delegate.Combine(_onCompleteCallback, value);
				}
			}
			remove
			{
				_onCompleteCallback = (Action<AssetRequest>)Delegate.Remove(_onCompleteCallback, value);
			}
		}

		internal void Done(UnityEngine.Object asset)
		{
			if (!_isDone)
			{
				_isDone = true;
				_asset = asset;
				_onCompleteCallback?.Invoke(this);
				_onCompleteCallback = null;
			}
		}

		public void Dispose()
		{
			_onCompleteCallback = null;
		}

		public AssetRequest()
		{
		}
	}
}
