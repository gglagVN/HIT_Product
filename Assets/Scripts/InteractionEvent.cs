using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnInteract;
    public UnityEvent OnHackSuccess;
    public UnityEvent OnHackFail;

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