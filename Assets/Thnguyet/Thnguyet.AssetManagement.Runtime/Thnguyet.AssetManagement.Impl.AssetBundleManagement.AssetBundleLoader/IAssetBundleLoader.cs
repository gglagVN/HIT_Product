using UnityEngine;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader
{
	public interface IAssetBundleLoader
	{
		AssetBundle Load(string bundleName);

		AssetBundleLoadRequest LoadAsync(string bundleName);
	}
}
