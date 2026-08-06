using System;
using System.Collections.Generic;
using UnityEngine;
using Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement
{
	public class AssetManagerAssetBundle : AssetManager
	{
		private readonly IAssetBundleLoader _assetBundleLoader;

		private readonly string _assetBundleSuffix;

		private readonly AssetBundleManifest _assetBundleManifest;

		private readonly Dictionary<NormalizedPath, HashSet<NormalizedPath>> _assetBundleDependencies;

		private readonly Dictionary<NormalizedPath, HashSet<NormalizedPath>> _assetBundleDependents;

		private readonly List<AssetRequestAssetBundle> _assetRequests;

		private readonly List<SceneRequestAssetBundle> _sceneRequests;

		private readonly Dictionary<NormalizedPath, AssetBundleLoadRequest> _assetBundleLoadRequests;

		private readonly Dictionary<NormalizedPath, AssetBundleRequest> _assetBundleRequests;

		private readonly Dictionary<NormalizedPath, AssetBundle> _assetBundleCache;

		private readonly Dictionary<NormalizedPath, UnityEngine.Object> _assetCache;

		private readonly List<NormalizedPath> _assetBundleLoadRequestRemoveList;

		private readonly List<NormalizedPath> _assetBundleRequestRemoveList;

		public AssetManagerAssetBundle(IAssetBundleLoader assetBundleLoader, string manifestBundlePath, string assetBundleSuffix)
		{
			throw new NotImplementedException(NotImplementedMessage);
		}

		internal const string NotImplementedMessage =
			"Nhanh AssetBundle cua Thnguyet.AssetManagement CHUA DUOC CAI DAT (than ham bi boc mat trong ban decompile). "
			+ "Dung AssetManagerResources hoac AssetManagerEditorDatabase, hoac viet mot AssetManager moi dua tren Addressables. "
			+ "Xem muc 4 trong Assets/Thnguyet/README.md.";

		private void InitBundleDependencies()
		{
		}

		public override void UnloadUnusedAssets()
		{
		}

		private void GetBundleUsages(HashSet<NormalizedPath> directReferencedBundles, HashSet<NormalizedPath> indirectReferencedBundles, HashSet<NormalizedPath> unusedBundles)
		{
		}

		public string DumpBundleDependents()
		{
			return default;
		}

		public string DumpBundleUsages()
		{
			return default;
		}

		private void CleanNullReferencedCaches()
		{
		}

		internal override T LoadAsset<T>(string path)
		{
			return default;
		}

		internal override AssetRequest LoadAssetAsync<T>(string path)
		{
			return default;
		}

		private void FormatBundleAndAssetName(string path, out string formattedPath, out string bundleName, out string assetName)
		{
			formattedPath = default;
			bundleName = default;
			assetName = default;
		}

		public override void Update(float dt)
		{
		}

		private void UpdateBundleLoadRequest()
		{
		}

		private void UpdateBundleRequest()
		{
		}

		private void UpdateAssetRequest()
		{
		}

		private void UpdateSceneRequest()
		{
		}

		private AssetBundle LoadAssetBundle(string bundleName, bool loadDependencies)
		{
			return default;
		}

		private void LoadAssetBundleAsync(string bundleName, bool loadDependencies)
		{
		}

		private bool IsAssetBundleAndDependenciesReady(string bundleName)
		{
			return default(bool);
		}

		private bool HasNullReferencedDependencies(string bundleName)
		{
			return default(bool);
		}

		private static void ThrowIfInvalidType(Type type)
		{
		}
	}
}
