using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Central controller for the hacking minigame. It owns puzzle state, validates movement,
/// handles the timer and resolves success or failure.
/// </summary>
public class HackManager : MonoBehaviour
{
    public static HackManager Instance { get; private set; }
    [Header("Puzzle")]
    [SerializeField] private HackLevel currentLevel;
    [SerializeField] private float timerDuration = 20f;
    [SerializeField] private float bonusTimeAmount = 5f;

    [Header("Visuals")]
    [SerializeField] private float invalidFlashDuration = 0.25f;
    [SerializeField] private HackCanvas hackCanvas;

    [Header("Events")]
    public UnityEvent onHackStarted;
    public UnityEvent onHackSucceeded;
    public UnityEvent onHackFailed;

    private HackNode currentNode;
    private int collectedKeys;
    private float remainingTime;
    private bool isActive;
    private bool isComplete;
    private bool isDragging;
    private GameObject targetObjectReference;
    // Runtime clones of level node prefabs. We instantiate these at StartHack
    // so that editing node state (visited/type) doesn't modify the prefab assets.
    private List<HackNode> runtimeNodes = new List<HackNode>();
    private HackNode runtimeStartNode;
    private GameObject runtimeContainer;

    public HackNode CurrentNode => currentNode;
    public HackLevel CurrentLevel => currentLevel;
    public GameObject CurrentTarget => targetObjectReference;
    public bool IsActive => isActive;
    public float RemainingTime => remainingTime;
    public bool HasKey() => collectedKeys >= GetRequiredKeyAmount();
    public MonoBehaviour playerMovement;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        HandleKeyboardNavigation();

