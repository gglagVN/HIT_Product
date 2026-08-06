# Thnguyet Framework

Bộ khung Unity dùng lại giữa các dự án: UI, âm thanh, nạp asset, pool, event, save, config, game feel, haptic.

Copy nguyên thư mục `Assets/Thnguyet` sang project mới là chạy được, với điều kiện cài đủ phụ thuộc ở mục 2.

---

## 1. Luật duy nhất: phụ thuộc chỉ đi một chiều

> **Game → Framework. KHÔNG BAO GIỜ Framework → Game.**

Framework không được biết gì về game cụ thể: không tên màn hình, không enum của game, không manager của game, không `using` sang assembly game. Thiếu thứ gì thì **sửa framework cho tổng quát hơn**, đừng kéo game vào.

### Khi framework cần gọi ngược lên game: dùng static hook

Framework khai báo `Action`/`Func` static, game gán lúc boot, framework chỉ gọi khi khác null.

```csharp
// Framework — Thnguyet.UI/UIButtonPress.cs
public static Action PlayClickSound;
if (PlayClickSound != null) PlayClickSound();
```

```csharp
// Game — gán một lần lúc boot
UIButtonPress.PlayClickSound = () => AudioService.Instance.PlaySfx(clickSfx);
```

**Nhớ gỡ hook khi thoát game hoặc reload domain** — static không tự reset, hook trỏ vào object đã huỷ sẽ giữ tham chiếu chết.

---

## 2. Mang sang project mới

### Phụ thuộc bắt buộc — cài TRƯỚC khi copy framework vào

| Phụ thuộc | Vì sao |
|---|---|
| **DOTween** | `Thnguyet.asmdef` tham chiếu `DOTween.Modules`. Thiếu là **cả assembly không compile**, kéo theo mọi module. Cài DOTween và chạy Setup của nó trước tiên. |
| **TextMeshPro** | `Unity.TextMeshPro` |
| **uGUI** | `UnityEngine.UI` |
| **URP** | `Unity.RenderPipelines.Universal.Runtime` |

Nếu project mới không dùng URP, phải bỏ tham chiếu đó khỏi `Thnguyet.asmdef` và gỡ code URP trong `Thnguyet.GameFeel`.

### Thứ tự làm

1. Cài DOTween → chạy Setup → đợi compile xong.
2. Cài TextMeshPro Essentials.
3. Copy `Assets/Thnguyet` vào.
4. Đợi Unity import (~1200 asset), kiểm tra Console sạch lỗi.

---

## 3. Assembly và namespace

Framework chia thành **6 assembly**:

| Assembly | File asmdef | Phủ |
|---|---|---|
| `Thnguyet` | `Thnguyet.asmdef` (gốc) | Toàn bộ module tự viết + `SOArchitecture` + `UnityExtensions` |
| `Thnguyet.Editor` | `Thnguyet.SOArchitecture/Editor/ScriptableObject-Architecture.Editor.asmdef` | Code editor của SOArchitecture |
| `Thnguyet.GameFeel` | `Thnguyet.GameFeel/Tools/Core/` | Toàn bộ GameFeel (qua 16 file `.asmref`) |
| `Thnguyet.GameFeel.Editor` | `Thnguyet.GameFeel/Tools/Core/Editor/` | Editor của GameFeel |
| `Thnguyet.NiceVibrations` | `Thnguyet.NiceVibrations/` | Haptic |
| `Thnguyet.NiceVibrations.Editor` | `Thnguyet.NiceVibrations/Scripts/Editor/` | Editor của haptic |

Game muốn dùng module nào thì asmdef của game phải tham chiếu assembly tương ứng. `Thnguyet` không tự động thấy `Thnguyet.GameFeel` và ngược lại.

**Lưu ý:** tên file `ScriptableObject-Architecture.Editor.asmdef` không khớp tên assembly `Thnguyet.Editor` bên trong. Đây là nợ kỹ thuật, xem mục 7.

