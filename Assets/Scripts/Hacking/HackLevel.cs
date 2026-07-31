using System.Collections.Generic;
using UnityEngine;

public enum HackPuzzleType
{
    Graph,
    Password
}

/// <summary>
/// Scriptable asset representing a hack puzzle. Author one asset per keypad.
/// </summary>
[CreateAssetMenu(menuName = "Hacking/Hack Level")]
public class HackLevel : ScriptableObject
{
    [SerializeField] private string levelName = "Default";
    [SerializeField] private float timeLimit = 20f;
    [SerializeField] private List<HackNode> nodes = new();
    [SerializeField] private HackNode startNode;
    [SerializeField] private HackNode endNode;
    [SerializeField] private HackPuzzleType puzzleType = HackPuzzleType.Graph;

    public string LevelName => levelName;
    public float TimeLimit => timeLimit;
    public List<HackNode> Nodes => nodes;
    public HackNode StartNode => startNode;
    public HackNode EndNode => endNode;
    public HackPuzzleType PuzzleType => puzzleType;
    [Header("Password")]
    [SerializeField] private string correctPassword = "210125";
    [SerializeField] private int passwordLength = 6;
    public string CorrectPassword => correctPassword;
    public int PasswordLength => passwordLength;
    public void Initialize(HackManager manager)
    {
        foreach (var node in nodes)
        {
            if (node != null)
            {
                node.SetVisited(false);
            }
        }

        if (startNode != null)
        {
            manager.SetCurrentNode(startNode);
        }
    }
}