        if (hackCanvas != null)
        {
            hackCanvas.UpdateVisuals();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // auto-locate canvas if not assigned
            if (hackCanvas == null)
            {
                hackCanvas = FindObjectOfType<HackCanvas>(true);
            }

            if (hackCanvas != null)
            {
                hackCanvas.OnNodePointerDown += HandlePointerDown;
                hackCanvas.OnNodePointerEnter += HandlePointerEnter;
                hackCanvas.OnNodePointerUp += HandlePointerUp;
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartHack(HackLevel level, GameObject targetObject)
    {
        // prepare runtime copies of level nodes so we don't mutate prefab assets
        CleanupRuntimeNodes();
        currentLevel = level;
        CreateRuntimeNodes(level);

        currentNode = null;
        collectedKeys = 0;
        remainingTime = level != null ? level.TimeLimit : timerDuration;
        isActive = true;
        isComplete = false;
        targetObjectReference = targetObject;
        onHackStarted?.Invoke();
        playerMovement.enabled = false;

        // set start node from runtime clones rather than the asset-level start
        if (runtimeStartNode != null)
        {
            SetCurrentNode(runtimeStartNode);
        }

        if (hackCanvas != null)
        {
            hackCanvas.Open(level, runtimeNodes, this);
        }

        StopAllCoroutines();
        StartCoroutine(TimerRoutine());
    }

    public void SetCurrentNode(HackNode node)
    {
        if (currentNode != null && node != null && currentNode != node && hackCanvas != null)
        {
            hackCanvas.MarkPathTraversal(currentNode, node);
        }

        currentNode = node;
        if (currentNode != null)
        {
            currentNode.SetVisited(true);
        }
    }

    public bool TryMoveTo(HackNode nextNode)
    {
        if (!isActive || currentNode == null || nextNode == null)
        {
            return false;
        }

        if (nextNode.NodeType == HackNodeType.End && !AllRequiredNodesVisited())
        {
            if (hackCanvas != null)
            {
                hackCanvas.FlashNode(nextNode, invalidFlashDuration);
            }
            return false;
        }

        if (currentNode.Neighbors.Contains(nextNode) && !nextNode.IsVisited)
        {
            SetCurrentNode(nextNode);
            nextNode.ExecuteEffect(this);
            return true;
        }

        if (hackCanvas != null)
        {
            hackCanvas.FlashNode(nextNode, invalidFlashDuration);
        }
        return false;
    }

    // Pointer event handlers forwarded by HackCanvas (UI-only).
    private void HandlePointerDown(HackNode node)
    {
        if (!isActive || node == null)
            return;

        // only start dragging if the player clicks the current node (start)
        if (node == currentNode)
        {
            isDragging = true;
        }
        else
        {
            // visual feedback for invalid start
            if (hackCanvas != null)
                hackCanvas.FlashNode(node, invalidFlashDuration);
        }
    }

    private void HandlePointerEnter(HackNode node)
    {
        if (!isActive || !isDragging || node == null)
            return;

        // delegate movement validation to TryMoveTo
        TryMoveTo(node);
    }

    private void HandlePointerUp()
    {
        isDragging = false;
    }

    public void CompleteHack()
    {
        if (!isActive || isComplete)
        {
            return;
        }
        playerMovement.enabled = true;
        isActive = false;
        isComplete = true;
        onHackSucceeded?.Invoke();

        if (hackCanvas != null)
        {
            hackCanvas.ShowSuccessEffect();
        }

        if (targetObjectReference != null)
        {
            var hackable = targetObjectReference.GetComponent<IHackable>();
            hackable?.OnHackSuccess();
        }

        if (runtimeContainer != null)
        {
            CleanupRuntimeNodes();
        }

        StopAllCoroutines();
    }

    public void FailHack()
    {
        if (!isActive || isComplete)
        {
            return;
        }
        playerMovement.enabled = true;

        isActive = false;
        isComplete = true;
        onHackFailed?.Invoke();

        if (targetObjectReference != null)
        {
            var hackable = targetObjectReference.GetComponent<IHackable>();
            hackable?.OnHackFailure();
        }

        if (hackCanvas != null)
        {
            hackCanvas.ShowFailureEffect();
        }

        if (runtimeContainer != null)
        {
            CleanupRuntimeNodes();
        }

        StopAllCoroutines();
    }

    public void CollectKey(int amount)
    {
        collectedKeys += amount;

        if (collectedKeys >= GetRequiredKeyAmount())
        {
            UnlockFirewallNodes();
        }
    }

    public void AddBonusTime()
    {
        remainingTime += bonusTimeAmount;
    }

    public void TeleportToRandomNode()
    {
        var nodes = runtimeNodes != null && runtimeNodes.Count > 0 ? runtimeNodes : (currentLevel != null ? currentLevel.Nodes : null);
        if (nodes == null || nodes.Count == 0)
        {
            return;
        }

        var available = new List<HackNode>();
        foreach (var node in nodes)
        {
            if (node != null && !node.IsVisited)
            {
                available.Add(node);
            }
        }

        if (available.Count > 0)
        {
            var target = available[Random.Range(0, available.Count)];
            SetCurrentNode(target);
        }
    }

    public void TeleportToNode(HackNode destination)
    {
        if (!isActive || destination == null || currentLevel == null)
        {
            return;
        }

        // ensure destination is one of our runtime nodes
        if (!runtimeNodes.Contains(destination))
        {
            return;
        }

        SetCurrentNode(destination);
    }

    private void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            TryMoveInDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            TryMoveInDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            TryMoveInDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            TryMoveInDirection(Vector2.right);
        }
    }

    private void TryMoveInDirection(Vector2 direction)
    {
        if (currentNode == null || currentNode.Neighbors == null || direction == Vector2.zero)
        {
            return;
        }

        HackNode bestNode = null;
        float bestScore = 0.5f;
        Vector2 from = currentNode.UIPosition;

        foreach (var neighbor in currentNode.Neighbors)
        {
            if (neighbor == null || neighbor.IsVisited)
            {
                continue;
            }

            Vector2 delta = neighbor.UIPosition - from;
            if (delta.sqrMagnitude < 0.0001f)
            {
                continue;
            }

            float score = Vector2.Dot(delta.normalized, direction);
            if (score > bestScore)
            {
                bestScore = score;
                bestNode = neighbor;
            }
        }

        if (bestNode != null)
        {
            TryMoveTo(bestNode);
        }
        else if (hackCanvas != null)
        {
            hackCanvas.FlashNode(currentNode, invalidFlashDuration);
        }
    }

