using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    public GameObject door;
    private bool openDoor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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
