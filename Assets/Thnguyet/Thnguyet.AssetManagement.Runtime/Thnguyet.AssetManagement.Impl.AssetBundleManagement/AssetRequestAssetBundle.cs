using System;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement
{
	internal class AssetRequestAssetBundle : AssetRequest
	{
		private readonly string _path;

		private readonly string _bundleName;

		private readonly string _assetName;

		private readonly Type _assetType;

		public string Path
		{
			get
			{
				return default;
			}
		}

		public string BundleName
		{
			get
			{
				return default;
			}
		}

		public string AssetName
		{
			get
			{
				return default;
			}
		}

		public Type AssetType
		{
			get
			{
				return default;
			}
		}

		public AssetRequestAssetBundle(string path, string bundleName, string assetName, Type assetType)
		{
			throw new NotImplementedException(AssetManagerAssetBundle.NotImplementedMessage);
		}
	}
}
