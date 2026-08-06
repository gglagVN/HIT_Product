using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public GameObject door;
    private bool openDoor;
    protected override void Interact()
    {
        openDoor = !openDoor;
        Animator anim = door.GetComponent<Animator>();
        if (anim.GetBool("isOpened") == false)
            anim.SetBool("isOpened", true);
        else
            anim.SetBool("isOpened", false);
    }
}
