using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.AssetManagement.Impl.ResourcesFolder
{
	/// Nap asset tu thu muc Resources theo dung duong dan ma game dung cho AssetBundle.
	/// Dung khi du an chua dung AssetBundle; thay bang AssetManagerAssetBundle khi da co bundle.
	public class AssetManagerResources : AssetManager
	{
		private readonly Dictionary<string, UnityEngine.Object> _cache =
			new Dictionary<string, UnityEngine.Object>(StringComparer.OrdinalIgnoreCase);

		internal override T LoadAsset<T>(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			if (_cache.TryGetValue(path, out var cached) && cached is T typedCache)
			{
				return typedCache;
			}
			T asset = UnityEngine.Resources.Load<T>(path);
			if (asset == null)
			{
				Debug.LogError("[AssetManagerResources] Khong tim thay asset '" + path + "' ("
					+ typeof(T).Name + ") trong Resources.");
				return null;
			}
			_cache[path] = asset;
			return asset;
		}

		internal override AssetRequest LoadAssetAsync<T>(string path)
		{
			AssetRequest request = new AssetRequest();
			request.Done(LoadAsset<T>(path));
			return request;
		}

		public override void UnloadUnusedAssets()
		{
			_cache.Clear();
			UnityEngine.Resources.UnloadUnusedAssets();
		}

		public override void Update(float dt)
		{
		}
	}
}
