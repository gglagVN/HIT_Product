using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteract : Interactable
{
    public Gun gunToUnlock;
    public int index;

    /// <summary>
    /// Chờ hết frame khởi tạo rồi bỏ vật phẩm nếu khẩu súng đã được mở khoá từ save.
    /// </summary>
    private IEnumerator Start()
    {
        yield return null;

        if (gunToUnlock != null && gunToUnlock.isPlayable)
        {
            Destroy(gameObject);
        }
    }

    protected override void Interact()
    {
        gunToUnlock.isPlayable = true;
        GunHolder gunHolder = FindObjectOfType<GunHolder>();
        gunHolder.SelectWeapon(index);
        Destroy(gameObject);
    }
}
