using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lever : Interactable
{
    [Header("Objects")]
    [SerializeField] private GameObject door;
    [SerializeField] private Animator doorAnimator;

    [Header("Camera")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera doorCam;
    [SerializeField] private float watchDoorTime = 2f;

    [Header("Self Destruct")]
    [SerializeField] private AudioSource selfDestructAudio;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private float flashSpeed = 3f;

    [Header("Player")]
    [SerializeField] private MonoBehaviour playerMovement;
    [SerializeField] private MonoBehaviour playerLook;
    [SerializeField] private CountdownTimer countdown;

    private Animator leverAnimator;

    private bool isUsed;
    private bool selfDestructStarted;

    public bool IsUsed => isUsed;

    private void Awake()
    {
        leverAnimator = GetComponent<Animator>();
        if (doorAnimator == null && door != null)
            doorAnimator = door.GetComponent<Animator>();

        if (selfDestructAudio == null)
            selfDestructAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        selfDestructStarted = false;
        if (globalVolume != null)
            globalVolume.weight = 0f;
    }

    private void Update()
    {
        if (!selfDestructStarted || globalVolume == null)
            return;

        globalVolume.weight = Mathf.PingPong(Time.time * flashSpeed, 1f);
    }

    protected override void Interact()
    {
        if (isUsed)
            return;

        StartCoroutine(OpenDoorSequence());
    }

    public void ForceActivate()
    {
        if (isUsed)
            return;

        isUsed = true;
        if (leverAnimator != null)
            leverAnimator.SetBool("isOpened", true);

        if (doorAnimator != null)
            doorAnimator.SetBool("isOpened", true);
    }

    private IEnumerator OpenDoorSequence()
    {
        isUsed = true;

        // Disable player
        playerMovement.enabled = false;
        playerLook.enabled = false;

        // Lever animation
        leverAnimator.SetBool("isOpened", true);

        yield return new WaitForSeconds(1f);

        // Open door
        OpenDoorImmediate();

        // Start self destruct
        StartSelfDestruct();

        // Door camera
        playerCam.enabled = false;
        doorCam.enabled = true;

        yield return new WaitForSeconds(watchDoorTime);

        // Return player camera
        doorCam.enabled = false;
        playerCam.enabled = true;

        yield return new WaitForSeconds(0.5f);

        // Enable player
        playerMovement.enabled = true;
        playerLook.enabled = true;
    }

    /// <summary>
    /// Đặt cửa về trạng thái đã mở, không kèm hiệu ứng hay chờ đợi.
    /// </summary>
    private void OpenDoorImmediate()
    {
        if (doorAnimator == null && door != null)
            doorAnimator = door.GetComponent<Animator>();

        if (doorAnimator != null)
            doorAnimator.SetBool("isOpened", true);
    }

    /// <summary>
    /// Khôi phục trạng thái cần gạt đã kéo và cửa đã mở, bỏ qua cutscene và đếm ngược.
    /// </summary>
    public void ForceUsed()
    {
        isUsed = true;

        if (leverAnimator == null)
            leverAnimator = GetComponent<Animator>();

        if (leverAnimator != null)
            leverAnimator.SetBool("isOpened", true);

        OpenDoorImmediate();
    }

    private void StartSelfDestruct()
    {
        selfDestructStarted = true;
        countdown.StartCountdown();
        if (selfDestructAudio != null && !selfDestructAudio.isPlaying)
        {
            selfDestructAudio.loop = true;
            selfDestructAudio.Play();
        }
    }

    public void StopSelfDestruct()
    {
        selfDestructStarted = false;

        if (globalVolume != null)
            globalVolume.weight = 0f;

        if (selfDestructAudio != null)
            selfDestructAudio.Stop();
    }
}