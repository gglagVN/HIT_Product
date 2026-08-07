using System.Collections.Generic;
using UnityEngine;
using Thnguyet.SaveGame;

public class GunHolderSaveData : SaveableComponent
{
    [Tooltip("The GunHolder component that manages player weapons.")]
    public GunHolder gunHolder;

    [System.Serializable]
    public class GunAmmoInfo
    {
        public string weaponName;
        public int bulletsLeft;
        public int amountOfBullet;
    }

    [System.Serializable]
    public class GunHolderData
    {
        public int currentWeaponIndex;
        public List<GunAmmoInfo> weapons = new List<GunAmmoInfo>();
    }

    private GunHolderData loadedData;

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = "GunHolderState";
    }

    protected override void Awake()
    {
        if (gunHolder == null)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
                gunHolder = playerObject.GetComponent<GunHolder>();
        }

        base.Awake();
    }

    public override object GetData()
    {
        var result = new GunHolderData();
        if (gunHolder == null || gunHolder.weapons == null)
            return result;

        result.currentWeaponIndex = gunHolder.currentWeapon;
        for (int i = 0; i < gunHolder.weapons.Length; i++)
        {
            var weaponObject = gunHolder.weapons[i];
            if (weaponObject == null)
                continue;

            var gun = weaponObject.GetComponent<Gun>();
            if (gun == null)
                continue;

            result.weapons.Add(new GunAmmoInfo
            {
                weaponName = weaponObject.name,
                bulletsLeft = gun.bulletsLeft,
                amountOfBullet = gun.amountOfBullet
            });
        }

        return result;
    }

    public override void SetData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return;

        loadedData = JsonUtility.FromJson<GunHolderData>(data);
        if (loadedData == null)
            return;

        if (gunHolder == null || gunHolder.weapons == null)
            return;

        foreach (var weaponInfo in loadedData.weapons)
        {
            foreach (var weaponObject in gunHolder.weapons)
            {
                if (weaponObject == null || weaponObject.name != weaponInfo.weaponName)
                    continue;

                var gun = weaponObject.GetComponent<Gun>();
                if (gun == null)
                    continue;

                gun.bulletsLeft = weaponInfo.bulletsLeft;
                gun.amountOfBullet = weaponInfo.amountOfBullet;
                break;
            }
        }
    }

    public override void OnAllDataLoaded()
    {
        if (loadedData == null || gunHolder == null || gunHolder.weapons == null)
            return;

        int selection = Mathf.Clamp(loadedData.currentWeaponIndex, 0, gunHolder.weapons.Length - 1);
        gunHolder.SelectWeapon(selection);
    }
}
