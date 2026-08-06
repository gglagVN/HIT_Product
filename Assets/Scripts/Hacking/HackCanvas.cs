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
    [SerializeField] private GameObject activeNodePrefab;
    [SerializeField] private GameObject visitedNodePrefab;
    [SerializeField] private TextMeshProUGUI timerText;

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
    private readonly Dictionary<string, Image> lineImagesByPair = new Dictionary<string, Image>();
    private readonly HashSet<string> createdLinePairs = new HashSet<string>();
    private readonly HashSet<string> traversedLinePairs = new HashSet<string>();
    private readonly List<HackNode> nodesToRefresh = new List<HackNode>();
    private bool lineColorsDirty = true;
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
            string.Join(" ", new string('*', enteredPassword.Length)
            .PadRight(passwordLength, '_').ToCharArray());
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
        if (hackManager != null && hackManager.IsActive && Input.GetKeyDown(KeyCode.Tab))
        {
            hackManager.FailHack();
            return;
        }

        if (lines.Count == 0 || hackManager == null || !hackManager.IsActive)
        {
            return;
        }

        float pulse = 0.5f + Mathf.Sin(Time.time * 2f) * 0.2f;
        foreach (var entry in lineImagesByPair)
        {
            var line = entry.Value;
            if (line == null || traversedLinePairs.Contains(entry.Key))
            {
                continue;
            }

            var color = line.color;
            color.a = pulse;
            line.color = color;
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

    public void MarkPathTraversal(HackNode from, HackNode to)
    {
        if (from == null || to == null)
        {
            return;
        }

        int a = from.GetInstanceID();
        int b = to.GetInstanceID();
        int min = Mathf.Min(a, b);
        int max = Mathf.Max(a, b);
        string pairKey = $"{min}_{max}";

        traversedLinePairs.Add(pairKey);
        lineColorsDirty = true;

        if (lineImagesByPair.TryGetValue(pairKey, out var lineImage) && lineImage != null)
        {
            lineImage.color = GetLineColorForPair(pairKey);
            return;
        }

        RebuildLines();
        if (lineImagesByPair.TryGetValue(pairKey, out var rebuiltLine) && rebuiltLine != null)
        {
            rebuiltLine.color = GetLineColorForPair(pairKey);
        }
    }

    private System.Collections.IEnumerator FlashRoutine(NodeVisual visual, float duration)
    {
        if (visual == null || visual.Image == null)
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

            var visualObject = Instantiate(GetPrefabForNode(node, NodeVisualState.Normal), nodeContainer, false);
            var visual = visualObject.AddComponent<NodeVisual>();
            visual.Initialize(this, node);
            visual.CurrentState = NodeVisualState.Normal;
            visual.SetOutline(false);
            visuals[node] = visual;
            PositionNode(visual, node);
        }

        RebuildLines();
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
        lineImage.color = GetLineColor(source, target);
        lineImage.raycastTarget = false;
        lineImage.maskable = false;
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
        lineImagesByPair[pairKey] = lineImage;
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

    private Color GetLineColor(NodeVisual source, NodeVisual target)
    {
        if (source == null || target == null)
        {
            return new Color(1f, 1f, 1f, 0.35f);
        }

        int a = source.Node != null ? source.Node.GetInstanceID() : -1;
        int b = target.Node != null ? target.Node.GetInstanceID() : -1;
        if (a < 0 || b < 0)
        {
            return new Color(1f, 1f, 1f, 0.35f);
        }

        int min = Mathf.Min(a, b);
        int max = Mathf.Max(a, b);
        return GetLineColorForPair($"{min}_{max}");
    }

    private Color GetLineColorForPair(string pairKey)
    {
        if (traversedLinePairs.Contains(pairKey))
        {
            return new Color(1f, 0.06f, 0.06f, 1f);
        }

        return new Color(0.8f, 0.85f, 1f, 0.45f);
    }

    private void RefreshLineColors()
    {
        foreach (var entry in lineImagesByPair)
        {
            if (entry.Value == null)
            {
                continue;
            }

            entry.Value.color = GetLineColorForPair(entry.Key);
        }
    }

    public void UpdateVisuals()
    {
        if (hackManager == null)
        {
            return;
        }

        if (lineColorsDirty)
        {
            RefreshLineColors();
            lineColorsDirty = false;
        }

        if (IsPasswordPuzzleActive())
        {
            return;
        }

        nodesToRefresh.Clear();
        foreach (var entry in visuals)
        {
            var node = entry.Key;
            var visual = entry.Value;
            if (node == null || visual == null)
            {
                continue;
            }

            var desiredState = GetVisualState(node);
            if (visual.CurrentState != desiredState)
            {
                nodesToRefresh.Add(node);
                continue;
            }

            bool isCurrent = node == hackManager.CurrentNode;
            visual.SetOutline(isCurrent);

            float scale;
            if (isCurrent)
            {
                scale = 1.15f;
            }
            else if (node.IsVisited)
            {
                float t = (Mathf.Sin(Time.time * 6f) + 1f) * 0.5f;
                scale = Mathf.Lerp(0.95f, 1.05f, t);
            }
            else
            {
                scale = 1f;
            }

            var targetScale = Vector3.one * scale;
            if (visual.RectTransform.localScale != targetScale)
            {
                visual.RectTransform.localScale = targetScale;
            }
        }

        foreach (var node in nodesToRefresh)
        {
            if (visuals.TryGetValue(node, out var currentVisual))
            {
                ReplaceVisual(node, currentVisual, GetVisualState(node));
            }
        }

        if (timerText != null)
        {
            int totalSeconds = Mathf.CeilToInt(hackManager.RemainingTime);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    private GameObject GetPrefabForNode(HackNode node, NodeVisualState state)
    {
        if (node == null)
        {
            return nodePrefab;
        }

        GameObject basePrefab = node.NodeType switch
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

        if (state == NodeVisualState.Active && activeNodePrefab != null)
        {
            return activeNodePrefab;
        }

        if (state == NodeVisualState.Visited && visitedNodePrefab != null)
        {
            return visitedNodePrefab;
        }

        return basePrefab != null ? basePrefab : nodePrefab;
    }

    private NodeVisualState GetVisualState(HackNode node)
    {
        if (node == null)
        {
            return NodeVisualState.Normal;
        }

        if (hackManager != null && node == hackManager.CurrentNode)
        {
            return NodeVisualState.Active;
        }

        return node.IsVisited ? NodeVisualState.Visited : NodeVisualState.Normal;
    }

    private void ReplaceVisual(HackNode node, NodeVisual currentVisual, NodeVisualState desiredState)
    {
        if (node == null || currentVisual == null)
        {
            return;
        }

        var oldRect = currentVisual.RectTransform;
        var position = oldRect != null ? oldRect.anchoredPosition : node.UIPosition;
        var scale = oldRect != null ? oldRect.localScale : Vector3.one;

        var newVisualObject = Instantiate(GetPrefabForNode(node, desiredState), nodeContainer, false);
        var newVisual = newVisualObject.AddComponent<NodeVisual>();
        newVisual.Initialize(this, node);
        newVisual.CurrentState = desiredState;
        newVisual.RectTransform.anchoredPosition = position;
        newVisual.RectTransform.localScale = scale;
        newVisual.SetOutline(node == hackManager?.CurrentNode);

        visuals[node] = newVisual;
        Destroy(currentVisual.gameObject);
        RebuildLines();
    }

    private void RebuildLines()
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
        lineImagesByPair.Clear();
        lineColorsDirty = true;

        foreach (var node in visuals.Keys)
        {
            if (node == null || !visuals.TryGetValue(node, out var sourceVisual))
            {
                continue;
            }

            foreach (var neighbor in node.Neighbors)
            {
                if (neighbor == null || !visuals.TryGetValue(neighbor, out var targetVisual))
                {
                    continue;
                }

                CreateLine(sourceVisual, targetVisual);
            }
        }
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
        lineImagesByPair.Clear();

        createdLinePairs.Clear();

        foreach (var visual in visuals.Values)
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
            }
        }

        visuals.Clear();
        traversedLinePairs.Clear();
        lineColorsDirty = true;
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

    private enum NodeVisualState
    {
        Normal,
        Visited,
        Active
    }

    private sealed class NodeVisual : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
    {
        private HackCanvas owner;
        private HackNode node;
        private UnityEngine.UI.Outline outline;
        private bool outlineEnabled;

        public RectTransform RectTransform { get; private set; }
        public Image Image { get; set; }
        public HackNode Node => node;
        public NodeVisualState CurrentState { get; set; }

        public void Initialize(HackCanvas canvas, HackNode targetNode)
        {
            owner = canvas;
            node = targetNode;
            RectTransform = GetComponent<RectTransform>();
            if (RectTransform == null)
            {
                RectTransform = gameObject.AddComponent<RectTransform>();
            }

            RectTransform.sizeDelta = new Vector2(56f, 56f);
            RectTransform.anchoredPosition = Vector2.zero;
            RectTransform.localScale = Vector3.one;

            var image = GetComponent<Image>();
            if (image != null)
            {
                image.type = Image.Type.Simple;
                image.raycastTarget = true;
            }

            Image = image;

            outline = GetComponent<UnityEngine.UI.Outline>();
            if (outline == null)
            {
                outline = gameObject.AddComponent<UnityEngine.UI.Outline>();
            }
            outline.effectColor = Color.clear;
            outline.effectDistance = new Vector2(2f, 2f);
            outline.enabled = false;
            outlineEnabled = false;
        }

        public void SetOutline(bool enabled)
        {
            if (outline == null)
                return;

            if (outlineEnabled == enabled)
                return;

            outlineEnabled = enabled;
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
            if (owner == null || node == null)
            {
                return;
            }

            owner.OnNodePointerEnter?.Invoke(node);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            owner.OnNodePointerUp?.Invoke();
        }
    }
}
