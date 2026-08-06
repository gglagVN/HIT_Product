using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Thnguyet.UI
{
// Hiệu ứng nhún khi nhấn nút: thu nhỏ lúc nhấn, bật lại (hơi nảy) lúc thả hoặc trượt tay ra ngoài.
// Nút disable (Button.interactable = false) thì không hiệu ứng, không tiếng.
// Gắn cạnh Button. Dùng unscaled time nên chạy cả khi game pause (timeScale = 0).
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class UIButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // Tiếng click do game cắm vào từ ngoài — thư viện không phụ thuộc hệ âm thanh của game.
    public static Action PlayClickSound;

    [SerializeField] float pressedScale = 0.92f;
    [SerializeField] float downTime     = 0.09f;
    [SerializeField] float upTime       = 0.16f;

    Vector3 _base = Vector3.one;
    bool    _captured;
    Button  _button;

    void Awake() { _base = transform.localScale; _captured = true; _button = GetComponent<Button>(); }

    public void OnPointerDown(PointerEventData e)
    {
        if (_button != null && !_button.interactable) return;
        if (PlayClickSound != null) PlayClickSound();
        if (!_captured) { _base = transform.localScale; _captured = true; }
        transform.DOKill();
        transform.DOScale(_base * pressedScale, downTime).SetUpdate(true).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData e)
    {
        transform.DOKill();
        transform.DOScale(_base, upTime).SetUpdate(true).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (!_captured) return;
        transform.DOKill();
        transform.DOScale(_base, upTime).SetUpdate(true).SetEase(Ease.OutQuad);
    }

    void OnDisable()
    {
        transform.DOKill();
        if (_captured) transform.localScale = _base;
    }
}

}
