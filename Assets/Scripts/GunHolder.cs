using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public GameObject[] weapons;
    public int currentWeapon;
    void Start()
    {
        currentWeapon = -1;
        for (int i = 0; i < weapons.Length; i++)
        {
            Gun g = weapons[i]?.GetComponent<Gun>();
            bool playable = (g != null) && g.isPlayable;
            weapons[i].SetActive(false);
            if (playable && currentWeapon == -1)
            {
                currentWeapon = i;
            }
        }
        if (currentWeapon == -1)
        {
            currentWeapon = 0;
        }
        else
        {
            weapons[currentWeapon].SetActive(true);
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.GetCurrentGun(currentWeapon);
                HUDManager.Instance.GetCurrentAmmo(currentWeapon);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(2);
    }

    public void SelectWeapon(int index)
    {
        if (index >= weapons.Length || index == currentWeapon) return;

        Gun currentGun = weapons[currentWeapon].GetComponent<Gun>();
        if (currentGun.isReloading)
            return;
        HUDManager.Instance.GetPrevGun(currentWeapon);
        Gun gun = weapons[index].GetComponent<Gun>();
        if (gun != null && !gun.isPlayable)
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }
        currentWeapon = index;
        HUDManager.Instance.GetCurrentGun(currentWeapon);
        HUDManager.Instance.GetCurrentAmmo(currentWeapon);
    }
    public Gun GetCurrentWeapon()
    {
        return weapons[currentWeapon].GetComponent<Gun>();
    }
}