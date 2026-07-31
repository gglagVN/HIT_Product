using System.Collections.Generic;
using UnityEngine;

public class HackNode : MonoBehaviour
{
    [SerializeField] private HackNodeType nodeType = HackNodeType.Normal;
    [SerializeField] private List<HackNode> neighbors = new();
    [SerializeField] private bool isVisited;
    [SerializeField] private bool requiresKey;
    [SerializeField] private int keyCost = 1;
    [SerializeField] private HackNode teleportDestination;
    [SerializeField] private float pulseSpeed = 1.5f;
    [Header("UI")]
    [SerializeField] private int id = 0;
    [SerializeField] private Vector2 uiPosition = Vector2.zero;

    public HackNodeType NodeType => nodeType;
    public List<HackNode> Neighbors => neighbors;
    public bool IsVisited => isVisited;
    public bool RequiresKey => requiresKey;
    public int KeyCost => keyCost;
    public HackNode TeleportDestination => teleportDestination;
    public float PulseSpeed => pulseSpeed;
    public int Id => id;
    public Vector2 UIPosition => uiPosition;

    public void SetVisited(bool value) => isVisited = value;

    public void SetNodeType(HackNodeType value) => nodeType = value;
    public void SetTeleportDestination(HackNode dest) => teleportDestination = dest;

    public virtual void ExecuteEffect(HackManager manager)
    {
        switch (nodeType)
        {
            case HackNodeType.Start:
                break;
            case HackNodeType.End:
                manager.CompleteHack();
                break;
            case HackNodeType.Virus:
                manager.FailHack();
                break;
            case HackNodeType.Firewall:
                if (!manager.HasKey())
                {
                    manager.FailHack();
                }
                break;
            case HackNodeType.Key:
                manager.CollectKey(keyCost);
                break;
            case HackNodeType.Bonus:
                manager.AddBonusTime();
                break;
            case HackNodeType.Teleport:
                if (teleportDestination != null)
                {
                    manager.TeleportToNode(teleportDestination);
                }
                else
                {
                    manager.TeleportToRandomNode();
                }
                break;
            default:
                break;
        }
    }
}

public enum HackNodeType
{
    Normal,
    Start,
    End,
    Virus,
    Firewall,
    Key,
    Bonus,
    Teleport
}
