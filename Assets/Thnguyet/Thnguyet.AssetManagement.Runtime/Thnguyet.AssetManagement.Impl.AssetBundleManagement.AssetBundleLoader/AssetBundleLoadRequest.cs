using System;
using UnityEngine;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader
{
	public class AssetBundleLoadRequest : CustomYieldInstruction
	{
		internal AssetBundleCreateRequest assetBundleCreateRequest;

		private bool _isDone;

		private AssetBundle _assetBundle;

		private Action<AssetBundleLoadRequest> _onCompleteCallback;

		public override bool keepWaiting
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsDone
		{
			get
			{
				return default(bool);
			}
		}

		public AssetBundle AssetBundle
		{
			get
			{
				return default;
			}
		}

		internal void Done(AssetBundle assetBundle)
		{
		}

		private void InvokeCompletionEvent()
		{
		}

		public AssetBundleLoadRequest()
		{
			throw new NotImplementedException(AssetManagerAssetBundle.NotImplementedMessage);
		}
	}
}
