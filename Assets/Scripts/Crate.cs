using UnityEngine;

public class Crate : Interactable
{
    [Header("Hacking")]
    [SerializeField] private HackManager hackManager;
    [SerializeField] private HackLevel hackLevel;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerLook playerLook;
    private Animator targetAnimator;
    private bool isSolved;

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

        if (targetObject != null)
        {
            targetAnimator = targetObject.GetComponent<Animator>();
        }
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
        if (isSolved) return;

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

        SetPlayerControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerLook.SetLookEnabled(true);
        MarkSolved();
    }

    public void MarkSolved()
    {
        if (isSolved)
            return;

        isSolved = true;
        if (targetAnimator != null)
        {
            targetAnimator.SetBool("isOpened", true);
        }

        var save = GetComponent<CrateSaveData>();
        if (save != null)
        {
            save.solved = true;
            save.DataChanged = true;
        }
    }

    private void OnHackFailed()
    {
        if (!IsCurrentHackTarget())
        {
            return;
        }

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
