using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueSequence dialogue;

    [Header("Settings")]
    public bool playOnce = true;
    public enum TriggerType
    {
        Sequence,
        SingleLine
    }

    private bool played;
    public void ResetTrigger()
    {
        played = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (playOnce && played)
            return;

        DialogueManager.Instance.PlaySequence(dialogue);

        played = true;
    }
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        if (box == null)
            return;

        Gizmos.color = Color.cyan;

        Matrix4x4 old = Gizmos.matrix;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube(box.center, box.size);

        Gizmos.matrix = old;
    }

}