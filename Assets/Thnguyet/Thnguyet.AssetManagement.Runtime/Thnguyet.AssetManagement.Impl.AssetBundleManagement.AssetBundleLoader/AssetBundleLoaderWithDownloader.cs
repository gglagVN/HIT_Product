using System.Collections.Generic;
using UnityEngine;
using Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader.Downloader;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader
{
	public class AssetBundleLoaderWithDownloader : IAssetBundleLoader
	{
		private readonly IAssetBundleDownloader _downloader;

		private readonly Dictionary<string, AssetBundleLoadRequest> _assetBundleLoadRequests;

		public AssetBundleLoaderWithDownloader(IAssetBundleDownloader downloader)
		{
			throw new System.NotImplementedException(AssetManagerAssetBundle.NotImplementedMessage);
		}

		public AssetBundle Load(string bundleName)
		{
			return default;
		}

		public AssetBundleLoadRequest LoadAsync(string bundleName)
		{
			return default;
		}
	}
}
