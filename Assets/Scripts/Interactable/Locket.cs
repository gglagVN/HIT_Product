using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locket : Interactable
{
    public GameObject door;
    private bool openDoor;

    public bool IsOpen => openDoor;

    protected override void Interact()
    {
        openDoor = !openDoor;
        Animator anim = door.GetComponent<Animator>();
        if (anim.GetBool("isOpened") == false)
            anim.SetBool("isOpened", true);
        else
            anim.SetBool("isOpened", false);
    }

    /// <summary>
    /// Đưa cửa về trạng thái đã mở mà không đảo trạng thái hiện tại.
    /// </summary>
    public void ForceOpen()
    {
        openDoor = true;

        if (door == null)
        {
            Debug.LogError($"{name}: chưa gán door cho Locket.", this);
            return;
        }

        Animator anim = door.GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("isOpened", true);
    }
}
