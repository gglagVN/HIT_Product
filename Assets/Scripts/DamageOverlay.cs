using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour
{
    public static DamageOverlay Instance;

    [SerializeField] private Image overlay;

    public float fadeSpeed = 2f;

    private Tween fadeTween;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(float alpha)
    {
        if (overlay == null)
        {
            Debug.LogError("DamageOverlay: chưa gán Image 'overlay' trong Inspector.", this);
            return;
        }

        KillFadeTween();

        Color c = overlay.color;
        c.a = alpha;
        overlay.color = c;

        float duration = fadeSpeed > 0f ? alpha / fadeSpeed : 0f;

        fadeTween = overlay
            .DOFade(0f, duration)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }

    private void KillFadeTween()
    {
        if (fadeTween != null && fadeTween.IsActive())
        {
            fadeTween.Kill();
        }

        fadeTween = null;
    }

    private void OnDisable()
    {
        KillFadeTween();
    }
}
