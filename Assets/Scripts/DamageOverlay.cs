using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageOverlay : MonoBehaviour
{
    public static DamageOverlay Instance;

    [SerializeField] private Image overlay;

    public float fadeSpeed = 2f;

    Coroutine fadeRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(float alpha)
    {
        Color c = overlay.color;
        c.a = alpha;
        overlay.color = c;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (overlay.color.a > 0)
        {
            Color c = overlay.color;
            c.a -= fadeSpeed * Time.deltaTime;
            overlay.color = c;

            yield return null;
        }

        Color color = overlay.color;
        color.a = 0;
        overlay.color = color;
    }
}