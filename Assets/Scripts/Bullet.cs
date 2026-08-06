using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;

    private Rigidbody body;
    private Bullet sourcePrefab;
    private float despawnTime;
    private bool isLive;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    /// Khởi động lại viên đạn vừa lấy từ pool: xoá quán tính cũ và hẹn giờ tự thu hồi.
    public void Arm(Bullet prefab, float lifeTime)
    {
        sourcePrefab = prefab;
        despawnTime = Time.time + lifeTime;
        isLive = true;

        if (body != null)
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        if (isLive && Time.time >= despawnTime)
        {
            Despawn();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isLive)
        {
            return;
        }

        Transform hitTransform = collision.transform;
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            if (hitTransform.CompareTag("Player") &&
                hitTransform.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.TakeDamage(10f);
            }
            CreateBulletImpactEffect(collision);
            Despawn();
        }
        if (collision.gameObject.CompareTag("Target") &&
            collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(bulletDamage);
        }
    }

    void CreateBulletImpactEffect(Collision collision)
    {
        if (GlobalReferences.Instance == null)
        {
            return;
        }

        ContactPoint contact = collision.contacts[0];
        GlobalReferences.Instance.SpawnImpact(contact.point, Quaternion.LookRotation(contact.normal), collision.transform);
    }

    private void Despawn()
    {
        if (!isLive)
        {
            return;
        }
        isLive = false;

        if (body != null)
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

        if (GlobalReferences.Instance != null)
        {
            GlobalReferences.Instance.ReleaseBullet(sourcePrefab, this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
