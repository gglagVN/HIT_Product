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
        for (int i = 0; i < listGunIsActive.Length; i++)
        {
            listGunIsActive[i].SetActive(i == index);
        }
    }
    public void GetPrevGun(int index)
    {
        for (int i = 0; i < listGunIsUnactive.Length; i++)
        {
            listGunIsUnactive[i].SetActive(i == index);
        }
    }
}
