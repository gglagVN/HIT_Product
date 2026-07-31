using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDialogue : MonoBehaviour
{
    public DialogueSequence dialogue;
    public void Play()
    {
        DialogueManager.Instance.PlaySequence(dialogue);
    }

}
