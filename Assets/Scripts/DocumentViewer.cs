using UnityEngine;
using UnityEngine.EventSystems;

public class DocumentViewer : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("Reference")]
    public RectTransform viewport;
    public RectTransform paper;

    [Header("Zoom")]
    public float zoomSpeed = 0.2f;
    public float minZoom = 1f;
    public float maxZoom = 3f;

    float currentZoom = 1f;

    Vector2 startPosition;

    private bool dragging;

    void Start()
    {
        startPosition = paper.anchoredPosition;
    }

    void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) < 0.01f)
            return;

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            viewport,
            Input.mousePosition,
            null,
            out localPoint);

        Vector2 before =
            (localPoint - paper.anchoredPosition) / currentZoom;

        currentZoom += scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        paper.localScale = Vector3.one * currentZoom;

        Vector2 after =
            before * currentZoom;

        paper.anchoredPosition =
            localPoint - after;

        ClampPosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentZoom <= 1f)
            return;

        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging)
            return;

        paper.anchoredPosition += eventData.delta;

        ClampPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    void ClampPosition()
    {
        Vector2 imageSize =
            paper.rect.size * currentZoom;

        Vector2 viewSize =
            viewport.rect.size;

        float limitX =
            Mathf.Max(0, (imageSize.x - viewSize.x) / 2);

        float limitY =
            Mathf.Max(0, (imageSize.y - viewSize.y) / 2);

        Vector2 pos = paper.anchoredPosition;

        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);
        pos.y = Mathf.Clamp(pos.y, -limitY, limitY);

        paper.anchoredPosition = pos;
    }

    public void ResetViewer()
    {
        currentZoom = 1;

        paper.localScale = Vector3.one;

        paper.anchoredPosition = startPosition;
    }
}