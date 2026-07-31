using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private Coroutine typingCoroutine;

    private bool isTyping;
    private bool skipTyping;
    private bool waitingForNext;
    [SerializeField]
    private float typeSpeed = 0.03f;

    [Header("UI")]
    public CanvasGroup dialogueGroup;
    public TMP_Text speakerText;
    public TMP_Text subtitleText;

    [Header("Audio")]
    public AudioSource voiceSource;

    private Coroutine currentDialogue;
    private bool isPlaying;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideDialogue();
    }

    public void Play(DialogueLine line)
    {
        if (currentDialogue != null)
            StopCoroutine(currentDialogue);

        currentDialogue = StartCoroutine(PlayRoutine(line));
    }

    IEnumerator PlayRoutine(DialogueLine line)
    {
        dialogueGroup.alpha = 1;
        dialogueGroup.blocksRaycasts = false;
        dialogueGroup.interactable = false;

        speakerText.text = line.speakerName;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.dialogue));

        if (line.voiceClip != null)
        {
            voiceSource.Stop();
            voiceSource.clip = line.voiceClip;
            voiceSource.Play();
        }

        float waitTime = line.duration;

        if (line.voiceClip != null)
            waitTime = Mathf.Max(waitTime, line.voiceClip.length);

        waitingForNext = true;

        while (waitingForNext)
        {
            yield return null;
        }

        if (!isPlaying)
        {
            HideDialogue();
        }
    }
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        skipTyping = false;

        subtitleText.text = "";

        foreach (char c in text)
        {
            if (skipTyping)
            {
                subtitleText.text = text;
                break;
            }

            subtitleText.text += c;

            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }
    public void PlaySequence(DialogueSequence sequence)
    {
        if (currentDialogue != null)
            StopCoroutine(currentDialogue);

        currentDialogue = StartCoroutine(PlaySequenceRoutine(sequence));
    }
    IEnumerator PlaySequenceRoutine(DialogueSequence sequence)
    {
        isPlaying = true;

        foreach (DialogueLine line in sequence.lines)
        {
            yield return StartCoroutine(PlayRoutine(line));
        }

        isPlaying = false;
        HideDialogue();
    }
    public DialogueSequence intro;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DialogueManager.Instance.PlaySequence(intro);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                waitingForNext = false;
            }
        }
    }

    void HideDialogue()
    {
        dialogueGroup.alpha = 0;
        speakerText.text = "";
        subtitleText.text = "";

        if (voiceSource != null)
            voiceSource.Stop();
    }
}