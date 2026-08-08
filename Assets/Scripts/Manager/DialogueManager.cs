using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Coroutine typingCoroutine;
    private Coroutine currentDialogue;

    private bool isTyping;
    private bool skipTyping;
    private bool waitingForNext;
    private bool isPlaying;

    [SerializeField]
    private float typeSpeed = 0.03f;

    private WaitForSeconds typeDelay;

    [Header("UI")]
    [SerializeField] private CanvasGroup dialogueGroup;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text subtitleText;

    [Header("Audio")]
    [SerializeField] private AudioSource voiceSource;

    [Header("Player Control")]
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private PlayerLook playerLook;

    [Header("Debug")]
    [SerializeField] private DialogueSequence intro;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        typeDelay = new WaitForSeconds(typeSpeed);

        HideDialogue();
    }

    // =========================================================
    // SINGLE LINE
    // =========================================================

    public void Play(DialogueLine line)
    {
        if (line == null)
            return;

        if (currentDialogue != null)
            StopCoroutine(currentDialogue);

        currentDialogue = StartCoroutine(
            PlayRoutine(line, false)
        );
    }

    private IEnumerator PlayRoutine(
        DialogueLine line,
        bool lockPlayer)
    {
        if (lockPlayer)
            LockPlayer();

        isPlaying = true;

        dialogueGroup.alpha = 1f;
        dialogueGroup.blocksRaycasts = false;
        dialogueGroup.interactable = false;

        speakerText.text = line.speakerName;

        // -------------------------
        // TYPE TEXT
        // -------------------------

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(
            TypeText(line.dialogue)
        );

        // -------------------------
        // VOICE
        // -------------------------

        if (voiceSource != null)
        {
            voiceSource.Stop();

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }
        }

        // -------------------------
        // WAIT FOR NEXT
        // -------------------------

        waitingForNext = true;

        while (waitingForNext)
        {
            yield return null;
        }

        isPlaying = false;

        HideDialogue();

        if (lockPlayer)
            UnlockPlayer();

        currentDialogue = null;
    }

    // =========================================================
    // TYPE TEXT
    // =========================================================

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        skipTyping = false;

        subtitleText.text = text;
        subtitleText.maxVisibleCharacters = 0;

        int length = text != null ? text.Length : 0;

        for (int i = 0; i < length; i++)
        {
            if (skipTyping)
                break;

            subtitleText.maxVisibleCharacters = i + 1;

            yield return typeDelay;
        }

        // Luôn hiển thị toàn bộ text
        subtitleText.maxVisibleCharacters = int.MaxValue;

        isTyping = false;
        skipTyping = false;

        typingCoroutine = null;
    }

    // =========================================================
    // PLAY SEQUENCE
    // =========================================================

    public void PlaySequence(
        DialogueSequence sequence,
        bool lockPlayerDuringDialogue = false)
    {
        if (sequence == null)
        {
            Debug.LogWarning("DialogueManager: DialogueSequence bị null.");
            return;
        }

        if (currentDialogue != null)
            StopCoroutine(currentDialogue);

        currentDialogue = StartCoroutine(
            PlaySequenceRoutine(
                sequence,
                lockPlayerDuringDialogue
            )
        );
    }

    private IEnumerator PlaySequenceRoutine(
        DialogueSequence sequence,
        bool lockPlayerDuringDialogue)
    {
        isPlaying = true;

        if (lockPlayerDuringDialogue)
            LockPlayer();

        foreach (DialogueLine line in sequence.lines)
        {
            if (line == null)
                continue;

            yield return StartCoroutine(
                PlayRoutineWithoutLock(line)
            );
        }

        isPlaying = false;

        HideDialogue();

        if (lockPlayerDuringDialogue)
            UnlockPlayer();

        currentDialogue = null;
    }

    // =========================================================
    // PLAY LINE INSIDE SEQUENCE
    // =========================================================

    private IEnumerator PlayRoutineWithoutLock(
        DialogueLine line)
    {
        dialogueGroup.alpha = 1f;
        dialogueGroup.blocksRaycasts = false;
        dialogueGroup.interactable = false;

        speakerText.text = line.speakerName;

        // -------------------------
        // TYPE TEXT
        // -------------------------

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(
            TypeText(line.dialogue)
        );

        // -------------------------
        // VOICE
        // -------------------------

        if (voiceSource != null)
        {
            voiceSource.Stop();

            if (line.voiceClip != null)
            {
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
            }
        }

        // -------------------------
        // WAIT
        // -------------------------

        waitingForNext = true;

        while (waitingForNext)
        {
            yield return null;
        }
    }

    // =========================================================
    // INPUT
    // =========================================================

    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlaySequence(intro, true);
        }

        // NEXT / SKIP
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isPlaying)
                return;

            // --------------------------------
            // TEXT CHƯA HIỆN HẾT
            // --------------------------------

            if (isTyping)
            {
                // Chỉ hiện toàn bộ text.
                // KHÔNG chuyển câu.
                skipTyping = true;

                return;
            }

            // --------------------------------
            // TEXT ĐÃ HIỆN HẾT
            // --------------------------------

            waitingForNext = false;
        }
    }

    // =========================================================
    // LOCK PLAYER
    // =========================================================

    private void LockPlayer()
    {
        if (playerMotor != null)
        {
            playerMotor.SetMovementEnabled(false);
        }

    }

    // =========================================================
    // UNLOCK PLAYER
    // =========================================================

    private void UnlockPlayer()
    {
        if (playerMotor != null)
        {
            playerMotor.SetMovementEnabled(true);
        }

    }

    // =========================================================
    // HIDE
    // =========================================================

    private void HideDialogue()
    {
        dialogueGroup.alpha = 0f;

        speakerText.text = "";
        subtitleText.text = "";

        if (voiceSource != null)
            voiceSource.Stop();

        isTyping = false;
        skipTyping = false;
        waitingForNext = false;
    }
}

