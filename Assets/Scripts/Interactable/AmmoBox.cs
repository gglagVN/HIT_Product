using UnityEngine;

public class AmmoBox : Interactable
{
    private GunHolder gunHolder;
    public int ammoAmount = 30;
    public Gun.AmmoType ammoType = Gun.AmmoType.Pistol;
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            gunHolder = player.GetComponent<GunHolder>();
        }
    }

    protected override void Interact()
    {
        if (gunHolder == null) return;

        foreach (GameObject weapon in gunHolder.weapons)
        {
            if (weapon == null) continue;

            Gun gun = weapon.GetComponent<Gun>();
            if (gun != null && gun.ammoType == ammoType)
            {
                gun.AddAmmo(ammoAmount);
            }
        }

        Destroy(gameObject);
    }
}
