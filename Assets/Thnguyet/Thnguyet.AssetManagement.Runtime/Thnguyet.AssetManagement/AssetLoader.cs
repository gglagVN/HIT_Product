using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.AssetManagement
{
	public class AssetLoader : IDisposable
	{
		private readonly AssetManager _assetManager;

		private readonly HashSet<NormalizedPath> _refAssetPaths;

		private bool _isDisposed;

		internal AssetLoader(AssetManager assetManager)
		{
			_assetManager = assetManager;
			_refAssetPaths = new HashSet<NormalizedPath>(10);
		}

		~AssetLoader()
		{
			Dispose(disposing: false);
		}

		public GameObject Instantiate(string path, Transform parent, bool worldPositionStays)
		{
			return Instantiate(path, new InstantiationParameters(parent, worldPositionStays));
		}

		private GameObject Instantiate(string path, InstantiationParameters instantiationParameters)
		{
			GameObject gameObject = LoadAsset<GameObject>(path);
			if (gameObject == null)
			{
				return null;
			}
			return instantiationParameters.Instantiate(gameObject);
		}

		public T LoadAsset<T>(string path) where T : UnityEngine.Object
		{
			ThrowIfDisposed();
			AddAssetRef(path);
			return _assetManager.LoadAsset<T>(path);
		}

		public AssetRequest LoadAssetAsync<T>(string path) where T : UnityEngine.Object
		{
			ThrowIfDisposed();
			AddAssetRef(path);
			return _assetManager.LoadAssetAsync<T>(path);
		}

		public void ReleaseAsset(string path)
		{
			ThrowIfDisposed();
			RemoveAssetRef(path);
		}

		public void ReleaseAllAssets()
		{
			ThrowIfDisposed();
			foreach (NormalizedPath refAssetPath in _refAssetPaths)
			{
				_assetManager.DecreaseAssetReferenceCount(refAssetPath);
			}
			_refAssetPaths.Clear();
		}

		private void AddAssetRef(string path)
		{
			if (_refAssetPaths.Add(path))
			{
				_assetManager.IncreaseAssetReferenceCount(path);
			}
		}

		private void RemoveAssetRef(string path)
		{
			if (_refAssetPaths.Remove(path))
			{
				_assetManager.DecreaseAssetReferenceCount(path);
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				foreach (NormalizedPath refAssetPath in _refAssetPaths)
				{
					_assetManager.DecreaseAssetReferenceCount(refAssetPath);
				}
			}
		}

		private void ThrowIfDisposed()
		{
			if (_isDisposed)
			{
				throw new Exception("AssetLoader already disposed");
			}
		}
	}
}
