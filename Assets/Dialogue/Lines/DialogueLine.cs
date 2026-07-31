using UnityEngine;

[CreateAssetMenu(fileName = "DialogueLine", menuName = "Dialogue/Dialogue Line")]
public class DialogueLine : ScriptableObject
{
    [Header("Speaker")]
    public string speakerName;

    [TextArea(2, 5)]
    public string dialogue;

    [Header("Voice")]
    public AudioClip voiceClip;

    [Header("Settings")]
    public float duration = 3f;
}