using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] private float vibratoPerSecond = 60f;
    [SerializeField] private float randomness = 90f;

    private Vector3 originalPos;
    private Tween shakeTween;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        KillShakeTween();

        transform.localPosition = originalPos;

        int vibrato = Mathf.Max(1, Mathf.RoundToInt(duration * vibratoPerSecond));

        shakeTween = transform
            .DOShakePosition(duration, new Vector3(magnitude, magnitude, 0f), vibrato, randomness, false, true)
            .SetLink(gameObject)
            .OnComplete(() => transform.localPosition = originalPos);
    }

    private void KillShakeTween()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }

        shakeTween = null;
    }

    private void OnDisable()
    {
        bool wasShaking = shakeTween != null && shakeTween.IsActive();

        KillShakeTween();

        if (wasShaking)
        {
            transform.localPosition = originalPos;
        }
    }
}
