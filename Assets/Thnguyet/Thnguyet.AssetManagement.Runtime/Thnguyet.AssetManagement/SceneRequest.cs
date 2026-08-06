using UnityEngine;

namespace Thnguyet.AssetManagement
{
	public class SceneRequest : CustomYieldInstruction
	{
		private bool _isDone;

		private bool _isCancelled;

		private bool _allowSceneActivation;

		private AsyncOperation _loadSceneOperation;

		public override bool keepWaiting
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsDone
		{
			get
			{
				return default(bool);
			}
		}

		public bool IsCancelled
		{
			get
			{
				return default(bool);
			}
		}

		internal AsyncOperation LoadSceneOperation
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		internal void Done()
		{
		}

		public SceneRequest()
		{
			throw new System.NotImplementedException(
				"Thnguyet.AssetManagement CHUA CO duong nap scene: SceneRequest bi boc than ham, va AssetManager "
				+ "cung khong he co LoadSceneAsync. Dung UnityEngine.SceneManagement truc tiep. "
				+ "Xem muc 4 trong Assets/Thnguyet/README.md.");
		}
	}
}