---

## 4. Bản đồ module

Module tự viết có hậu tố `.Runtime`; thư viện bên thứ ba đã vendor thì không có hậu tố.

| Thư mục | File | Nội dung | Tình trạng |
|---|---|---|---|
| `Thnguyet.Audio.Runtime` | 9 | `AudioService` (cửa vào duy nhất), `AudioManager` (pool SFX), `MusicManager` (crossfade), `AudioMixerManager`, `AudioSO`/`AudioConfigSO`/`AudioClipGroup` | **Production-ready** — module hoàn chỉnh nhất |
| `Thnguyet.AssetManagement.Runtime` | 20 | Nạp asset có đếm tham chiếu: `AssetManager`, `AssetLoader`, `AssetRequest`, `NormalizedPath` + 3 backend | **Một nửa** — lõi và backend Resources/EditorDatabase chạy tốt; nhánh AssetBundle rỗng |
| `Thnguyet.Common.Runtime` | 11 | `ObjectPool`, `ComponentPool`, `CollectionPool`, `StringBuilderPool`, `SerializedDictionary`, extension cho IList/Transform/GameObject/Delegate | **Production-ready** |
| `Thnguyet.UI.Runtime` | 8 | **Hai hệ UI song song** — xem mục 6.2 | **Dùng được**, nhưng phải chọn một hệ |
| `Thnguyet.Modules.Runtime` | 32 | Lõi tạp: 7 lớp Singleton, `EventManager`, `EventChannelSO`, state machine, `Timer`, `CoroutineUpdater`, extension, `*Util`, spline, `Presentation` | **Chạy được nhưng tên vô nghĩa** — xem mục 7 |
| `Thnguyet.ScriptableConfig.Runtime` | 2 | `ConfigSO`, `ConfigTableSO<TKey,TValue>` | **Production-ready** |
| `Thnguyet.SaveGame.Runtime` | 1 | `SaveGameManager` + `ISaveData` | **Dùng tạm** — thực chất là PlayerPrefs; tên hàm nói "file" nhưng không ghi file, không có backup |
| `Thnguyet.DebugCommand.Runtime` | 6 | Console lệnh debug trong game | **KHÔNG DÙNG ĐƯỢC** — rỗng 100%, xem mục 5 |
| `Thnguyet.GameFeel` | 791 | Feedback/juice (`FeedbackPlayer`), shaker, spring, tween, pool, sound manager, state machine, scene loading, achievements, AI, loot | **Production-ready** — vendor |
| `Thnguyet.NiceVibrations` | 16 | Haptic, có plugin native iOS/Android | **Production-ready** — vendor |
| `Thnguyet.SOArchitecture` | 180 | `GameEvent`, `GameEventListener`, `*Variable`, `*Reference`, `*Collection`, code generation | **Production-ready** — vendor |
| `Thnguyet.UnityExtensions` | 104 | Attribute Inspector, base class editor, tween serialize được (`TweenPlayer`), path Bezier/Cardinal, `Range`/`TreeNode`/`TaskQueue` | **Production-ready** — vendor |

Tổng ~37 MB, ~1180 file `.cs`.

---

## 5. CHƯA CÀI ĐẶT — đọc trước khi gọi API

Framework này dựng từ **bản decompile bị bóc thân hàm**. Một số chỗ chỉ còn vỏ. Những chỗ đó **ném `NotImplementedException` ngay ở constructor** kèm thông báo tiếng Việt, nên không thể lỡ dùng nhầm mà không biết.

| Chỗ rỗng | Hành vi | Dùng gì thay |
|---|---|---|
| `Thnguyet.DebugCommand.Runtime` — **cả module** | `DebugCommandConsole` ctor ném | Console có sẵn của project, ví dụ `Assets/Plugins/IngameDebugConsole` |
| `Thnguyet.AssetManagement` — **toàn bộ nhánh AssetBundle** (8 file) | `AssetManagerAssetBundle` và mọi request/loader/downloader đều ném | `AssetManagerResources` hoặc `AssetManagerEditorDatabase`; hoặc viết `AssetManager` mới trên nền Addressables |
| `SceneRequest` | ctor ném | `UnityEngine.SceneManagement` trực tiếp. `AssetManager` **không hề có** `LoadSceneAsync` |

