using System.Collections.Generic;
using Thnguyet.Pool.Extension;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; }
    public GameObject bulletImpactEffectPrefab;

    [SerializeField] private int bulletPoolCapacity = 32;
    [SerializeField] private int impactPoolCapacity = 32;

    private readonly Dictionary<Bullet, ComponentPool<Bullet>> bulletPools =
        new Dictionary<Bullet, ComponentPool<Bullet>>();

    private ComponentPool<Transform> impactPool;
    private Vector3 impactPrefabScale;

    void Awake()
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

    /// Lấy một viên đạn từ pool ứng với prefab, đặt sẵn vị trí, hướng và hẹn giờ tự thu hồi.
    public Bullet SpawnBullet(Bullet prefab, Vector3 position, Quaternion rotation, float lifeTime)
    {
        if (prefab == null)
        {
            Debug.LogError("GlobalReferences: chưa gán bullet prefab cho chỗ gọi SpawnBullet.", this);
            return null;
        }

        if (!bulletPools.TryGetValue(prefab, out ComponentPool<Bullet> pool))
        {
            pool = new ComponentPool<Bullet>(null, prefab, bulletPoolCapacity);
            bulletPools.Add(prefab, pool);
        }

        Bullet bullet = pool.Get();
        bullet.transform.SetPositionAndRotation(position, rotation);
        bullet.Arm(prefab, lifeTime);
        return bullet;
    }

    /// Trả viên đạn về đúng pool đã cấp phát nó.
    public void ReleaseBullet(Bullet prefab, Bullet bullet)
    {
        if (bullet == null)
        {
            return;
        }

        if (prefab != null && bulletPools.TryGetValue(prefab, out ComponentPool<Bullet> pool))
        {
            pool.Release(bullet);
            return;
        }

        Destroy(bullet.gameObject);
    }

    /// Lấy một hiệu ứng va đạn từ pool, đặt vị trí, hướng và cho bám theo vật bị bắn nếu attachTo khác null.
    public Transform SpawnImpact(Vector3 position, Quaternion rotation, Transform attachTo)
    {
        if (bulletImpactEffectPrefab == null)
        {
            Debug.LogError("GlobalReferences: chưa gán bulletImpactEffectPrefab.", this);
            return null;
        }

        if (impactPool == null)
        {
            if (bulletImpactEffectPrefab.GetComponent<DecalDestroyer>() == null)
            {
                Debug.LogError("GlobalReferences: bulletImpactEffectPrefab thiếu DecalDestroyer ở root nên hiệu ứng sẽ không bao giờ được trả về pool.", this);
            }

            impactPrefabScale = bulletImpactEffectPrefab.transform.localScale;
            impactPool = new ComponentPool<Transform>(null, bulletImpactEffectPrefab.transform, impactPoolCapacity);
        }

        PruneDestroyedImpacts();

        Transform impact = impactPool.Get();
        impact.localScale = impactPrefabScale;

        if (attachTo != null)
        {
            impact.SetParent(attachTo, true);
        }

        impact.SetPositionAndRotation(position, rotation);

        if (impact.TryGetComponent(out DecalDestroyer destroyer))
        {
            destroyer.MarkPooled();
        }

        return impact;
    }

    /// Gỡ hiệu ứng va đạn khỏi vật đang bám theo rồi trả về pool.
    /// Trả về false khi instance không do pool này quản, lúc đó chỗ gọi phải tự huỷ nó.
    public bool ReleaseImpact(Transform instance)
    {
        if (instance == null || impactPool == null)
        {
            return false;
        }

        if (!impactPool.ActiveElements.Contains(instance))
        {
            return true;
        }

        instance.SetParent(null, true);
        impactPool.Release(instance);
        return true;
    }

    /// Loại khỏi danh sách đang hoạt động những hiệu ứng đã bị destroy kèm vật cha, tránh trả object chết về pool.
    private void PruneDestroyedImpacts()
    {
        List<Transform> activeImpacts = impactPool.ActiveElements;
        for (int i = activeImpacts.Count - 1; i >= 0; i--)
        {
            if (activeImpacts[i] == null)
            {
                activeImpacts.RemoveAt(i);
            }
        }
    }
}
