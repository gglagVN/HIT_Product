using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    private GunAudio gunAudio;
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public MonoBehaviour playerMovement;
    public PlayerLook playerLook;

    [Header("Shooting")]
    public bool isPlayable = false;
    public int weaponDamage;
    public float shootingDelay = 0.15f;
    private float spreadIntensity = 0.02f;
    public float hipSpreadIntensity = 0;
    public float adsSpreadIntensity = 0;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public GameObject muzzleEffect;
    private Animator anim;
    public float reloadTime;
    public int magazineSize, bulletsLeft, amountOfBullet;
    public bool isReloading;

    [Header("Burst")]
    public int bulletPerBurst = 3;

    [Header("Shotgun")]
    public int pelletsPerShot = 1; // 1 = súng thường, >1 = shotgun

    private Bullet bulletPrefabBullet;
    private ParticleSystem muzzleParticles;
    private bool isShooting;
    private bool readyToShoot = true;
    private bool allowReset = true;
    private int burstBulletsLeft;
    public bool isADS;

    private int lastMagazineAmmoDisplayed = int.MinValue;
    private int lastTotalAmmoDisplayed = int.MinValue;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode = ShootingMode.Single;

    public enum AmmoType
    {
        Pistol,
        Rifle,
        Shotgun
    }

    [Header("Ammo Type")]
    public AmmoType ammoType = AmmoType.Pistol;
    public int maxAmmo = 120;

    private void Awake()
    {
        burstBulletsLeft = bulletPerBurst;
        anim = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        amountOfBullet = Mathf.Max(amountOfBullet, 0);
        spreadIntensity = hipSpreadIntensity;
        gunAudio = GetComponent<GunAudio>();

        if (bulletPrefab != null && !bulletPrefab.TryGetComponent(out bulletPrefabBullet))
        {
            Debug.LogError($"{name}: bulletPrefab thiếu component Bullet.", this);
        }

        if (muzzleEffect != null)
        {
            muzzleParticles = muzzleEffect.GetComponent<ParticleSystem>();
        }

        if (playerLook == null)
        {
            playerLook = FindObjectOfType<PlayerLook>();
        }
    }

    private void OnEnable()
    {
        lastMagazineAmmoDisplayed = int.MinValue;
        lastTotalAmmoDisplayed = int.MinValue;
    }

    public void AddAmmo(int amount)
    {
        if (amount <= 0) return;

        amountOfBullet = Mathf.Min(amountOfBullet + amount, maxAmmo);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isReloading)
        {
            EnterADS();
        }
        if (Input.GetMouseButtonUp(1) && !isReloading)
        {
            ExitADS();
        }
        switch (currentShootingMode)
        {
            case ShootingMode.Auto:
                isShooting = Input.GetMouseButton(0);
                break;

            case ShootingMode.Single:
            case ShootingMode.Burst:
                isShooting = Input.GetMouseButtonDown(0);
                break;
        }

        if (Input.GetKeyDown(KeyCode.R) &&
            bulletsLeft < magazineSize &&
            !isReloading)
        {
            Reload();
        }

        // if (readyToShoot &&
        //     !isShooting &&
        //     !isReloading &&
        //     bulletsLeft <= 0)
        // {
        //     Reload();
        // }

        if (readyToShoot &&
            isShooting &&
            !isReloading)
        {
            burstBulletsLeft = bulletPerBurst;
            FireWeapon();
        }

        UpdateAmmoUI();

    }

    /// <summary>
    /// Đẩy số đạn lên HUD, chỉ ghi lại text khi giá trị hiển thị thay đổi.
    /// </summary>
    private void UpdateAmmoUI()
    {
        HUDManager hud = HUDManager.Instance;
        if (hud == null || bulletPerBurst == 0) return;

        int magazineAmmo = bulletsLeft / bulletPerBurst;
        int totalAmmo = amountOfBullet / bulletPerBurst;

        if (magazineAmmo != lastMagazineAmmoDisplayed)
        {
            lastMagazineAmmoDisplayed = magazineAmmo;
            if (hud.magazineAmmoUI != null)
            {
                hud.magazineAmmoUI.SetText("{0}", magazineAmmo);
            }
        }

        if (totalAmmo != lastTotalAmmoDisplayed)
        {
            lastTotalAmmoDisplayed = totalAmmo;
            if (hud.totalAmmoUI != null)
            {
                hud.totalAmmoUI.SetText("{0}", totalAmmo);
            }
        }
    }
    private void EnterADS()
    {
        anim.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.CrossHair.SetActive(false);
        spreadIntensity = adsSpreadIntensity;

        if (playerLook != null)
        {
            playerLook.SetADS(true);
        }
    }
    private void ExitADS()
    {
        anim.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.CrossHair.SetActive(true);
        spreadIntensity = hipSpreadIntensity;

        if (playerLook != null)
        {
            playerLook.SetADS(false);
        }
    }

    private void FireWeapon()
    {
        if (playerMovement.enabled == false || Time.timeScale == 0) return;
        if (bulletsLeft <= 0)
        {
            gunAudio.PlayEmpty();
            return;
        }
        bulletsLeft--;
        gunAudio.PlayShoot();

        if (muzzleParticles != null)
        {
            muzzleParticles.Play();
        }
        if (pelletsPerShot > 1)
        {
            CameraShake.Instance.Shake(0.15f, 0.1f);
        }
        else
        {
            CameraShake.Instance.Shake(0.08f, 0.04f);
        }
        readyToShoot = false;

        if (anim != null)
        {
            if (isADS)
            {
                anim.SetTrigger("RECOIL_ADS");
            }
            else
            {
                anim.SetTrigger("RECOIL");
            }

        }

        // Bắn nhiều viên nếu là shotgun
        for (int i = 0; i < pelletsPerShot; i++)
        {
            Vector3 shootingDirection =
                CalculateDirectionAndSpread().normalized;

            Bullet bul = GlobalReferences.Instance.SpawnBullet(
                bulletPrefabBullet,
                bulletSpawn.position,
                Quaternion.identity,
                bulletPrefabLifeTime);

            if (bul == null)
            {
                continue;
            }

            bul.bulletDamage = weaponDamage;
            bul.transform.forward = shootingDirection;

            if (bul.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(
                    shootingDirection * bulletVelocity,
                    ForceMode.Impulse);
            }
        }

        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst &&
            burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void Reload()
    {
        if (isReloading || amountOfBullet == 0) return;
        gunAudio.PlayReload();
        anim.SetTrigger("RELOAD");
        isReloading = true;

        Invoke(nameof(ReloadCompleted), reloadTime);
    }

    private void ReloadCompleted()
    {
        int bulletsToReload = Mathf.Min(magazineSize - bulletsLeft, amountOfBullet);
        bulletsLeft += bulletsToReload;
        amountOfBullet -= bulletsToReload;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        LayerMask mask = ~LayerMask.GetMask("WeaponRender");

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, mask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000f);
        }

        Vector3 direction = (targetPoint - bulletSpawn.position).normalized;

        float z = Random.Range(-spreadIntensity, spreadIntensity);
        float y = Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }
}