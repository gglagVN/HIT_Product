using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Thnguyet.AssetManagement.Impl.AssetBundleManagement.AssetBundleLoader.Downloader
{
	public class AssetBundleDownloaderDefault : IAssetBundleDownloader
	{
		public enum BundleStatus
		{
			Local,
			NotExist,
			DownloadedHashMissing,
			DownloadedHashNotMatch,
			DownloadedHashMatch
		}

		public class BundleDownloadRequest : CustomYieldInstruction
		{
			public enum RequestResult
			{
				InProgress,
				Success,
				NetworkError,
				FileIOError
			}

			private readonly string _bundleName;

			private readonly string _downloadPath;

			private RequestResult _result;

			private string _error;

			private float _progress;

			private string _bundleHash;

			public override bool keepWaiting
			{
				get
				{
					return default(bool);
				}
			}

			public string BundleName
			{
				get
				{
					return default;
				}
			}

			public string DownloadPath
			{
				get
				{
					return default;
				}
			}

			public RequestResult Result
			{
				get
				{
					return default(RequestResult);
				}
			}

			public string Error
			{
				get
				{
					return default;
				}
				internal set
				{
				}
			}

			public float Progress
			{
				get
				{
					return default(float);
				}
				internal set
				{
				}
			}

			public string BundleHash
			{
				get
				{
					return default;
				}
			}

			public BundleDownloadRequest(string bundleName, string downloadPath)
			{
			}

			internal void Done(RequestResult result, string bundleHash)
			{
			}
		}

		public class BundleHashRequest : CustomYieldInstruction
		{
			public enum RequestResult
			{
				InProgress,
				Success,
				NetworkError
			}

			private readonly string _bundleName;

			private RequestResult _result;

			private string _error;

			private string _bundleHash;

			public override bool keepWaiting
			{
				get
				{
					return default(bool);
				}
			}

			public string BundleName
			{
				get
				{
					return default;
				}
			}

			public RequestResult Result
			{
				get
				{
					return default(RequestResult);
				}
			}

			public string Error
			{
				get
				{
					return default;
				}
				internal set
				{
				}
			}

			public string BundleHash
			{
				get
				{
					return default;
				}
			}

			public BundleHashRequest(string bundleName)
			{
			}

			internal void Done(RequestResult result, string bundleHash)
			{
			}
		}

		public class BundleRequest : CustomYieldInstruction
		{
			public enum RequestResult
			{
				InProgress,
				Success,
				NetworkErrorHashRequest,
				NetworkErrorBundleDownload,
				FileIOError
			}

			private Action<BundleRequest> _onCompleteCallback;

			private readonly string _bundleName;

			private string _bundlePath;

			private float _progress;

			private string _error;

			private RequestResult _result;

			public override bool keepWaiting
			{
				get
				{
					return default(bool);
				}
			}

			public string BundleName
			{
				get
				{
					return default;
				}
			}

			public string BundlePath
			{
				get
				{
					return default;
				}
			}

			internal float Progress
			{
				set
				{
				}
			}

			public string Error
			{
				get
				{
					return default;
				}
				internal set
				{
				}
			}

			public RequestResult Result
			{
				get
				{
					return default(RequestResult);
				}
			}

			public event Action<BundleRequest> onComplete
			{
				add
				{
				}
				remove
				{
				}
			}

			public BundleRequest(string bundleName)
			{
			}

			internal void Done(RequestResult result, string bundlePath)
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass23_0
		{
			public AssetBundleDownloaderDefault _003C_003E4__this;

			public BundleRequest request;

			public string bundleName;

			public _003C_003Ec__DisplayClass23_0()
			{
			}
		}









		private readonly string _streamingAssetsPath;

		private readonly HashSet<string> _streamingAssetsFilePaths;

		private readonly string _localDirectory;

		private readonly string _downloadDirectory;

		private static readonly string BundleHashKeyFormatter;

		private string _downloadUrl;

		private Dictionary<string, BundleRequest> _bundleRequests;

		private MonoBehaviour _coroutineRunner;

		public AssetBundleDownloaderDefault(string localDirectory, string downloadDirectory, IEnumerable<string> streamingAssetsFilePaths, string downloadUrl, MonoBehaviour coroutineRunner)
		{
			throw new System.NotImplementedException(AssetManagerAssetBundle.NotImplementedMessage);
		}

		public AssetBundle Load(string bundleName)
		{
			return default;
		}

		public AssetBundleDownloadRequest RequestBundle(string bundleName)
		{
			return default;
		}

		private bool IsFileExist(string path)
		{
			return default(bool);
		}

		private bool IsInStreamingAssets(string path)
		{
			return default(bool);
		}

		private static string FormatPath(string path)
		{
			return default;
		}

		public void Initialize(string downloadUrl, MonoBehaviour coroutineRunner)
		{
		}

		public bool IsBundleReady(string bundleName)
		{
			return default(bool);
		}

		public BundleStatus GetBundleStatus(string bundleName)
		{
			return default(BundleStatus);
		}

		private BundleRequest RequestBundle_Internal(string bundleName)
		{
			return default;
		}

		private IEnumerator TrackBundleRequest(BundleRequest request)
		{
			return default;
		}

		private IEnumerator RequestBundleCoroutine(BundleRequest request)
		{
			return default;
		}

		private BundleHashRequest RequestBundleHash(string bundleName)
		{
			return default;
		}

		private IEnumerator RequestBundleHashCoroutine(BundleHashRequest request)
		{
			return default;
		}

		private BundleDownloadRequest RequestBundleDownload(string bundleName, string downloadPath)
		{
			return default;
		}

		private IEnumerator RequestBundleDownloadCoroutine(BundleDownloadRequest request)
		{
			return default;
		}

		private bool IsInLocalDirectory(string bundleName, out string fullPath)
		{
			fullPath = default;
			return default(bool);
		}

		private bool IsInDownloadDirectory(string bundleName, out string fullPath)
		{
			fullPath = default;
			return default(bool);
		}

		private bool TryGetBundleCachedHash(string bundleName, out string hash)
		{
			hash = default;
			return default(bool);
		}

		private void SetBundleCachedHash(string bundleName, string hash)
		{
		}
	}
}
