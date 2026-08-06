using System;
using UnityEngine;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader.Downloader
{
	public class AssetBundleDownloadRequest : CustomYieldInstruction
	{
		private Action<AssetBundleDownloadRequest> _onCompleteCallback;

		private readonly string _bundleName;

		private string _bundlePath;

		private bool _isDone;

		private bool _isSuccess;

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

		public bool IsSuccess
		{
			get
			{
				return default(bool);
			}
		}

		public string BundleName
		{
			get
			{
				return default;
			}
		}

		public string BundlePath
		{
			get
			{
				return default;
			}
		}

		public event Action<AssetBundleDownloadRequest> onComplete
		{
			add
			{
			}
			remove
			{
			}
		}

		public AssetBundleDownloadRequest(string bundleName)
		{
			throw new NotImplementedException(AssetManagerAssetBundle.NotImplementedMessage);
		}

		internal void Done(bool isSuccess, string bundlePath)
		{
		}
	}
}