Bên trong các file đó còn nhiều hàm `return default;` — nhưng constructor đã ném trước nên không với tới được. Đừng gỡ lệnh ném ra mà không cài đặt lại phần thân.

**Không nằm trong danh sách trên tức là đã cài đặt thật.** `return default;` trong `ConfigTableSO.Get()` là đường lỗi hợp lệ (log rồi trả mặc định), không phải vỏ rỗng.

---

## 6. Bẫy hay gặp

### 6.1. Đổi tên namespace hoặc assembly sẽ ÂM THẦM xoá dữ liệu `[SerializeReference]`

Trường `[SerializeReference]` lưu **tên class + namespace + tên assembly** vào file YAML. Đổi bất kỳ cái nào trong ba → Unity không tìm ra type → gán null. **Không lỗi compile, không cảnh báo.**

`FeedbackPlayer` trong `Thnguyet.GameFeel` dùng `[SerializeReference]` cho danh sách feedback. Nếu buộc phải đổi tên, phải sửa cả key trong file `.prefab`/`.unity`:

```yaml
type: {class: FeedbackScale, ns: Thnguyet.GameFeel.Feedbacks, asm: Thnguyet.GameFeel}
```

Đổi **tên field** serialize cũng mất dữ liệu tương tự — dùng `[FormerlySerializedAs]` nếu bắt buộc.

### 6.2. Hai hệ UI song song — chọn một, đừng trộn

| | Hệ 1 | Hệ 2 |
|---|---|---|
| Vào bằng | `CanvasManager` (`AutoSingleton`) | `UIManager` |
| Màn hình kế thừa | `BaseUIComp` → `BaseUIMenu` | `UIBase` |
| Cách mở | theo tầng `eUILayer`, nạp từ Resources | `Open<T>()` theo kiểu, có state + animation hook |

Trộn hai hệ trên cùng một màn hình sẽ tranh nhau quản lý stack. Chọn một hệ cho mỗi màn hình.

### 6.3. Bảy lớp Singleton base — biết mình đang kế thừa cái nào

`Singleton<T>`, `AutoSingleton<T>`, `SingletonMono<T>`, `ManualSingletonMono<T>`, `SingletonUI<T>`, `SceneSingleton<T>`, `MonoSingleton<T>`.

Khác biệt then chốt: **có tự tạo instance khi chưa có hay không**, và **có sống qua load scene hay không**. Đọc file trước khi kế thừa. `AudioService : SceneSingleton`, `CanvasManager : AutoSingleton`.

### 6.4. Ba hệ event song song

`Thnguyet.Event.EventManager` (bus theo struct) · `Thnguyet.EventScriptable.EventChannelSO` · `Thnguyet.ScriptableObjectArchitecture` GameEvent. Chọn một cho mỗi luồng, đừng phát ở hệ này rồi nghe ở hệ kia.

### 6.5. Thư mục `EditorOnly` không phải thư mục Editor

`Thnguyet.UnityExtensions/Common/EditorOnly/` **không tên là `Editor`** nên Unity không tự loại khỏi build. Nó chỉ được chặn bằng `#if UNITY_EDITOR`. Nếu thêm file mới vào đó, tự nhớ bọc `#if`.

---

## 7. Quy ước đặt tên

### Type trong `Thnguyet.GameFeel`

Vendor gốc dùng tiền tố `MM`. Đã bỏ hết theo quy tắc:

- `MMF_X` → `FeedbackX`
- `MMX` → `X`
- Nếu `X` đụng tên UnityEngine/System, đụng type khác trong project, hoặc bị field cùng tên che → **`FeelX`** (`FeelDebug`, `FeelTime`, `FeelChannel`, `FeelTilemap`, `FeelGUI`, `FeelSingleton`, `FeelSoundManager`…)

