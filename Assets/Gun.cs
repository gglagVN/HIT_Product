using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Header("Shooting")]
    public float shootingDelay = 0.15f;
    public float spreadIntensity = 0.02f;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public GameObject muzzleEffect;
    private Animator anim;
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header("Burst")]
    public int bulletPerBurst = 3;

    [Header("Shotgun")]
    public int pelletsPerShot = 1; // 1 = súng thường, >1 = shotgun

    private bool isShooting;
    private bool readyToShoot = true;
    private bool allowReset = true;
    private int burstBulletsLeft;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode = ShootingMode.Single;

    private void Awake()
    {
        burstBulletsLeft = bulletPerBurst;
        anim = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    private void Update()
    {
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

        if (readyToShoot &&
            !isShooting &&
            !isReloading &&
            bulletsLeft <= 0)
        {
            Reload();
        }

        if (readyToShoot &&
            isShooting &&
            bulletsLeft > 0 &&
            !isReloading)
        {
            burstBulletsLeft = bulletPerBurst;
            FireWeapon();
        }

        if (AmmoManager.Instance.ammoDisplay != null)
        {
            AmmoManager.Instance.ammoDisplay.text =
                $"{bulletsLeft}/{magazineSize}";
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        if (muzzleEffect != null)
        {
            muzzleEffect.GetComponent<ParticleSystem>().Play();
        }

        readyToShoot = false;

        if (anim != null)
        {
            anim.SetTrigger("RECOIL");
        }

        // Bắn nhiều viên nếu là shotgun
        for (int i = 0; i < pelletsPerShot; i++)
        {
            Vector3 shootingDirection =
                CalculateDirectionAndSpread().normalized;

            GameObject bullet =
                Instantiate(
                    bulletPrefab,
                    bulletSpawn.position,
                    Quaternion.identity);

            bullet.transform.forward = shootingDirection;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(
                    shootingDirection * bulletVelocity,
                    ForceMode.Impulse);
            }

            StartCoroutine(
                DestroyBulletAfterTime(
                    bullet,
                    bulletPrefabLifeTime));
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
        if (isReloading) return;

        anim.SetTrigger("RELOAD");
        isReloading = true;

        Invoke(nameof(ReloadCompleted), reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private IEnumerator DestroyBulletAfterTime(
        GameObject bullet,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bullet != null)
        {
            Destroy(bullet);
        }
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

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000f);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x =
            Random.Range(
                -spreadIntensity,
                spreadIntensity);

        float y =
            Random.Range(
                -spreadIntensity,
                spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }
}