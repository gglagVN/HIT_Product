using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteract : Interactable
{
    public Gun gunToUnlock;
    public int index;

    protected override void Interact()
    {
        gunToUnlock.isPlayable = true;
        GunHolder gunHolder = FindObjectOfType<GunHolder>();
        gunHolder.SelectWeapon(index);
        Destroy(gameObject);
    }
}