**Khi thêm type mới vào `Thnguyet.GameFeel`, kiểm tra tên có đụng UnityEngine không trước khi đặt.** Type nằm trong `namespace Thnguyet.GameFeel` sẽ *che* type UnityEngine cùng tên ngay trong chính các file đó.

### Preprocessor define

Define theo package của GameFeel dùng tiền tố `GAMEFEEL_`: `GAMEFEEL_URP`, `GAMEFEEL_CINEMACHINE`, `GAMEFEEL_CINEMACHINE3`, `GAMEFEEL_HDRP`, `GAMEFEEL_POSTPROCESSING`, `GAMEFEEL_UI`, `GAMEFEEL_UGUI2`, `GAMEFEEL_PHYSICS2D`, `GAMEFEEL_INPUTSYSTEM`, `GAMEFEEL_TEXTMESHPRO`, `GAMEFEEL_VISUALEFFECTGRAPH`.

Chúng khai báo ở `versionDefines` trong `Thnguyet.GameFeel.asmdef`, **không** ở Project Settings — sửa ở asmdef.

`GAMEFEEL_CINEMACHINE_LEGACY` / `_LEGACY3` là define **cố tình không bao giờ định nghĩa**, giữ vài khối code cũ ở trạng thái tắt. Đừng khai báo chúng.

Riêng `MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED` vẫn nằm ở Project Settings (Android/iOS/Standalone), do `NiceVibrations/Define/` tự quản lý. Đổi tên nó phải sửa cả script sinh define lẫn Project Settings.

### Nợ kỹ thuật đã biết

Chưa sửa, ghi lại để khỏi quên:

| Vấn đề | Chi tiết |
|---|---|
| `Thnguyet.Modules.Runtime` là túi đựng tạp | "Modules" không mô tả gì. Nên tách: Core (singleton, coroutine), Events, StateMachine, Sequencing, Math/Spline, Cryptography; chuyển `UIBehavior/` sang `Thnguyet.UI.Runtime`, gộp `Extensions/` về `Common` |
| Hậu tố `.Runtime` chỉ là trang trí | Không có asmdef riêng cho từng module, nên hậu tố không ứng với ranh giới assembly nào. Hoặc tách asmdef theo module, hoặc bỏ hậu tố |
| `Thnguyet.Util` vs `Thnguyet.Utils` | Hai namespace, cái đầu chỉ có đúng 1 file `ObfuscatorXor.cs` |
| `Thnguyet.Extensions` nằm ở hai module | Vừa trong `Common.Runtime` (4 file) vừa trong `Modules.Runtime` (2 file) |
| Namespace lệch thư mục | Thư mục `Audio` ↔ namespace `AudioManagement`; thư mục `SOArchitecture` ↔ namespace `ScriptableObjectArchitecture`; thư mục `NiceVibrations` ↔ namespace `Lofelt.NiceVibrations` |
| Lỗi chính tả | `TranformExtensions` (thiếu chữ *s*) |
| `MD5Util` trùng tên ở hai namespace | `Thnguyet.Utils` và nhánh AssetBundle downloader |
| `Utility.cs` | God-class 472 dòng, chứa thêm class `ReflectionWrapper` |
| `enum eUILayer` | Hungarian notation; nên là `UILayer` |

---

## 8. Bản quyền

Framework chứa mã của người khác — xem `ATTRIBUTIONS.md`. Namespace đã đổi tiền tố sang `Thnguyet.*` **nhưng bản quyền không đổi theo**.

Đặc biệt: `Thnguyet.GameFeel` và `Thnguyet.NiceVibrations` là **asset trả phí trên Asset Store**, cấp phép theo tài khoản. Mang sang project riêng của bạn thì được; **đưa cho bên thứ ba là vi phạm license**.
