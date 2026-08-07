using System.Collections;
using UnityEngine;

/// <summary>
/// Entry point for interaction with a keypad. It opens the hacking UI and pauses the player.
/// </summary>
public class KeypadInteract : Interactable
{
    [Header("Hacking")]
    [SerializeField] private InteractionEvent interactionEvent;
    [SerializeField] private HackManager hackManager;
    [SerializeField] private HackLevel hackLevel;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerLook playerLook;
    private AudioSource audioSource;
    private Animator targetAnimator;
    private bool isSolved;

    public bool IsSolved => isSolved;

    private void Awake()
    {
        isSolved = false;
        if (hackManager == null)
        {
            hackManager = FindObjectOfType<HackManager>();
        }

        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        if (playerLook == null)
        {
            playerLook = FindObjectOfType<PlayerLook>();
        }
        audioSource = targetObject.GetComponent<AudioSource>();
        targetAnimator = targetObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        var mgr = hackManager != null ? hackManager : HackManager.Instance;
        if (mgr != null)
        {
            mgr.onHackSucceeded.AddListener(OnHackSucceeded);
            mgr.onHackFailed.AddListener(OnHackFailed);
        }
    }

    private void OnDisable()
    {
        var mgr = hackManager != null ? hackManager : HackManager.Instance;
        if (mgr != null)
        {
            mgr.onHackSucceeded.RemoveListener(OnHackSucceeded);
            mgr.onHackFailed.RemoveListener(OnHackFailed);
        }
    }

    protected override void Interact()
    {
        if (hackManager == null || hackLevel == null)
        {
            Debug.LogWarning("HackManager or HackLevel is not assigned.");
            return;
        }

        if (isSolved)
        {
            return;
        }

        if (hackManager.IsActive)
        {
            return;
        }

        SetPlayerControl(false);
        playerLook.SetLookEnabled(false);
        if (hackLevel.PuzzleType == HackPuzzleType.Password)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        interactionEvent?.InvokeInteract();
        hackManager.StartHack(hackLevel, targetObject);
    }

    private bool IsCurrentHackTarget()
    {
        if (hackManager == null)
        {
            return false;
        }

        return hackManager.CurrentTarget == targetObject && hackManager.CurrentLevel == hackLevel;
    }

    private void OnHackSucceeded()
    {
        if (!IsCurrentHackTarget())
        {
            return;
        }
        isSolved = true;
        interactionEvent?.InvokeHackSuccess();

        SetPlayerControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLook.SetLookEnabled(true);
        StartCoroutine("delay");
    }

    /// <summary>
    /// Đánh dấu keypad đã hack xong và mở cửa ngay, không chạy minigame cũng không phát lại hội thoại.
    /// </summary>
    public void ForceSolved()
    {
        isSolved = true;

        if (targetAnimator == null && targetObject != null)
        {
            targetAnimator = targetObject.GetComponent<Animator>();
        }

        if (targetAnimator != null)
        {
            targetAnimator.SetBool("isOpened", true);
        }
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        if (audioSource != null)
            audioSource.Play();
        targetObject.GetComponent<Animator>().SetBool("isOpened", true);
    }

    private void OnHackFailed()
    {
        if (!IsCurrentHackTarget())
        {
            return;
        }
        interactionEvent?.InvokeHackFail();

        SetPlayerControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLook.SetLookEnabled(true);
    }

    private void SetPlayerControl(bool enabled)
    {
        if (inputManager != null)
        {
            inputManager.SetPlayerControlsEnabled(enabled);
        }
    }
}
