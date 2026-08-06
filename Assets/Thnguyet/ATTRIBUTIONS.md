# Nguồn gốc mã bên thứ ba trong Thnguyet Framework

Framework này chứa mã của người khác. Tên type, namespace và assembly đã đổi sang `Thnguyet.*` để gộp về framework, **nhưng bản quyền không đổi theo**. Giữ nguyên file này và mọi header bản quyền trong mã nguồn khi bê framework sang project khác.

---

## `Thnguyet.GameFeel/` — 791 file

| | |
|---|---|
| Tên gốc | **Feel** (MMFeedbacks + MMTools) |
| Tác giả | More Mountains (Renaud Forestié) |
| Namespace gốc | `MoreMountains.Feedbacks`, `MoreMountains.Tools`, `MoreMountains.FeedbacksForThirdParty` |
| Assembly gốc | `MoreMountains.Tools`, `MoreMountains.Tools.Editor` |
| Hiện tại | `Thnguyet.GameFeel`, `Thnguyet.GameFeel.Feedbacks`, `Thnguyet.GameFeel.ThirdParty` · assembly `Thnguyet.GameFeel`, `Thnguyet.GameFeel.Editor` |
| Đổi tên | Bỏ toàn bộ tiền tố `MM`/`MMF_` khỏi 953 type, 752 file, 103 thư mục |

> **ASSET TRẢ PHÍ — Unity Asset Store EULA.** Cấp phép theo tài khoản mua, mỗi lập trình viên một seat.
> Được dùng lại trong các project của **chính người mua**. **KHÔNG được** phân phối lại, đưa cho bên
> thứ ba, hay đẩy lên repo công khai. Nếu chia sẻ framework này cho ai khác, **phải gỡ thư mục này ra trước.**
>
> Việc đổi tên khiến **không thể update từ Asset Store nữa** — bản gốc còn trong git tại `Assets/Feel`.

## `Thnguyet.NiceVibrations/` — 16 file

| | |
|---|---|
| Tên gốc | **Nice Vibrations** |
| Tác giả | Lofelt (nay thuộc More Mountains) |
| Namespace gốc | `Lofelt.NiceVibrations` |
| Assembly gốc | `Lofelt.NiceVibrations`, `Lofelt.NiceVibrations.Editor` |
| Hiện tại | assembly đổi thành `Thnguyet.NiceVibrations` (+ `.Editor`); **namespace giữ nguyên `Lofelt.NiceVibrations`** |
| Nội dung | Haptic đa nền tảng, kèm plugin native iOS (`LofeltHaptics.framework`) và Android |

> **ASSET TRẢ PHÍ — Unity Asset Store EULA.** Cùng ràng buộc như Feel ở trên.

## `Thnguyet.SOArchitecture/` — 180 file

| | |
|---|---|
| Tên gốc | ScriptableObject Architecture |
| Tác giả | Daniel Everland |
| Namespace gốc | `ScriptableObjectArchitecture` (+ `.Editor`) |
| Assembly gốc | `ScriptableObject-Architecture`, `ScriptableObject-Architecture.Editor` |
| Hiện tại | `Thnguyet.ScriptableObjectArchitecture` · assembly `Thnguyet.Editor` |
| Nội dung | `GameEvent`/`GameEventListener`, `*Variable`, `*Reference`, `*Collection`, code generation, drawer và inspector riêng |

**LICENSE: CHƯA XÁC NHẬN.** Bản import không kèm file LICENSE. Dự án gốc thường phát hành theo MIT, nhưng phải tự tra lại repo và chép LICENSE vào đây.

## `Thnguyet.UnityExtensions/` — 104 file

| | |
|---|---|
| Tên gốc | Unity Extensions |
| Namespace gốc | `UnityExtensions`, `UnityExtensions.Tween`, `UnityExtensions.Paths` (+ `.Editor`) |
| Assembly gốc | `UnityExtensions` |
| Hiện tại | thêm tiền tố `Thnguyet.` |
| Nội dung | attribute cho Inspector, base class editor, hệ tween serialize được (`TweenPlayer`), path Bezier/Cardinal có gizmo, `Range`/`TreeNode`/`TaskQueue`/state machine |

**LICENSE: CHƯA XÁC ĐỊNH.** Không có file LICENSE, không file `.cs` nào có header bản quyền. Cần tra lại repo gốc trên GitHub và chép LICENSE vào đây.

## `Thnguyet.Common.Runtime/com.spacepuppy.Collections/` — 1 file

| | |
|---|---|
| Tên gốc | Spacepuppy Framework — `SerializableDictionaryBase` |
| Namespace | `com.spacepuppy.Collections` (giữ nguyên) |
| Nội dung | `DrawableDictionary`, `SerializableDictionaryBase<,>` — nền cho `SerializedDictionary` |

**LICENSE: CHƯA XÁC ĐỊNH.**

---

## Việc cần làm

MIT, Apache-2.0 và BSD đều **bắt buộc giữ lại thông báo bản quyền** khi phân phối lại. Đổi namespace không phải vấn đề; xoá tên tác giả mới là.

1. Tra và chép file LICENSE cho `UnityExtensions`, `SOArchitecture`, `spacepuppy`.
2. Nếu một trong số đó hoá ra không có license mở → gỡ khỏi framework trước khi đưa cho bên thứ ba.
3. `GameFeel` và `NiceVibrations` là asset trả phí → luôn gỡ ra trước khi chia sẻ framework.
