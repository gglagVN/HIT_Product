using UnityEngine.SceneManagement;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement
{
	internal class SceneRequestAssetBundle : SceneRequest
	{
		private readonly string _bundleName;

		private readonly string _sceneName;

		private readonly LoadSceneMode _loadSceneMode;

		public string SceneName
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

		public LoadSceneMode LoadSceneMode
		{
			get
			{
				return default(LoadSceneMode);
			}
		}

		/// Constructor cua lop nay bi xoa han trong ban decompile nen 3 field readonly khong co duong gan.
		public SceneRequestAssetBundle()
		{
			throw new System.NotImplementedException(AssetManagerAssetBundle.NotImplementedMessage);
		}
	}
}
