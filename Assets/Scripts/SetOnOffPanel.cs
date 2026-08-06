using DG.Tweening;
using UnityEngine;

public class SetOnOffPanel : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float closeAnimationTime = 1f;
    [SerializeField] private float openAnimationTime = 0.25f;
    [SerializeField] private float closedScale = 0.85f;

    private CanvasGroup[] canvasGroups;
    private RectTransform[] rectTransforms;
    private Sequence panelSequence;

    private bool isOpen = false;
    private bool isAnimating = false;

    private void Awake()
    {
        int count = gameObjects != null ? gameObjects.Length : 0;

        canvasGroups = new CanvasGroup[count];
        rectTransforms = new RectTransform[count];

        for (int i = 0; i < count; i++)
        {
            GameObject go = gameObjects[i];
            if (go == null) continue;

            canvasGroups[i] = go.GetComponent<CanvasGroup>();
            rectTransforms[i] = go.GetComponent<RectTransform>();

            if (canvasGroups[i] == null)
            {
                Debug.LogError(
                    "SetOnOffPanel: '" + go.name + "' chưa có CanvasGroup. " +
                    "Hãy thêm component CanvasGroup cho object này trong Inspector để fade hoạt động.", go);
            }

            Animator anim = go.GetComponent<Animator>();
            if (anim != null)
            {
                anim.enabled = false;
            }

            go.SetActive(false);
        }
    }

    public void TogglePanels()
    {
        if (isAnimating)
            return;

        if (isOpen)
        {
            ClosePanels();
        }
        else
        {
            OpenPanels();
        }
    }

    private void OpenPanels()
    {
        KillSequence();

        isOpen = true;
        isAnimating = true;

        panelSequence = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];
            if (go == null) continue;

            go.SetActive(true);

            CanvasGroup group = canvasGroups[i];
            if (group != null)
            {
                group.alpha = 0f;
                group.interactable = false;
                group.blocksRaycasts = false;

                panelSequence.Join(group.DOFade(1f, openAnimationTime).SetEase(Ease.OutQuad));
            }

            RectTransform rect = rectTransforms[i];
            if (rect != null)
            {
                rect.localScale = Vector3.one * closedScale;

                panelSequence.Join(rect.DOScale(1f, openAnimationTime).SetEase(Ease.OutBack));
            }
        }

        panelSequence.OnComplete(() =>
        {
            SetPanelsInteractable(true);
            isAnimating = false;
            panelSequence = null;
        });
    }

    private void ClosePanels()
    {
        KillSequence();

        isOpen = false;
        isAnimating = true;

        SetPanelsInteractable(false);

        panelSequence = DOTween.Sequence().SetUpdate(true).SetLink(gameObject);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];
            if (go == null || !go.activeSelf) continue;

            CanvasGroup group = canvasGroups[i];
            if (group != null)
            {
                panelSequence.Join(group.DOFade(0f, closeAnimationTime).SetEase(Ease.InQuad));
            }

            RectTransform rect = rectTransforms[i];
            if (rect != null)
            {
                panelSequence.Join(rect.DOScale(closedScale, closeAnimationTime).SetEase(Ease.InBack));
            }
        }

        panelSequence.OnComplete(() =>
        {
            HideAllPanels();
            isAnimating = false;
            panelSequence = null;
        });
    }

    public void ForceClose()
    {
        KillSequence();

        isOpen = false;
        isAnimating = false;

        HideAllPanels();
    }

    private void HideAllPanels()
    {
        if (canvasGroups == null) return;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];
            if (go == null) continue;

            CanvasGroup group = canvasGroups[i];
            if (group != null)
            {
                group.alpha = 0f;
                group.interactable = false;
                group.blocksRaycasts = false;
            }

            RectTransform rect = rectTransforms[i];
            if (rect != null)
            {
                rect.localScale = Vector3.one * closedScale;
            }

            go.SetActive(false);
        }
    }

    private void SetPanelsInteractable(bool value)
    {
        if (canvasGroups == null) return;

        for (int i = 0; i < canvasGroups.Length; i++)
        {
            CanvasGroup group = canvasGroups[i];
            if (group == null) continue;

            group.interactable = value;
            group.blocksRaycasts = value;
        }
    }

    private void KillSequence()
    {
        if (panelSequence != null && panelSequence.IsActive())
        {
            panelSequence.Kill();
        }

        panelSequence = null;
    }

    private void OnDisable()
    {
        KillSequence();
    }
}
