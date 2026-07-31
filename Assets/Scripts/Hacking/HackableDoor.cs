using UnityEngine;

/// <summary>
/// Example target that reacts to a successful or failed hack attempt.
/// </summary>
public class HackableDoor : MonoBehaviour, IHackable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openParameter = "Open";

    public void OnHackSuccess()
    {
        if (animator != null)
        {
            animator.SetBool(openParameter, true);
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"{name} unlocked by hacking.");
    }

    public void OnHackFailure()
    {
        Debug.Log($"{name} remained locked after a failed hack attempt.");
    }
}
