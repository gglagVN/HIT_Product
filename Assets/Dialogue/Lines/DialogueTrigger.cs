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

    [SerializeField] private string saveId;

    private bool played;

    public string SaveId => saveId;

    public bool Played => played;

    public void ResetTrigger()
    {
        played = false;
    }

    /// <summary>
    /// Đánh dấu đoạn thoại này đã nghe rồi để khi nạp save không phát lại.
    /// </summary>
    public void ForcePlayed()
    {
        played = true;
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
#if UNITY_EDITOR
    private void Reset()
    {
        GenerateSaveIdIfEmpty();
    }

    private void OnValidate()
    {
        GenerateSaveIdIfEmpty();
    }

    /// <summary>
    /// Sinh id duy nhất cho object trong Editor khi id còn trống.
    /// </summary>
    private void GenerateSaveIdIfEmpty()
    {
        if (Application.isPlaying)
            return;

        if (!string.IsNullOrEmpty(saveId))
            return;

        saveId = System.Guid.NewGuid().ToString("N");
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

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