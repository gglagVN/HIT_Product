using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool useEvents;
    public string promtMessage;

    [SerializeField] private string saveId;

    public string SaveId => saveId;

    public void BaseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {

    }

#if UNITY_EDITOR
    protected virtual void Reset()
    {
        GenerateSaveIdIfEmpty();
    }

    protected virtual void OnValidate()
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
}
