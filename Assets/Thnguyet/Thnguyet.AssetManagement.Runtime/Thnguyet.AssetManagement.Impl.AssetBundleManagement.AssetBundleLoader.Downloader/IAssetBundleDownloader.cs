using UnityEngine;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader.Downloader
{
	public interface IAssetBundleDownloader
	{
		bool IsBundleReady(string bundleName);

		AssetBundle Load(string bundleName);

		AssetBundleDownloadRequest RequestBundle(string bundleName);
	}
}
