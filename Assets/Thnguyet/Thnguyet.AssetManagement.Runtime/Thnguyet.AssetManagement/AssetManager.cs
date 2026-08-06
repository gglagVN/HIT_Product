using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Thnguyet.AssetManagement
{
	public abstract class AssetManager
	{
		private readonly ReferenceCounter _assetReferenceCounter = new ReferenceCounter(50);

		public AssetLoader CreateAssetLoader()
		{
			return new AssetLoader(this);
		}

		internal void IncreaseAssetReferenceCount(NormalizedPath path)
		{
			lock (_assetReferenceCounter)
			{
				_assetReferenceCounter.Increase(path);
			}
		}

		internal void DecreaseAssetReferenceCount(NormalizedPath path)
		{
			lock (_assetReferenceCounter)
			{
				_assetReferenceCounter.Decrease(path);
			}
		}

		internal void ForeachAssetReferences(Action<NormalizedPath, int> action)
		{
			lock (_assetReferenceCounter)
			{
				foreach (KeyValuePair<NormalizedPath, int> item in _assetReferenceCounter)
				{
					item.Deconstruct(out var path, out var count);
					action?.Invoke(path, count);
				}
			}
		}

		public string DumpAssetReferences()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Referenced assets:");
			ForeachAssetReferences(delegate(NormalizedPath path, int count)
			{
				sb.AppendLine(string.Format("{0} {1}", path, count));
			});
			return sb.ToString();
		}

		public abstract void UnloadUnusedAssets();

		internal abstract T LoadAsset<T>(string path) where T : UnityEngine.Object;

		internal abstract AssetRequest LoadAssetAsync<T>(string path) where T : UnityEngine.Object;

		public abstract void Update(float dt);

		protected AssetManager()
		{
		}
	}
}