    private int GetRequiredKeyAmount()
    {
        var nodes = runtimeNodes != null && runtimeNodes.Count > 0 ? runtimeNodes : (currentLevel != null ? currentLevel.Nodes : null);
        if (nodes == null)
            return 0;

        int total = 0;
        foreach (var node in nodes)
        {
            if (node != null && node.NodeType == HackNodeType.Key)
            {
                total += node.KeyCost;
            }
        }

        return total;
    }

    private void UnlockFirewallNodes()
    {
        var nodes = runtimeNodes != null && runtimeNodes.Count > 0 ? runtimeNodes : (currentLevel != null ? currentLevel.Nodes : null);
        if (nodes == null)
            return;

        foreach (var node in nodes)
        {
            if (node != null && node.NodeType == HackNodeType.Firewall)
            {
                node.SetNodeType(HackNodeType.Normal);
            }
        }
    }

    private bool AllRequiredNodesVisited()
    {
        var nodes = runtimeNodes != null && runtimeNodes.Count > 0 ? runtimeNodes : (currentLevel != null ? currentLevel.Nodes : null);
        if (nodes == null)
            return false;

        foreach (var node in nodes)
        {
            if (node == null || node.NodeType == HackNodeType.Virus || node.NodeType == HackNodeType.End)
            {
                continue;
            }

            if (!node.IsVisited)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator TimerRoutine()
    {
        while (isActive && remainingTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        if (isActive)
        {
            FailHack();
        }
    }

    private void CreateRuntimeNodes(HackLevel level)
    {
        runtimeNodes.Clear();
        runtimeStartNode = null;

        if (level == null || level.Nodes == null)
            return;

        // container to keep runtime clones tidy
        CleanupRuntimeNodes();
        runtimeContainer = new GameObject("HackRuntimeNodes");
        runtimeContainer.transform.SetParent(transform, false);

        var map = new Dictionary<HackNode, HackNode>();

        // first pass: instantiate clones
        foreach (var src in level.Nodes)
        {
            if (src == null)
                continue;

            var go = Instantiate(src.gameObject, runtimeContainer.transform, false);
            go.name = src.gameObject.name + "_runtime";
            // hide from hierarchy to avoid clutter
            go.hideFlags = HideFlags.HideInHierarchy;
            var clone = go.GetComponent<HackNode>();
            if (clone == null)
            {
                Destroy(go);
                continue;
            }

            clone.SetVisited(false);
            map[src] = clone;
            runtimeNodes.Add(clone);
        }

        // second pass: remap neighbors
        foreach (var src in level.Nodes)
        {
            if (src == null)
                continue;

            if (!map.TryGetValue(src, out var clone))
                continue;

            clone.Neighbors.Clear();
            foreach (var n in src.Neighbors)
            {
                if (n != null && map.TryGetValue(n, out var nn))
                {
                    clone.Neighbors.Add(nn);
                }
            }
            // remap teleport destination if present
            if (src.TeleportDestination != null && map.TryGetValue(src.TeleportDestination, out var td))
            {
                clone.SetTeleportDestination(td);
            }
        }

        // determine runtime start node
        if (level.StartNode != null && map.TryGetValue(level.StartNode, out var runtimeStart))
        {
            runtimeStartNode = runtimeStart;
        }
    }

    private void CleanupRuntimeNodes()
    {
        if (runtimeContainer != null)
        {
            // un-hide so we can destroy in editor safely
            runtimeContainer.hideFlags = HideFlags.None;
            Destroy(runtimeContainer);
            runtimeContainer = null;
        }

        runtimeNodes.Clear();
        runtimeStartNode = null;
    }
}
