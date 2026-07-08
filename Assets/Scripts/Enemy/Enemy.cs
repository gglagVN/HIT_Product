using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum EnemyStyle
    {
        Gunner,
        Zombie
    }

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnowPos;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPos { get => lastKnowPos; set => lastKnowPos = value; }
    [SerializeField]
    private string currentState;
    public Paths path;
    public GameObject debugSphere;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Combat")]
    public EnemyStyle enemyStyle = EnemyStyle.Gunner;
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate = 1f;
    public float meleeRange = 2f;
    public float meleeDamage = 10f;
    public float meleeCooldown = 1f;
    public float walkSpeed = 2f;
    public float chaseSpeed = 4.5f;
    private float nextAttackTime;
    private Animator anim;
    private bool attackAnimationPlaying;

    void Start()
    {
        anim = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        if (Agent != null)
        {
            Agent.speed = walkSpeed;
        }
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
        ResetAnimationBools();
    }

    void Update()
    {
        bool playerSeen = CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
        if (debugSphere != null)
        {
            debugSphere.transform.position = lastKnowPos;
        }
        UpdateAnimationState(playerSeen);
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer <= fieldOfView / 2f)
                {
                    Ray ray = new Ray(
                        transform.position + (Vector3.up * eyeHeight),
                        targetDirection.normalized
                    );

                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                            LastKnownPos = player.transform.position;
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void UpdateAnimationState(bool playerSeen)
    {
        if (anim == null)
        {
            return;
        }

        bool isMoving = Agent != null && Agent.velocity.sqrMagnitude > 0.1f;
        bool shouldChase = playerSeen && enemyStyle == EnemyStyle.Zombie;

        if (shouldChase)
        {
            Agent.speed = chaseSpeed;
        }
        else
        {
            Agent.speed = walkSpeed;
        }

        anim.SetBool("isWalking", !attackAnimationPlaying && !shouldChase && isMoving);
        anim.SetBool("isChasing", !attackAnimationPlaying && shouldChase);
        anim.SetBool("isAttacking", attackAnimationPlaying);
    }

    private void ResetAnimationBools()
    {
        if (anim == null)
        {
            return;
        }

        anim.SetBool("isWalking", false);
        anim.SetBool("isChasing", false);
        anim.SetBool("isAttacking", false);
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        attackAnimationPlaying = false;
        UpdateAnimationState(CanSeePlayer());
    }

    public virtual void PerformAttack()
    {
        if (enemyStyle == EnemyStyle.Gunner)
        {
            Shoot();
        }
        else
        {
            TryMeleeAttack();
        }
    }

    public virtual void SetAttackDestination()
    {
        if (enemyStyle == EnemyStyle.Zombie && Player != null)
        {
            Agent.SetDestination(Player.transform.position);
        }
        else
        {
            Agent.SetDestination(transform.position + (Random.insideUnitSphere * 5f));
        }
    }

    public virtual void Shoot()
    {
        if (gunBarrel == null || Player == null)
        {
            return;
        }

        GameObject bullet = Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, transform.rotation);
        Vector3 shootDirection = (Player.transform.position - gunBarrel.transform.position).normalized;
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40f;
        }
    }

    public virtual void TryMeleeAttack()
    {
        if (Player == null || Time.time < nextAttackTime)
        {
            return;
        }
        if (enemyStyle == Enemy.EnemyStyle.Zombie)
        {
            Agent.SetDestination(Player.transform.position);
        }
        if (Vector3.Distance(transform.position, Player.transform.position) <= meleeRange)
        {
            attackAnimationPlaying = true;
            if (anim != null)
            {
                anim.SetBool("isAttacking", true);
            }
            StartCoroutine(ResetAttackAnimation());

            PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
            }

            nextAttackTime = Time.time + meleeCooldown;
        }
    }
}
