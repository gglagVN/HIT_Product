using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;

    [Header("Hit Effects")]
    [SerializeField] private ParticleSystem enemyHitParticle;

    private Rigidbody body;
    private Bullet sourcePrefab;
    private float despawnTime;
    private bool isLive;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

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
            return;

        // =========================
        // BẮN TRÚNG ENEMY
        // =========================
        if (collision.gameObject.CompareTag("Target"))
        {
            if (collision.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(bulletDamage);
            }

            CreateEnemyHitEffect(collision);

            Despawn();
            return;
        }

        // =========================
        // BẮN TRÚNG PLAYER
        // =========================
        if (collision.transform.CompareTag("Player") &&
            collision.transform.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(10f);

            CreateBulletImpactEffect(collision);

            Despawn();
            return;
        }

        // =========================
        // BẮN TRÚNG VẬT THỂ KHÁC
        // =========================
        if (!collision.gameObject.CompareTag("Bullet"))
        {
            CreateBulletImpactEffect(collision);
            Despawn();
        }
    }

    private void CreateEnemyHitEffect(Collision collision)
    {
        if (enemyHitParticle == null)
            return;

        ContactPoint contact = collision.contacts[0];

        ParticleSystem effect = Instantiate(
            enemyHitParticle,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        effect.Play();

        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
    }

    private void CreateBulletImpactEffect(Collision collision)
    {
        if (GlobalReferences.Instance == null)
            return;

        ContactPoint contact = collision.contacts[0];

        GlobalReferences.Instance.SpawnImpact(
            contact.point,
            Quaternion.LookRotation(contact.normal),
            collision.transform
        );
    }

    private void Despawn()
    {
        if (!isLive)
            return;

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