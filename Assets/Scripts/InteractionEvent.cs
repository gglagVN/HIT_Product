using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnInteract;
    public UnityEvent OnHackSuccess;
    public UnityEvent OnHackFail;

    [Header("Trigger Zone")]
    [SerializeField] private bool fireOnPlayerEnter;
    private bool triggerFired;

    private void OnTriggerEnter(Collider other)
    {
        if (!fireOnPlayerEnter || triggerFired)
        {
            return;
        }
        if (!other.CompareTag("Player"))
        {
            return;
        }
        triggerFired = true;
        OnInteract?.Invoke();
    }

    public void InvokeInteract()
    {
        OnInteract?.Invoke();
    }

    public void InvokeHackSuccess()
    {
        OnHackSuccess?.Invoke();
    }

    public void InvokeHackFail()
    {
        OnHackFail?.Invoke();
    }
}