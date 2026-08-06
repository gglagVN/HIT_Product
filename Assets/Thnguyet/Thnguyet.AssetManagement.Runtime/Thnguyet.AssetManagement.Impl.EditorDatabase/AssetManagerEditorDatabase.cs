#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Thnguyet.AssetManagement.Impl.EditorDatabase
{
	/// Nap asset thang tu AssetDatabase theo dung duong dan ma game dung cho AssetBundle.
	/// Dung trong Editor khi chua build bundle; thay bang AssetManagerAssetBundle khi da co bundle.
	public class AssetManagerEditorDatabase : AssetManager
	{
		public const string DEFAULT_ROOT = "Assets/_inassetbundle";

		private static readonly string[] IGNORED_EXTENSIONS = { ".meta", ".cs", ".asmdef" };

		private static readonly string[] EXTENSION_PRIORITY =
		{
			".prefab", ".asset", ".mat", ".mixer", ".controller", ".anim", ".ogg", ".wav", ".mp3"
		};

		private readonly string _root;

		private readonly Dictionary<string, string> _assetPathByLoadPath =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private readonly Dictionary<string, UnityEngine.Object> _cache =
			new Dictionary<string, UnityEngine.Object>(StringComparer.OrdinalIgnoreCase);

		public AssetManagerEditorDatabase()
			: this(DEFAULT_ROOT)
		{
		}

		public AssetManagerEditorDatabase(string root)
		{
			_root = root.TrimEnd('/');
			BuildIndex();
		}

		/// Lap chi muc "duong dan game" -> "duong dan asset trong project", bo phan mo rong.
		private void BuildIndex()
		{
			string absoluteRoot = Path.Combine(Directory.GetCurrentDirectory(), _root);
			if (!Directory.Exists(absoluteRoot))
			{
				Debug.LogError("[AssetManagerEditorDatabase] Khong thay thu muc goc asset: " + _root);
				return;
			}
			foreach (string file in Directory.GetFiles(absoluteRoot, "*", SearchOption.AllDirectories))
			{
				string extension = Path.GetExtension(file);
				if (Array.IndexOf(IGNORED_EXTENSIONS, extension.ToLowerInvariant()) >= 0)
				{
					continue;
				}
				string assetPath = _root + "/" + file.Substring(absoluteRoot.Length + 1).Replace('\\', '/');
				string loadPath = assetPath.Substring(_root.Length + 1);
				loadPath = loadPath.Substring(0, loadPath.Length - extension.Length);
				if (!_assetPathByLoadPath.TryGetValue(loadPath, out var existing))
				{
					_assetPathByLoadPath.Add(loadPath, assetPath);
					continue;
				}
				int rankNew = Rank(extension);
				int rankOld = Rank(Path.GetExtension(existing));
				if (rankNew < rankOld)
				{
					_assetPathByLoadPath[loadPath] = assetPath;
				}
				else if (rankNew == rankOld)
				{
					Debug.LogWarning("[AssetManagerEditorDatabase] Trung duong dan '" + loadPath
						+ "': giu '" + existing + "', bo qua '" + assetPath + "'");
				}
			}
		}

		/// Thu tu uu tien khi nhieu file cung mot duong dan nap (vi du Sprite .asset truoc .png).
		private static int Rank(string extension)
		{
			int index = Array.IndexOf(EXTENSION_PRIORITY, extension.ToLowerInvariant());
			return index < 0 ? EXTENSION_PRIORITY.Length : index;
		}

		internal override T LoadAsset<T>(string path)
		{
			if (_cache.TryGetValue(path, out var cached) && cached is T typedCache)
			{
				return typedCache;
			}
			if (!_assetPathByLoadPath.TryGetValue(path, out var assetPath))
			{
				Debug.LogError("[AssetManagerEditorDatabase] Khong tim thay asset '" + path + "' trong " + _root);
				return null;
			}
			T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
			if (asset == null)
			{
				foreach (UnityEngine.Object sub in AssetDatabase.LoadAllAssetsAtPath(assetPath))
				{
					asset = sub as T;
					if (asset != null)
					{
						break;
					}
				}
			}
			if (asset == null)
			{
				Debug.LogError("[AssetManagerEditorDatabase] Asset '" + assetPath + "' khong phai kieu "
					+ typeof(T).Name);
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
			Resources.UnloadUnusedAssets();
		}

		public override void Update(float dt)
		{
		}
	}
}
#endif
