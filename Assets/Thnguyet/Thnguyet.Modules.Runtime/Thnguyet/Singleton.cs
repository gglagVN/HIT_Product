using UnityEngine;
using System;

namespace Thnguyet
{
    /// <summary>
    /// Singleton base class
    /// </summary>
    /// <remarks>
    /// Singleton thuần C# (không phải MonoBehaviour): instance tạo sẵn khi class được nạp, không bao giờ null.
    /// Không có vòng đời Unity — không Awake/Update, không tự reset khi đổi scene.
    /// </remarks>
    public class Singleton<T> where T : class, new()
    {
        private static readonly T singleton = new T();

        public static T instance
        {
            get
            {
                return singleton;
            }
        }

        public static T I
        {
            get { return instance; }
        }
    }

    /// <summary>
    /// Singleton for mono behavior object
    /// </summary>
    /// <remarks>
    /// CẢNH BÁO — đừng nhầm với <see cref="SceneSingleton{T}"/>: hành vi NGƯỢC nhau.
    /// <see cref="AutoSingleton{T}"/> TỰ TẠO GameObject "[@TênLớp]" khi scene chưa có, nên `instance` gần như không bao giờ null
    /// (trừ lúc thoát app) — tiện nhưng dễ đẻ object rác ngoài ý muốn và bỏ qua mọi ref đã gán trong Inspector.
    /// <see cref="SceneSingleton{T}"/> thì BẮT BUỘC đặt sẵn object trong scene, thiếu là trả null kèm log lỗi.
    /// Không tự DontDestroyOnLoad; đổi scene là mất instance (khác <see cref="SceneSingleton{T}"/>).
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T singleton;

        private static bool quitting;

        public static bool IsInstanceValid() { return singleton != null && !quitting; }

        /// Có sẵn instance chưa — dùng để kiểm tra mà KHÔNG kích hoạt việc tự tạo.
        public static bool HasInstance { get { return IsInstanceValid(); } }

        void Reset()
        {
            gameObject.name = typeof(T).Name;
        }

        /// Instance đang sống; CHƯA CÓ THÌ TỰ TẠO GameObject mới kèm component T. Trả null khi app đang thoát.
        public static T instance
        {
            get
            {
                // Đang thoát app thì trả null: chạm vào lúc này sẽ dựng lại một GameObject mới
                // ngay khi Unity đang huỷ scene, để lại object rác và log lỗi.
                if (quitting) return null;

                if (AutoSingleton<T>.singleton == null)
                {
                    AutoSingleton<T>.singleton = FindFirstObjectByType<T>();
                    if (AutoSingleton<T>.singleton == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "[@" + typeof(T).Name + "]";
                        AutoSingleton<T>.singleton = obj.AddComponent<T>();
                    }
                }

                return AutoSingleton<T>.singleton;
            }
        }

        public static T I
        {
            get { return instance; }
        }

        /// Tên theo chuẩn đặt tên C#; `instance` và `I` giữ lại cho code cũ.
        public static T Instance
        {
            get { return instance; }
        }

        /// Instance này có phải bản đang sống không — lớp con gọi sau base.Awake() để biết có nên
        /// chạy tiếp phần khởi tạo của mình hay không (bản sao đã bị đánh dấu huỷ thì dừng).
        protected bool IsSingletonInstance { get { return ReferenceEquals(singleton, this); } }

        /// Bản sao thứ hai tự huỷ để chỉ còn đúng một instance sống.
        protected virtual void Awake()
        {
            if (singleton != null && !ReferenceEquals(singleton, this))
            {
                Destroy(gameObject);
                return;
            }
            singleton = this as T;
            quitting = false;
        }

        protected virtual void OnApplicationQuit()
        {
            quitting = true;
        }
    }

    /// <summary>
    /// Tên cũ của <see cref="AutoSingleton{T}"/>, giữ lại để code cũ không vỡ.
    /// </summary>
    /// <remarks>
    /// Đừng dùng cho code mới: tên này gần giống tên cũ của <see cref="SceneSingleton{T}"/> nhưng hành vi ngược nhau.
    /// </remarks>
    [System.Obsolete("Doi ten thanh AutoSingleton<T> — no TU TAO GameObject khi thieu. Ban doi dat san trong scene la SceneSingleton<T>.")]
    public class SingletonMono<T> : AutoSingleton<T> where T : MonoBehaviour
    {
    }

    /// <summary>
    /// Singleton for mono behavior object
    /// </summary>
    /// <remarks>
    /// Bản "thủ công" của <see cref="AutoSingleton{T}"/>: KHÔNG tự tạo GameObject, KHÔNG tự huỷ bản sao.
    /// `instance` chỉ có giá trị sau khi Awake của object đặt sẵn trong scene chạy, và về null khi object bị huỷ.
    /// Dùng khi muốn instance sống đúng theo vòng đời scene — kiểm tra IsInstanceValid() trước khi gọi.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class ManualSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }

        public static bool IsInstanceValid() { return instance != null; }

        public static T I
        {
            get { return instance; }
        }

        void Reset()
        {
            gameObject.name = typeof(T).Name;
        }

        protected virtual void Awake()
        {
            if (instance == null)
                instance = (T)(MonoBehaviour)this;
        }

        protected void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }

    /// <summary>
    /// Singleton cho component UI kế thừa <see cref="UI.BaseUIComp"/>.
    /// </summary>
    /// <remarks>
    /// Giống <see cref="AutoSingleton{T}"/>: TỰ TẠO GameObject "[@TênLớp]" khi không tìm thấy trong scene —
    /// với UI thì object tự tạo nằm ngoài Canvas nên không hiển thị được, hầu như luôn là dấu hiệu quên đặt prefab.
    /// Không có cờ quitting nên chạm vào lúc thoát app vẫn dựng object rác.
    /// </remarks>
    public class SingletonUI<T> : UI.BaseUIComp where T : UI.BaseUIComp
    {
        private static T singleton;

        public static bool IsInstanceValid() { return singleton != null; }

        void Reset()
        {
            gameObject.name = typeof(T).Name;
        }

        public static T instance
        {
            get
            {
                if (SingletonUI<T>.singleton == null)
                {
                    SingletonUI<T>.singleton = (T)FindObjectOfType(typeof(T));
                    if (SingletonUI<T>.singleton == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = "[@" + typeof(T).Name + "]";
                        SingletonUI<T>.singleton = obj.AddComponent<T>();
                    }
                }

                return SingletonUI<T>.singleton;
            }
        }

        public static T I
        {
            get { return instance; }
        }
    }
}

