using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public GameObject[] weapons;
    public int currentWeapon;
    void Start()
    {
        currentWeapon = 0;
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
        if (index >= weapons.Length) return;

        Gun currentGun = weapons[currentWeapon].GetComponent<Gun>();
        if (currentGun.isReloading)
            return;

        Gun gun = weapons[index].GetComponent<Gun>();
        if (gun != null && !gun.isPlayable)
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        currentWeapon = index;
    }
}