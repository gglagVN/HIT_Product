using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI layer for the hacking puzzle. It renders nodes, lines and basic visual feedback.
/// </summary>
public class HackCanvas : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform nodeContainer;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject startNodePrefab;
    [SerializeField] private GameObject endNodePrefab;
    [SerializeField] private GameObject virusNodePrefab;
    [SerializeField] private GameObject firewallNodePrefab;
    [SerializeField] private GameObject keyNodePrefab;
    [SerializeField] private GameObject bonusNodePrefab;
    [SerializeField] private GameObject teleportNodePrefab;
    [SerializeField] private Text timerText;

    [Header("Colors")]
    [SerializeField] private Color normalColor = new Color(0.85f, 0.95f, 1f, 1f);
    [SerializeField] private Color activeColor = new Color(1f, 0.95f, 0.35f, 1f);
    [SerializeField] private Color visitedColor = new Color(0.25f, 0.72f, 0.9f, 1f);
    [SerializeField] private Color invalidColor = Color.red;
    [SerializeField] private Color virusColor = new Color(1f, 0.2f, 0.35f, 1f);
    [SerializeField] private Color firewallColor = new Color(0.9f, 0.35f, 1f, 1f);
    [SerializeField] private Color endColor = new Color(0.15f, 0.95f, 0.45f, 1f);
    [SerializeField] private Color failureOverlayColor = new Color(1f, 0.15f, 0.15f, 0.75f);
    [SerializeField] private Color successOverlayColor = new Color(0.15f, 0.92f, 0.28f, 0.75f);
    [SerializeField] private float failureEffectDuration = 0.6f;

    [Header("Password Puzzle")]
    [SerializeField] private string correctPassword = "210125";
    [SerializeField] private int passwordLength = 6;
    public string CorrectPassword => correctPassword;
    public int PasswordLength => passwordLength;

    private readonly Dictionary<HackNodeType, Sprite> generatedSprites = new Dictionary<HackNodeType, Sprite>();
    private Image overlayImage;
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TextMeshProUGUI passwordDisplayText;

    [SerializeField] private Button[] numberButtons;

    [SerializeField] private Button clearButton;
    [SerializeField] private Button enterButton;
    private string enteredPassword = "";

    private HackManager hackManager;
    // UI -> Gameplay events. HackCanvas does not contain game rules,
    // it only forwards pointer events from visuals to the HackManager.
    public System.Action<HackNode> OnNodePointerDown;
    public System.Action<HackNode> OnNodePointerEnter;
    public System.Action OnNodePointerUp;
    private HackLevel currentLevel;

    private readonly Dictionary<HackNode, NodeVisual> visuals = new Dictionary<HackNode, NodeVisual>();
    private readonly List<Image> lines = new List<Image>();
    private readonly HashSet<string> createdLinePairs = new HashSet<string>();
    private bool isDragging;

    private void Awake()
    {
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i;

            numberButtons[i].onClick.AddListener(() =>
            {
                OnPasswordButtonPressed(number);
            });
        }

        clearButton.onClick.AddListener(() =>
        {
            OnPasswordButtonPressed(-1);
        });

        enterButton.onClick.AddListener(CheckPassword);
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (nodeContainer == null)
        {
            var containerObject = new GameObject("NodeContainer");
            containerObject.transform.SetParent(transform, false);
            nodeContainer = containerObject.AddComponent<RectTransform>();
        }

        if (nodePrefab == null)
        {
            var nodeObject = new GameObject("NodeVisual");
            var image = nodeObject.AddComponent<Image>();
            image.color = normalColor;
            nodeObject.AddComponent<CanvasGroup>();
            nodePrefab = nodeObject;
        }

        InitializeFailureOverlay();
        gameObject.SetActive(false);
    }


    private void CreatePasswordButton(Transform parent, string label, int digit)
    {
        var buttonObject = new GameObject(label + "Button", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(parent, false);

        var button = buttonObject.GetComponent<Button>();
        var buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(0.95f, 0.95f, 0.95f, 1f);

        var colors = button.colors;
        colors.normalColor = new Color(0.95f, 0.95f, 0.95f, 1f);
        colors.highlightedColor = new Color(0.8f, 0.85f, 1f, 1f);
        colors.pressedColor = new Color(0.6f, 0.7f, 1f, 1f);
        button.colors = colors;

        var textObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        textObject.transform.SetParent(buttonObject.transform, false);
        var buttonText = textObject.GetComponent<Text>();
        buttonText.text = label;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 22;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = new Color(0.1f, 0.1f, 0.1f, 1f);

        var textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        button.onClick.AddListener(() => OnPasswordButtonPressed(digit));
    }

    private void OnPasswordButtonPressed(int digit)
    {
        if (digit < 0)
        {
            enteredPassword = "";
        }
        else if (enteredPassword.Length < passwordLength)
        {
            enteredPassword += digit;
        }

        UpdatePasswordDisplay();
    }
    private void CheckPassword()
    {
        if (enteredPassword == correctPassword)
        {
            hackManager.CompleteHack();
        }
        else
        {
            enteredPassword = "";
            UpdatePasswordDisplay();

            hackManager.FailHack();
        }
    }

    private void UpdatePasswordDisplay()
    {
        passwordDisplayText.text =
            new string('*', enteredPassword.Length)
            .PadRight(passwordLength, '_');
    }

    private void ResetPasswordInput()
    {
        enteredPassword = "";
        UpdatePasswordDisplay();
    }

    private bool IsPasswordPuzzleActive()
    {
        return currentLevel != null &&
               currentLevel.PuzzleType == HackPuzzleType.Password;
    }

    private void SetPuzzleVisibility()
    {
        var showPassword = IsPasswordPuzzleActive();

        if (passwordPanel != null)
        {
            passwordPanel.SetActive(showPassword);
        }

        if (nodeContainer != null)
        {
            nodeContainer.gameObject.SetActive(!showPassword);
        }
    }

    private void Update()
    {
        if (hackManager != null && hackManager.IsActive && Input.GetKeyDown(KeyCode.Escape))
        {
            hackManager.FailHack();
            return;
        }

        if (lines.Count == 0 || hackManager == null || !hackManager.IsActive)
        {
            return;
        }

        float pulse = 0.5f + Mathf.Sin(Time.time * 2f) * 0.2f;
        foreach (var line in lines)
        {
            if (line != null)
            {
                var color = line.color;
                color.a = pulse;
                line.color = color;
            }
        }
    }

    public void Open(HackLevel level, List<HackNode> nodes, HackManager manager)
    {
        currentLevel = level;
        hackManager = manager;
        if (level.PuzzleType == HackPuzzleType.Password)
        {
            passwordPanel.SetActive(true);
            nodeContainer.gameObject.SetActive(false);
            timerText.text = null;
        }
        else
        {
            passwordPanel.SetActive(false);
            nodeContainer.gameObject.SetActive(true);

            BuildFromNodes(nodes);
        }

        ResetPasswordInput();

        SetPuzzleVisibility();

        gameObject.SetActive(true);

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        UpdateVisuals();
    }

    // Overload to open canvas using runtime node instances (clones).


    public void Close()
    {
        gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        isDragging = false;
        ResetPasswordInput();
        if (overlayImage != null)
        {
            overlayImage.color = new Color(failureOverlayColor.r, failureOverlayColor.g, failureOverlayColor.b, 0f);
        }
    }

    public void ShowFailureEffect()
    {
        ShowOverlayEffect(failureOverlayColor, closeAfterFinish: true);
    }

    public void ShowSuccessEffect()
    {
        ShowOverlayEffect(successOverlayColor, closeAfterFinish: true);
    }

    private void ShowOverlayEffect(Color overlayColor, bool closeAfterFinish)
    {
        if (overlayImage == null)
        {
            InitializeFailureOverlay();
        }

        if (overlayImage == null)
        {
            Close();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(OverlayRoutine(overlayColor, closeAfterFinish));
    }

    public void FlashNode(HackNode node, float duration)
    {
        if (node == null || !visuals.TryGetValue(node, out var visual))
        {
            return;
        }

        StopAllCoroutines();
        StartCoroutine(FlashRoutine(visual, duration));
    }

    private System.Collections.IEnumerator FlashRoutine(NodeVisual visual, float duration)
    {
        if (visual == null)
        {
            yield break;
        }

        var originalColor = visual.Image.color;
        visual.Image.color = invalidColor;
        yield return new WaitForSeconds(duration);
        visual.Image.color = originalColor;
    }

    private void Build(HackLevel level)
    {
        BuildFromNodes(level != null ? level.Nodes : null);
    }

    private void BuildFromNodes(List<HackNode> nodes)
    {
        ClearVisuals();

        if (nodes == null)
            return;

        foreach (var node in nodes)
        {
            if (node == null)
                continue;

            var visualObject = Instantiate(GetPrefabForNode(node), nodeContainer, false);
            var visual = visualObject.AddComponent<NodeVisual>();
            visual.Initialize(this, node);
            var image = visualObject.GetComponent<Image>();
            image.color = GetBaseColor(node);
            image.sprite = GetSpriteForNode(node);
            image.type = Image.Type.Simple;
            visual.Image = image;
            visuals[node] = visual;
            PositionNode(visual, node);
        }

        foreach (var node in nodes)
        {
            if (node == null)
                continue;

            if (!visuals.TryGetValue(node, out var sourceVisual))
                continue;

            foreach (var neighbor in node.Neighbors)
            {
                if (neighbor == null || !visuals.TryGetValue(neighbor, out var targetVisual))
                    continue;

                CreateLine(sourceVisual, targetVisual);
            }
        }
    }

    private void CreateLine(NodeVisual source, NodeVisual target)
    {
        // Avoid duplicate line between pair by checking instance ids
        int a = source.Node.GetInstanceID();
        int b = target.Node.GetInstanceID();
        int min = Mathf.Min(a, b);
        int max = Mathf.Max(a, b);
        string pairKey = $"{min}_{max}";

        if (createdLinePairs.Contains(pairKey))
            return;

        createdLinePairs.Add(pairKey);

        var lineObject = new GameObject("Line", typeof(RectTransform), typeof(Image));
        lineObject.transform.SetParent(nodeContainer, false);
        var lineImage = lineObject.GetComponent<Image>();
        lineImage.color = new Color(1f, 1f, 1f, 0.35f);
        var rect = lineObject.GetComponent<RectTransform>();
        rect.SetAsFirstSibling();

        var from = source.RectTransform.anchoredPosition;
        var to = target.RectTransform.anchoredPosition;
        var diff = to - from;
        var distance = diff.magnitude;
        rect.sizeDelta = new Vector2(distance, 4f);
        rect.anchoredPosition = from + diff / 2f;
        rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        lines.Add(lineImage);
    }

    private void PositionNode(NodeVisual visual, HackNode node)
    {
        if (visual == null || node == null)
        {
            return;
        }
        // Use the serialized UI position on the node so nodes live fully in the canvas.
        visual.RectTransform.anchoredPosition = node.UIPosition;
    }

    public void UpdateVisuals()
    {
        if (hackManager == null)
        {
            return;
        }

        if (IsPasswordPuzzleActive())
        {
            return;
        }

        foreach (var entry in visuals)
        {
            var node = entry.Key;
            var visual = entry.Value;
            if (node == null || visual == null)
            {
                continue;
            }

            visual.Image.color = GetVisualColor(node);
            visual.Image.sprite = GetSpriteForNode(node);
            // show a white outline for the current node
            visual.SetOutline(node == hackManager.CurrentNode);

            // visited nodes get a gentle pulse; current node is emphasized
            if (node == hackManager.CurrentNode)
            {
                visual.RectTransform.localScale = Vector3.one * 1.15f;
            }
            else if (node.IsVisited)
            {
                float t = (Mathf.Sin(Time.time * 6f) + 1f) * 0.5f; // 0..1
                float s = Mathf.Lerp(0.95f, 1.05f, t);
                visual.RectTransform.localScale = Vector3.one * s;
            }
            else
            {
                visual.RectTransform.localScale = Vector3.one;
            }
        }

        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(hackManager.RemainingTime)}s";
        }
    }

    private GameObject GetPrefabForNode(HackNode node)
    {
        if (node == null)
        {
            return nodePrefab;
        }

        return node.NodeType switch
        {
            HackNodeType.Start => startNodePrefab != null ? startNodePrefab : nodePrefab,
            HackNodeType.End => endNodePrefab != null ? endNodePrefab : nodePrefab,
            HackNodeType.Virus => virusNodePrefab != null ? virusNodePrefab : nodePrefab,
            HackNodeType.Firewall => firewallNodePrefab != null ? firewallNodePrefab : nodePrefab,
            HackNodeType.Key => keyNodePrefab != null ? keyNodePrefab : nodePrefab,
            HackNodeType.Bonus => bonusNodePrefab != null ? bonusNodePrefab : nodePrefab,
            HackNodeType.Teleport => teleportNodePrefab != null ? teleportNodePrefab : nodePrefab,
            _ => nodePrefab
        };
    }

    private Sprite GetSpriteForNode(HackNode node)
    {
        if (node == null)
        {
            return GetOrCreateSprite(HackNodeType.Normal);
        }

        if (generatedSprites.TryGetValue(node.NodeType, out var sprite) && sprite != null)
        {
            return sprite;
        }

        return GetOrCreateSprite(node.NodeType);
    }

    private Sprite GetOrCreateSprite(HackNodeType nodeType)
    {
        if (generatedSprites.TryGetValue(nodeType, out var existing) && existing != null)
        {
            return existing;
        }

        var texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        var pixels = new Color32[64 * 64];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(0, 0, 0, 0);
        }

        FillShape(texture, pixels, nodeType);
        texture.SetPixels32(pixels);
        texture.Apply();

        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        generatedSprites[nodeType] = sprite;
        return sprite;
    }

    private void FillShape(Texture2D texture, Color32[] pixels, HackNodeType nodeType)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                bool inside = false;
                switch (nodeType)
                {
                    case HackNodeType.Start:
                        inside = IsDiamond(x, y, texture.width, texture.height);
                        break;
                    case HackNodeType.End:
                        inside = IsStar(x, y, texture.width, texture.height);
                        break;
                    case HackNodeType.Virus:
                        inside = IsHexagon(x, y, texture.width, texture.height);
                        break;
                    case HackNodeType.Firewall:
                        inside = IsShield(x, y, texture.width, texture.height);
                        break;
                    case HackNodeType.Key:
                        inside = IsCircle(x, y, texture.width, texture.height);
                        break;
                    case HackNodeType.Bonus:
                        inside = IsLightning(x, y, texture.width, texture.height);
                        break;
                    case HackNodeType.Teleport:
                        inside = IsRoundedSquare(x, y, texture.width, texture.height);
                        break;
                    default:
                        inside = IsCircle(x, y, texture.width, texture.height);
                        break;
                }

                if (inside)
                {
                    pixels[y * texture.width + x] = new Color32(255, 255, 255, 255);
                }
            }
        }
    }

    private bool IsCircle(int x, int y, int width, int height)
    {
        float cx = width * 0.5f;
        float cy = height * 0.5f;
        float radius = width * 0.28f;
        return (x - cx) * (x - cx) + (y - cy) * (y - cy) <= radius * radius;
    }

    private bool IsDiamond(int x, int y, int width, int height)
    {
        float cx = width * 0.5f;
        float cy = height * 0.5f;
        float dx = Mathf.Abs(x - cx);
        float dy = Mathf.Abs(y - cy);
        return dx + dy <= width * 0.3f;
    }

    private bool IsHexagon(int x, int y, int width, int height)
    {
        float cx = width * 0.5f;
        float cy = height * 0.5f;
        float dx = Mathf.Abs(x - cx);
        float dy = Mathf.Abs(y - cy);
        return dy <= height * 0.24f && dx <= width * 0.32f - 0.5f * dy * 0.8f;
    }

    private bool IsShield(int x, int y, int width, int height)
    {
        float cx = width * 0.5f;
        float cy = height * 0.5f;
        float rx = width * 0.24f;
        float ry = height * 0.3f;
        bool body = (x - cx) * (x - cx) / (rx * rx) + (y - cy) * (y - cy) / (ry * ry) <= 1f;
        bool top = y >= height * 0.18f && y <= height * 0.42f && x >= width * 0.32f && x <= width * 0.68f;
        return body || top;
    }

    private bool IsStar(int x, int y, int width, int height)
    {
        float cx = width * 0.5f;
        float cy = height * 0.5f;
        float dist = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
        float angle = Mathf.Atan2(y - cy, x - cx);
        float spike = 0.6f + 0.2f * Mathf.Sin(5f * angle);
        return dist <= width * 0.22f * spike;
    }

    private bool IsLightning(int x, int y, int width, int height)
    {
        bool top = (x >= width * 0.35f && x <= width * 0.5f && y >= height * 0.1f && y <= height * 0.7f);
        bool middle = (x >= width * 0.28f && x <= width * 0.62f && y >= height * 0.25f && y <= height * 0.85f);
        bool bottom = (x >= width * 0.42f && x <= width * 0.7f && y >= height * 0.15f && y <= height * 0.95f);
        return top || middle || bottom;
    }

    private bool IsRoundedSquare(int x, int y, int width, int height)
    {
        float cx = width * 0.5f;
        float cy = height * 0.5f;
        float rx = width * 0.26f;
        float ry = height * 0.26f;
        return Mathf.Abs(x - cx) <= rx && Mathf.Abs(y - cy) <= ry;
    }

    private Color GetVisualColor(HackNode node)
    {
        if (node == null)
        {
            return normalColor;
        }

        if (hackManager != null && node == hackManager.CurrentNode)
        {
            return activeColor;
        }

        if (node.IsVisited)
        {
            return visitedColor;
        }

        return node.NodeType switch
        {
            HackNodeType.End => endColor,
            HackNodeType.Virus => virusColor,
            HackNodeType.Firewall => firewallColor,
            HackNodeType.Key => new Color(1f, 0.85f, 0.35f, 1f),
            HackNodeType.Bonus => new Color(1f, 0.6f, 0.15f, 1f),
            _ => normalColor
        };
    }

    private Color GetBaseColor(HackNode node)
    {
        if (node == null)
        {
            return normalColor;
        }

        return GetVisualColor(node);
    }

    private void ClearVisuals()
    {
        foreach (var line in lines)
        {
            if (line != null)
            {
                Destroy(line.gameObject);
            }
        }

        lines.Clear();

        createdLinePairs.Clear();

        foreach (var visual in visuals.Values)
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
            }
        }

        visuals.Clear();
    }

    private void InitializeFailureOverlay()
    {
        if (overlayImage != null)
        {
            return;
        }

        var overlayObject = new GameObject("FailureOverlay", typeof(RectTransform), typeof(Image));
        overlayObject.transform.SetParent(transform, false);

        var overlayRect = overlayObject.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.sizeDelta = Vector2.zero;
        overlayRect.anchoredPosition = Vector2.zero;

        var image = overlayObject.GetComponent<Image>();
        image.color = new Color(failureOverlayColor.r, failureOverlayColor.g, failureOverlayColor.b, 0f);
        image.raycastTarget = false;
        overlayImage = image;

        overlayObject.transform.SetAsLastSibling();
    }

    private System.Collections.IEnumerator OverlayRoutine(Color targetColor, bool closeAfterFinish)
    {
        if (overlayImage == null)
        {
            Close();
            yield break;
        }

        float half = failureEffectDuration * 0.5f;
        var transparent = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);

        for (float t = 0f; t < half; t += Time.deltaTime)
        {
            overlayImage.color = Color.Lerp(transparent, targetColor, t / half);
            yield return null;
        }

        overlayImage.color = targetColor;

        yield return new WaitForSeconds(0.2f);

        for (float t = 0f; t < half; t += Time.deltaTime)
        {
            overlayImage.color = Color.Lerp(targetColor, transparent, t / half);
            yield return null;
        }

        overlayImage.color = transparent;

        if (closeAfterFinish)
        {
            Close();
        }
    }

    public void BeginDrag(HackNode node)
    {
        // Deprecated: kept for compatibility but no gameplay logic here.
        OnNodePointerDown?.Invoke(node);
    }

    public void DragOver(HackNode node)
    {
        // Forward pointer enter to the gameplay manager.
        OnNodePointerEnter?.Invoke(node);
    }

    public void EndDrag()
    {
        OnNodePointerUp?.Invoke();
    }

    private sealed class NodeVisual : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
    {
        private HackCanvas owner;
        private HackNode node;

        public RectTransform RectTransform { get; private set; }
        public Image Image { get; set; }
        public HackNode Node => node;

        public void Initialize(HackCanvas canvas, HackNode targetNode)
        {
            owner = canvas;
            node = targetNode;
            RectTransform = GetComponent<RectTransform>();
            RectTransform.sizeDelta = new Vector2(56f, 56f);
            RectTransform.anchoredPosition = Vector2.zero;
            RectTransform.localScale = Vector3.one;
            var image = GetComponent<Image>();
            if (image != null)
            {
                image.sprite = null;
                image.type = Image.Type.Simple;
                image.raycastTarget = true;
            }
            // ensure an Outline component exists for highlighting current node
            var outline = GetComponent<UnityEngine.UI.Outline>();
            if (outline == null)
            {
                outline = gameObject.AddComponent<UnityEngine.UI.Outline>();
            }
            outline.effectColor = Color.clear;
            outline.effectDistance = new Vector2(2f, 2f);
            outline.enabled = false;
        }

        public void SetOutline(bool enabled)
        {
            var outline = GetComponent<UnityEngine.UI.Outline>();
            if (outline == null)
                return;

            outline.effectColor = enabled ? Color.white : Color.clear;
            outline.enabled = enabled;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            owner.OnNodePointerDown?.Invoke(node);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // PointerEnter events don't provide button state reliably, forward anyway.
            owner.OnNodePointerEnter?.Invoke(node);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            owner.OnNodePointerUp?.Invoke();
        }
    }
}
