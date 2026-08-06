using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.UI
{
    /// <summary>
    /// Tầng hiển thị của một <see cref="BaseUIMenu"/>; giá trị càng lớn càng nằm trên.
    /// </summary>
    /// <remarks>
    /// Mỗi tầng là một GameObject con do <see cref="CanvasManager"/> tạo lúc Awake, theo đúng thứ tự khai báo ở đây —
    /// thêm/đổi thứ tự phần tử là đổi cả thứ tự render, đừng chèn giữa.
    /// Đẩy một menu tầng Menu sẽ tự đóng sạch tầng Popup.
    /// </remarks>
    public enum eUILayer
    {
        Background = 0,
        Menu,
        Popup,
        AlwaysOnTop
    }

    /// <summary>
    /// Bộ quản lý UI dạng stack theo tầng: nạp prefab <see cref="BaseUIMenu"/> từ Resources, cache lại và đẩy/đóng theo định danh.
    /// </summary>
    /// <remarks>
    /// Đây là MỘT trong HAI hệ UI của framework — hệ này (CanvasManager + <see cref="BaseUIMenu"/>) dùng cho menu/popup toàn màn hình.
    /// Hệ còn lại là <see cref="UIManager"/> + <see cref="UIBase"/>. Đừng trộn hai hệ trong cùng một màn hình.
    /// Gắn component này lên GameObject có Canvas, đặt Script Execution Order sớm, rồi gọi <see cref="Init"/> một lần lúc boot.
    /// API đều là static: gọi được từ mọi nơi nhưng cũng nghĩa là state sống xuyên suốt, phải Init lại khi reload.
    /// CẢNH BÁO: nhóm hàm banner quảng cáo (<see cref="SetAdsBannerSize(bool,int)"/>, <see cref="SetAdsBannerSizeByRatio"/>,
    /// <see cref="SetBannerBackgroundColor"/>, <see cref="SetBannerBackgroundSprite"/>) đọc `_AdsRectTrans` —
    /// field này KHÔNG BAO GIỜ được gán (dòng khởi tạo trong Awake đang bị comment), nên gọi vào là NullReferenceException.
    /// </remarks>
    public class CanvasManager : Thnguyet.AutoSingleton<CanvasManager>
    {
        static Canvas _UICanvas;

        public static Canvas UICanvas
        {
            get { return _UICanvas; }
        }

        public static float ScreenScale { get; protected set; }

        static RectTransform _UIRectTrans;

        public static RectTransform UIRectTrans
        {
            get { return _UIRectTrans; }
        }

        static RectTransform _AdsRectTrans;

        public static RectTransform AdsRectTrans
        {
            get { return _AdsRectTrans; }
        }

        static string DefaultDataPath;
        static Dictionary<string, Stack<BaseUIMenu>> UICached = new Dictionary<string, Stack<BaseUIMenu>>();
        static List<List<BaseUIMenu>> OpenedUIStack = new List<List<BaseUIMenu>>();

#if UNITY_EDITOR
        static bool sFinishAwake = false;
#endif
        protected override void Awake()
        {
            base.Awake();
            if (!IsSingletonInstance) return;

            UICached.Clear();
            OpenedUIStack.Clear();
            _UICanvas = this.GetComponent<Canvas>();
            _UIRectTrans = new GameObject("UI", typeof(RectTransform)).GetComponent<RectTransform>();
            _UIRectTrans.SetParent(this.transform);
            SetFullScreenRect(_UIRectTrans);
            // _AdsRectTrans = Instantiate(_UIRectTrans, this.transform);
            // _AdsRectTrans.name = "Ads";

            var layers = System.Enum.GetNames(typeof(eUILayer));
            for (int i = 0; i < layers.Length; ++i)
            {
                var newLayer = new GameObject(layers[i], typeof(RectTransform));
                newLayer.transform.SetParent(_UIRectTrans.transform);
                SetFullScreenRect(newLayer.GetComponent<RectTransform>());
                OpenedUIStack.Add(new List<BaseUIMenu>());
            }

            // ScreenScale = UICanvas.pixelRect.size.y / UICanvas.scaleFactor / 1080;

#if UNITY_EDITOR
            sFinishAwake = true;
#endif
        }

        static void EnsureLayerStacksInitialized()
        {
            int layerCount = System.Enum.GetNames(typeof(eUILayer)).Length;
            while (OpenedUIStack.Count < layerCount)
            {
                OpenedUIStack.Add(new List<BaseUIMenu>());
            }
        }

        static bool CachePoppedMenu(BaseUIMenu menu, bool destroy)
        {
            if (menu == null)
            {
                return false;
            }

            if (destroy)
            {
                Destroy(menu.gameObject);
            }
            else
            {
                menu.gameObject.SetActive(false);
                if (!string.IsNullOrEmpty(menu.UIIdentifier))
                {
                    if (!UICached.ContainsKey(menu.UIIdentifier))
                    {
                        UICached[menu.UIIdentifier] = new Stack<BaseUIMenu>();
                    }

                    if (!UICached[menu.UIIdentifier].Contains(menu))
                    {
                        UICached[menu.UIIdentifier].Push(menu);
                    }
                }
            }

            if (EventOnMenuPopped != null)
            {
                EventOnMenuPopped(menu);
            }

            return true;
        }

        void SetFullScreenRect(RectTransform target)
        {
            target.transform.localScale = Vector3.one;
            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.one;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
        }

        /// CHƯA DÙNG ĐƯỢC: `_AdsRectTrans` không bao giờ được gán nên gọi hàm này ném NullReferenceException.
        public static void SetAdsBannerSizeByRatio(bool top, float ratioByWidth)
        {
            SetAdsBannerSize(top, Mathf.CeilToInt(_AdsRectTrans.rect.width * ratioByWidth));
        }

        /// CHƯA DÙNG ĐƯỢC: `_AdsRectTrans` không bao giờ được gán nên gọi hàm này ném NullReferenceException.
        public static void SetAdsBannerSize(bool top, int height)
        {
            _UIRectTrans.offsetMin = new Vector2(_UIRectTrans.offsetMin.x, top ? 0 : height);
            _UIRectTrans.offsetMax = new Vector2(_UIRectTrans.offsetMin.x, top ? -height : 0);
            _AdsRectTrans.offsetMin = new Vector2(_AdsRectTrans.offsetMin.x, top ? _UIRectTrans.rect.height : 0);
            _AdsRectTrans.offsetMax = new Vector2(_AdsRectTrans.offsetMin.x, top ? 0 : -_UIRectTrans.rect.height);
        }

        /// CHƯA DÙNG ĐƯỢC: `_AdsRectTrans` không bao giờ được gán nên gọi hàm này ném NullReferenceException.
        public static void SetAdsBannerSize(bool top, int height, eUILayer layer)
        {
            var layerTrans = _UIRectTrans.GetChild((int) layer).GetComponent<RectTransform>();
            layerTrans.offsetMin = new Vector2(_UIRectTrans.offsetMin.x, top ? 0 : height);
            layerTrans.offsetMax = new Vector2(_UIRectTrans.offsetMin.x, top ? -height : 0);
            _AdsRectTrans.offsetMin = new Vector2(_AdsRectTrans.offsetMin.x, top ? _UIRectTrans.rect.height : 0);
            _AdsRectTrans.offsetMax = new Vector2(_AdsRectTrans.offsetMin.x, top ? 0 : -_UIRectTrans.rect.height);
        }

        /// CHƯA DÙNG ĐƯỢC: `_AdsRectTrans` không bao giờ được gán nên gọi hàm này ném NullReferenceException.
        public static void SetBannerBackgroundColor(Color input)
        {
            Image img = _AdsRectTrans.GetComponent<Image>();
            if (img == null)
            {
                img = _AdsRectTrans.gameObject.AddComponent<Image>();
            }

            img.color = input;
        }

        /// CHƯA DÙNG ĐƯỢC: `_AdsRectTrans` không bao giờ được gán nên gọi hàm này ném NullReferenceException.
        public static void SetBannerBackgroundSprite(Sprite input)
        {
            Image img = _AdsRectTrans.GetComponent<Image>();
            if (img == null)
            {
                img = _AdsRectTrans.gameObject.AddComponent<Image>();
            }

            img.sprite = input;
        }

        /// Nạp prefab UI từ Resources; thiếu prefab thì báo lỗi rõ đường dẫn thay vì để null
        /// trôi xuống gây NullReferenceException khó lần.
        private static BaseUIMenu LoadUIPrefab(string identifier)
        {
            var prefab = Resources.Load<BaseUIMenu>(DefaultDataPath + identifier);
            if (prefab == null)
            {
                Debug.LogError("[CanvasManager] Khong tim thay prefab UI '" + DefaultDataPath + identifier
                    + "'. Hay kiem tra ten prefab trong Resources hoac hang so duong dan.");
            }
            return prefab;
        }

        public static BaseUIMenu TryCacheUI(string identifier)
        {
            EnsureLayerStacksInitialized();
            if (!UICached.ContainsKey(identifier))
            {
                UICached[identifier] = new Stack<BaseUIMenu>();

                var prefab = LoadUIPrefab(identifier);
                if (prefab == null) return null;
                var cached = Instantiate(prefab, _UIRectTrans.GetChild((int) prefab.UILayer));
                cached.UIIdentifier = identifier;
                UICached[identifier].Push(cached);
                return cached;
            }
            else if (UICached[identifier].Count <= 0)
            {
                var prefab = LoadUIPrefab(identifier);
                if (prefab == null) return null;
                var cached = Instantiate(prefab, _UIRectTrans.GetChild((int) prefab.UILayer));
                cached.UIIdentifier = identifier;
                UICached[identifier].Push(cached);
#if UNITY_EDITOR
                if (cached.IsUnique) Debug.LogWarning(string.Format("UI {0} is Unique!!!", identifier));
#endif
                return cached;
            }

            return null;
        }

        public static System.Action<BaseUIMenu> EventOnMenuPushed;
        public static System.Action<BaseUIMenu> EventOnMenuPopped;

        /// Mở menu theo định danh (tên prefab trong Resources): lấy từ cache hoặc nạp mới, gọi Init(initParams) rồi đưa lên trên cùng tầng.
        /// Menu đã mở sẵn thì trả về chính nó, không mở chồng. Đẩy menu tầng Menu sẽ tự đóng hết tầng Popup. Trả null khi thiếu prefab.
        public static BaseUIMenu Push(string identifier, object[] initParams)
        {
            EnsureLayerStacksInitialized();
            BaseUIMenu existingMenu = IsSpecificUIShown(identifier);
            if (existingMenu != null)
            {
                return existingMenu;
            }

            if (TryCacheUI(identifier) == null && UICached[identifier].Count <= 0)
            {
                return null;
            }

            BaseUIMenu menu = UICached[identifier].Pop();
            if (menu.UILayer == eUILayer.Menu && OpenedUIStack[(int) eUILayer.Popup].Count > 0)
            {
                PopAllLayer(eUILayer.Popup);
            }

            menu.gameObject.SetActive(true);
            OpenedUIStack[(int) menu.UILayer].Add(menu);
            menu.Init(initParams);
            menu.ResetActiveTime();
            menu.transform.SetAsLastSibling();

            if (EventOnMenuPushed != null)
            {
                EventOnMenuPushed(menu);
            }

            return menu;
        }

        public static void PopTop(eUILayer layer)
        {
            EnsureLayerStacksInitialized();
            if (OpenedUIStack[(int) layer].Count <= 0)
            {
                return;
            }

            var layerGroup = OpenedUIStack[(int) layer];
            BaseUIMenu menu = layerGroup[layerGroup.Count - 1];
            menu.Pop();
        }

        public static bool PopSelf(BaseUIMenu menu, bool destroy = false)
        {
            if (menu == null)
            {
                return false;
            }

            EnsureLayerStacksInitialized();
            int layerIndex = (int) menu.UILayer;
            if (layerIndex < 0 || layerIndex >= OpenedUIStack.Count)
            {
                return CachePoppedMenu(menu, destroy);
            }

            var layerGroup = OpenedUIStack[layerIndex];
            if (layerGroup.Count <= 0)
            {
                return CachePoppedMenu(menu, destroy);
            }

            var index = layerGroup.FindIndex((x) => x == menu);
            if (index >= 0)
            {
                layerGroup.RemoveAt(index);
                return CachePoppedMenu(menu, destroy);
            }

            return CachePoppedMenu(menu, destroy);
        }

        /// Đóng menu đang mở theo định danh, quét từ tầng thấp lên cao và lấy cái đầu tiên khớp.
        /// Menu được tắt và trả về cache (tái dùng lần Push sau), không Destroy. Trả false nếu không có menu nào đang mở khớp định danh.
        public static bool Pop(string identifier)
        {
            EnsureLayerStacksInitialized();
            BaseUIMenu menu = null;
            for (int i = 0; i <= (int) eUILayer.AlwaysOnTop && menu == null; ++i)
            {
                menu = OpenedUIStack[i].Find((x) => x.UIIdentifier == identifier);
            }

            return menu != null ? PopSelf(menu) : false;
        }

        /// Đóng toàn bộ menu của một tầng, từ trên cùng xuống, qua BaseUIMenu.Pop() nên lớp con override vẫn chạy đúng.
        public static void PopAllLayer(eUILayer layer)
        {
            EnsureLayerStacksInitialized();
            List<BaseUIMenu> popList = new List<BaseUIMenu>(OpenedUIStack[(int) layer].ToArray());
            for (int i = popList.Count - 1; i >= 0; --i)
            {
                BaseUIMenu menu = popList[i];
                menu.Pop();
            }
        }

        public static bool IsPopupShown()
        {
            EnsureLayerStacksInitialized();
            return OpenedUIStack[(int) eUILayer.Popup].Count > 0;
        }

        public static BaseUIMenu GetCurrentMenu(eUILayer topLayer = eUILayer.AlwaysOnTop)
        {
            EnsureLayerStacksInitialized();
            for (int i = (int) topLayer; i >= 0; --i)
            {
                if (OpenedUIStack[i].Count > 0)
                {
                    return OpenedUIStack[i][OpenedUIStack[i].Count - 1];
                }
            }

            return null;
        }

        public static BaseUIMenu GetCurrentMenuByLayer(eUILayer layer)
        {
            EnsureLayerStacksInitialized();
            int i = (int) layer;
            if (OpenedUIStack[i].Count > 0)
            {
                return OpenedUIStack[i][OpenedUIStack[i].Count - 1];
            }

            return null;
        }

        /// Menu có định danh này đang mở hay không: trả về chính instance đang mở, null nếu không.
        /// Tên là "Is..." nhưng KHÔNG trả bool — kiểm tra bằng `!= null`.
        public static BaseUIMenu IsSpecificUIShown(string identifier)
        {
            EnsureLayerStacksInitialized();
            for (int i = 0; i < OpenedUIStack.Count; ++i)
            {
                var currentStack = OpenedUIStack[i];
                for (int j = 0; j < currentStack.Count; ++j)
                {
                    if (currentStack[j].UIIdentifier == identifier)
                    {
                        return currentStack[j];
                    }
                }
            }

            return null;
        }

        public static int GetUIStackCount(eUILayer layer)
        {
            EnsureLayerStacksInitialized();
            int i = (int) layer;
            return OpenedUIStack[i].Count;
        }

        /// Lấy instance menu để đọc/ghi dữ liệu mà KHÔNG mở nó: ưu tiên bản đang mở, sau đó bản trong cache,
        /// cuối cùng mới nạp prefab nếu autoCreated. Instance nạp mới đang tắt — muốn hiện phải gọi <see cref="Push"/>.
        /// Trả null khi không tìm được prefab.
        public static BaseUIMenu GetMenu(string identifier, bool autoCreated = true)
        {
            var result = IsSpecificUIShown(identifier);
            if (result != null) return result;
            if (UICached.ContainsKey(identifier) && UICached[identifier].Count > 0)
            {
                result = UICached[identifier].Peek();
            }
            else if (autoCreated)
            {
                // Thiếu prefab thì TryCacheUI đã báo lỗi và không đẩy gì vào stack — Peek lúc đó ném
                // InvalidOperationException, nên phải kiểm tra trước.
                TryCacheUI(identifier);
                if (UICached.ContainsKey(identifier) && UICached[identifier].Count > 0)
                {
                    result = UICached[identifier].Peek();
                }
            }

            return result;
        }

        public static void AddUIToCache(BaseUIMenu menu)
        {
            EnsureLayerStacksInitialized();
            if (menu.UIIdentifier == null)
                menu.UIIdentifier = menu.name;
            if (!UICached.ContainsKey(menu.UIIdentifier))
                UICached[menu.UIIdentifier] = new Stack<BaseUIMenu>();
            UICached[menu.UIIdentifier].Push(menu);
            menu.gameObject.SetActive(false);
        }

        /// Khởi động hệ UI: đặt thư mục gốc chứa prefab trong Resources rồi mở luôn menu mặc định.
        /// dataPath là đường dẫn tương đối trong Resources và PHẢI có dấu "/" cuối (ví dụ "UI/Menu/") vì được nối thẳng với định danh.
        /// Gọi đúng một lần lúc boot, sau khi Awake của CanvasManager đã chạy — sai thứ tự sẽ có log cảnh báo trong Editor.
        public static BaseUIMenu Init(string dataPath, string defaultMenuIdentifier)
        {
#if UNITY_EDITOR
            if (!sFinishAwake) Debug.LogWarning("[ERROR] CanvasManager priority is not set correctly!!!");
#endif
            DefaultDataPath = dataPath;
            return Push(defaultMenuIdentifier, null);
        }

        public static void SetRenderCamera(Camera newCamera)
        {
            _UICanvas.worldCamera = newCamera;
        }

        float mLastBackeyTime = -1;

        private void Update()
        {
            var topMenuLayer = GetCurrentMenuByLayer(eUILayer.Menu);
            if (topMenuLayer != null) topMenuLayer.UpdateActiveTime(Time.unscaledDeltaTime);

#if UNITY_ANDROID || UNITY_EDITOR
            if ((sSystemLoadingPopup == null || !sSystemLoadingPopup.activeSelf) && Application.isFocused &&
                Input.GetKey(KeyCode.Escape) && mLastBackeyTime < Time.unscaledTime)
            {
                mLastBackeyTime = Time.unscaledTime + 0.15f;
                var topMenu = GetCurrentMenu();
                if (topMenu != null)
                {
                    topMenu.HandleSafeChoice();
                }
            }
#endif
        }

        public static GameObject sSystemLoadingPopup = null;

        public static void ShowSystemLoadingPopup(bool show)
        {
            if (sSystemLoadingPopup == null)
            {
                sSystemLoadingPopup = new GameObject("SystemLoadingPopup");
                sSystemLoadingPopup.transform.SetParent(UICanvas.transform);
                sSystemLoadingPopup.transform.localPosition = Vector2.zero;
                sSystemLoadingPopup.transform.localScale = Vector2.one;
                var rect = sSystemLoadingPopup.AddComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.anchoredPosition = Vector2.zero;
            }

            sSystemLoadingPopup.SetActive(show);
        }

        public static bool IsSystemLoadingScreenShowing()
        {
            return sSystemLoadingPopup != null ? sSystemLoadingPopup.gameObject.activeSelf : false;
        }

        public static void DestroyAllUICanDestroy()
        {
            EnsureLayerStacksInitialized();
            List<KeyValuePair<string, Stack<BaseUIMenu>>> listClear =
                new List<KeyValuePair<string, Stack<BaseUIMenu>>>();
            foreach (var group in UICached)
            {
                var list = new List<BaseUIMenu>();
                while (group.Value.Count > 0)
                {
                    var menu = group.Value.Pop();

                    var check = OpenedUIStack[(int) menu.UILayer].Contains(menu);
                    if (menu.CanDestroy && !check)
                    {
                        listClear.Add(group);
                        Destroy(menu.gameObject);
                    }
                    else
                    {
                        Debug.Log(menu.UIIdentifier);
                        list.Add(menu);
                    }
                }

                foreach (var menu in list)
                    group.Value.Push(menu);
            }

            foreach (var pair in listClear)
            {
                if (pair.Value.Count <= 0)
                {
                    // Debug.Log("Destroy " + pair.Key);
                    UICached.Remove(pair.Key);
                }
            }
        }

        public static BaseUIMenu[] GetAllOpenedUI()
        {
            List<BaseUIMenu> result = new List<BaseUIMenu>();
            for (int i = 0; i < OpenedUIStack.Count; ++i)
            {
                var childList = OpenedUIStack[i];
                for (int j = 0; j < childList.Count; ++j)
                {
                    result.Add(childList[j]);
                }
            }

            return result.ToArray();
        }
    }
}
