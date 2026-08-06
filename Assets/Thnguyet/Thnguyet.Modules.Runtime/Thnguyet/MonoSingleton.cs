using UnityEngine;

namespace Thnguyet
{
	/// <summary>
	/// Singleton MonoBehaviour BẮT BUỘC đặt sẵn trong scene.
	/// </summary>
	/// <remarks>
	/// CẢNH BÁO — đừng nhầm với <see cref="AutoSingleton{T}"/>: hai tên gần giống nhau nhưng hành vi NGƯỢC nhau.
	/// Ở đây KHÔNG có chuyện tự tạo GameObject: thiếu object trong scene thì <see cref="Instance"/> trả null và log lỗi một lần.
	/// Object được DontDestroyOnLoad nên sống xuyên scene; bản sao thứ hai bị Destroy ngay trong Awake.
	/// Khởi tạo của lớp con viết trong <see cref="OnAwake"/>, không override Awake (Awake ở đây là private).
	/// </remarks>
	public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
	{
		private static T _instance;

		private static bool _missingWarned;

		public static bool HasInstance => _instance != null;

		/// Instance đặt sẵn trong scene; TRẢ NULL kèm log lỗi nếu chưa có object nào — không tự tạo.
		public static T Instance
		{
			get
			{
				if (_instance == null && !_missingWarned)
				{
					_missingWarned = true;
					UnityEngine.Debug.LogError("[SceneSingleton] Khong tim thay " + typeof(T).Name
						+ " trong scene. Hay dat san GameObject \"[" + typeof(T).Name + "]\" kem component nay vao scene.");
				}
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			_instance = (T)this;
			_missingWarned = false;
			Object.DontDestroyOnLoad(base.gameObject);
			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		public SceneSingleton()
		{
		}
	}

	/// <summary>
	/// Tên cũ của <see cref="SceneSingleton{T}"/>, giữ lại để code cũ không vỡ.
	/// </summary>
	/// <remarks>
	/// Đừng dùng cho code mới: tên này gần giống <see cref="AutoSingleton{T}"/> nhưng hành vi ngược nhau.
	/// </remarks>
	[System.Obsolete("Doi ten thanh SceneSingleton<T> — no DOI object dat san trong scene. Ban tu tao GameObject la AutoSingleton<T>.")]
	public class MonoSingleton<T> : SceneSingleton<T> where T : MonoSingleton<T>
	{
	}
}
