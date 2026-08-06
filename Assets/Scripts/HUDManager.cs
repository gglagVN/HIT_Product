using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; set; }
    [Header("Ammo")]
    public GameObject[] listAmmo;
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    [Header("Weapon")]
    public GameObject[] listGunIsActive;
    public GameObject[] listGunIsUnactive;
    [Header("Throwable")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;
    public GameObject CrossHair;
    private GunHolder cachedGunHolder;
    private Gun[] cachedGuns;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void GetCurrentAmmo(int index)
    {
        for (int i = 0; i < listAmmo.Length; i++)
        {
            listAmmo[i].SetActive(i == index);
        }
    }
    public void GetCurrentGun(int index)
    {
        EnsureGunCache();
        for (int i = 0; i < listGunIsActive.Length; i++)
        {
            listGunIsActive[i].SetActive(IsWeaponUnlocked(i) && i == index);
        }
    }
    public void GetPrevGun(int index)
    {
        EnsureGunCache();
        for (int i = 0; i < listGunIsUnactive.Length; i++)
        {
            listGunIsUnactive[i].SetActive(IsWeaponUnlocked(i) && i == index);
        }
    }

    /// <summary>
    /// Tìm GunHolder một lần và cache sẵn mảng Gun tương ứng với danh sách vũ khí.
    /// </summary>
    private void EnsureGunCache()
    {
        if (cachedGunHolder == null)
        {
            cachedGunHolder = FindObjectOfType<GunHolder>();
            cachedGuns = null;
        }

        if (cachedGunHolder == null || cachedGunHolder.weapons == null)
        {
            cachedGuns = null;
            return;
        }

        if (cachedGuns != null && cachedGuns.Length == cachedGunHolder.weapons.Length) return;

        cachedGuns = new Gun[cachedGunHolder.weapons.Length];
        for (int i = 0; i < cachedGuns.Length; i++)
        {
            GameObject weapon = cachedGunHolder.weapons[i];
            cachedGuns[i] = weapon != null ? weapon.GetComponent<Gun>() : null;
        }
    }

    /// <summary>
    /// Kiểm tra vũ khí ở vị trí index đã được mở khoá hay chưa.
    /// </summary>
    private bool IsWeaponUnlocked(int index)
    {
        if (cachedGuns == null || index >= cachedGuns.Length) return true;

        Gun gun = cachedGuns[index];
        return (gun == null) || gun.isPlayable;
    }
}
