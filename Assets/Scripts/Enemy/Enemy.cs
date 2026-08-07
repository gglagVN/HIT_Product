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
    private EnemyAudio enemyAudio;
    private bool playerDetected = false;

    private float nextIdleSound;
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnowPos;
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPos { get => lastKnowPos; set => lastKnowPos = value; }
#if UNITY_EDITOR
    [SerializeField]
    private string currentState;
#endif
    public Paths path;
    public GameObject debugSphere;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;
    [SerializeField] private LayerMask sightMask = ~0;

    private bool playerVisible;
    private int playerVisibleFrame = -1;

    public bool PlayerVisible
    {
        get
        {
            if (playerVisibleFrame != Time.frameCount)
            {
                playerVisibleFrame = Time.frameCount;
                playerVisible = CanSeePlayer();
            }
            return playerVisible;
        }
    }

    [Header("Combat")]
    public EnemyStyle enemyStyle = EnemyStyle.Gunner;
    public Transform gunBarrel;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float bulletLifeTime = 5f;
    private static Bullet fallbackBulletPrefab;
    [Range(0.1f, 10f)]
    public float fireRate = 1f;
    public float meleeRange = 2f;
    public float meleeDamage = 10f;
    public float meleeCooldown = 1f;
    public float walkSpeed = 2f;
    public float chaseSpeed = 4.5f;
    [Tooltip("Tốc độ xoay người về phía player, độ/giây")]
    public float turnSpeed = 360f;
    public float blendSmooth = 0.12f;

    [Header("Animation")]
    [Tooltip("Vận tốc mà clip đi bộ trông khớp mặt đất khi phát ở 1x. Hạ xuống nếu chân đạp chậm hơn thân")]
    [SerializeField] private float walkClipReferenceSpeed = 2f;
    [Tooltip("Vận tốc mà clip chạy trông khớp mặt đất khi phát ở 1x")]
    [SerializeField] private float runClipReferenceSpeed = 4.5f;
    [SerializeField] private Vector2 animSpeedRange = new Vector2(0.4f, 1.6f);
    private float losePlayerTimer;
    [SerializeField] private float losePlayerDelay = 2f;
    private float nextAttackTime;
    private Animator anim;
    private bool attackAnimationPlaying;

    private static readonly int AnimIsWalking = Animator.StringToHash("isWalking");
    private static readonly int AnimIsChasing = Animator.StringToHash("isChasing");
    private static readonly int AnimIsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int AnimMoveX = Animator.StringToHash("moveX");
    private static readonly int AnimMoveY = Animator.StringToHash("moveY");
    private static readonly int AnimSpeed = Animator.StringToHash("animSpeed");

    private bool animIsWalking;
    private bool animIsChasing;
    private bool animIsAttacking;

    private void SetAnimBool(int parameterHash, bool value, ref bool cachedValue)
    {
        if (cachedValue == value)
        {
            return;
        }
        cachedValue = value;
        anim.SetBool(parameterHash, value);
    }

    private bool IsGunnerPathing()
    {
        return Agent != null && Agent.hasPath && !Agent.pathPending && Agent.remainingDistance > Agent.stoppingDistance + 0.1f;
    }

    private bool IsGunnerStopped()
    {
        return Agent != null && !IsGunnerPathing();
    }

    void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();
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
        nextIdleSound = Time.time + Random.Range(3f, 8f);
    }

    void Update()
    {
        if (isDead)
            return;
        bool playerSeen = PlayerVisible;

        HandleFootstepState(playerSeen);

        HandleEnemyAudio(playerSeen);

#if UNITY_EDITOR
        currentState = stateMachine != null && stateMachine.activeState != null
            ? stateMachine.activeState.ToString()
            : string.Empty;
#endif

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
                // Đã giao chiến thì không cần đúng hướng nhìn nữa, nếu không địch sẽ mất dấu ngay khi vòng qua bên hông
                bool engaged = stateMachine != null && stateMachine.activeState is AttackState;
                if (engaged || angleToPlayer <= fieldOfView / 2f)
                {
                    Ray ray = new Ray(
                        transform.position + (Vector3.up * eyeHeight),
                        targetDirection.normalized
                    );

                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, sightDistance, sightMask, QueryTriggerInteraction.Ignore))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
#if UNITY_EDITOR
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
#endif
                            LastKnownPos = player.transform.position;
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    private void HandleEnemyAudio(bool playerSeen)
    {
        if (enemyAudio == null)
            return;

        //-------------------------
        // Detect
        //-------------------------

        if (playerSeen)
        {
            losePlayerTimer = Time.time + losePlayerDelay;

            if (!playerDetected)
            {
                playerDetected = true;
                enemyAudio.PlayDetect();
            }
        }
        else
        {
            if (Time.time >= losePlayerTimer)
            {
                playerDetected = false;
            }
        }

        //-------------------------
        // Idle
        //-------------------------

        if (!playerSeen && Time.time >= nextIdleSound)
        {
            if (enemyStyle == EnemyStyle.Zombie)
                enemyAudio.PlayIdle();

            nextIdleSound = Time.time + Random.Range(8f, 15f);
        }
    }
    private bool isRunning;
    private bool isWalking;
    private void UpdateAnimationState(bool playerSeen)
    {
        if (anim == null)
        {
            return;
        }

        bool isMoving = Agent != null && Agent.velocity.sqrMagnitude > 0.1f;
        float moveX = 0f;
        float moveY = 0f;

        // compute local velocity for blend-tree (X = strafe, Y = forward)
        if (Agent != null)
        {
            Vector3 localVel = transform.InverseTransformDirection(Agent.velocity);
            float speedDenom = Mathf.Max(Agent.speed, 0.0001f);
            moveX = localVel.x / speedDenom;
            moveY = localVel.z / speedDenom;
        }

        if (enemyStyle == EnemyStyle.Zombie)
        {
            bool shouldChase = playerSeen;

            if (Agent != null)
            {
                Agent.speed = shouldChase ? chaseSpeed : walkSpeed;
            }

            SetAnimBool(AnimIsWalking, !attackAnimationPlaying && !shouldChase && isMoving, ref animIsWalking);
            SetAnimBool(AnimIsChasing, !attackAnimationPlaying && shouldChase, ref animIsChasing);
            SetAnimBool(AnimIsAttacking, attackAnimationPlaying, ref animIsAttacking);
            UpdateStrideSpeed(shouldChase);
            // Zombie doesn't use blend tree; skip setting moveX/moveY
            return;
        }

        bool isAttackState = stateMachine != null && stateMachine.activeState is AttackState;
        bool isPathing = IsGunnerPathing();
        bool shouldAttackGunner = attackAnimationPlaying || (isAttackState && playerSeen && !isPathing);
        bool shouldChaseGunner = playerSeen && (isAttackState || isPathing);
        bool shouldWalkGunner = !isAttackState && !playerSeen && isMoving;

        SetAnimBool(AnimIsWalking, shouldWalkGunner, ref animIsWalking);
        SetAnimBool(AnimIsChasing, shouldChaseGunner, ref animIsChasing);
        SetAnimBool(AnimIsAttacking, shouldAttackGunner, ref animIsAttacking);

        if (shouldChaseGunner)
        {
            anim.SetFloat(AnimMoveX, moveX, blendSmooth, Time.deltaTime);
            anim.SetFloat(AnimMoveY, moveY, blendSmooth, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(AnimMoveX, 0f, blendSmooth, Time.deltaTime);
            anim.SetFloat(AnimMoveY, 0f, blendSmooth, Time.deltaTime);
        }

    }
    /// <summary>
    /// Khớp tốc độ phát clip bước chân với vận tốc di chuyển thật để chân không trượt trên sàn.
    /// </summary>
    private void UpdateStrideSpeed(bool chasing)
    {
        if (Agent == null)
        {
            return;
        }

        float reference = chasing ? runClipReferenceSpeed : walkClipReferenceSpeed;
        if (reference < 0.01f)
        {
            return;
        }

        Vector3 velocity = Agent.velocity;
        velocity.y = 0f;

        float multiplier = Mathf.Clamp(velocity.magnitude / reference, animSpeedRange.x, animSpeedRange.y);
        anim.SetFloat(AnimSpeed, multiplier);
    }

    private bool isDead;
    public bool IsDead => isDead;

    public void Die()
    {
        isDead = true;

        if (enemyAudio != null)
            enemyAudio.StopFootstep();
    }
    private void HandleFootstepState(bool playerSeen)
    {
        if (Agent == null)
            return;

        // Không di chuyển
        if (Agent.velocity.sqrMagnitude < 0.05f)
        {
            isWalking = false;
            isRunning = false;

            if (enemyAudio != null)
                enemyAudio.StopFootstep();

            return;
        }

        if (enemyStyle == EnemyStyle.Zombie)
        {
            {
                isWalking = anim.GetBool(AnimIsWalking);
                isRunning = anim.GetBool(AnimIsChasing);
            }
        }
        else
        {
            // Gunner
            if (Agent.speed >= chaseSpeed - 0.1f)
            {
                isRunning = true;
                isWalking = false;
            }
            else
            {
                isWalking = true;
                isRunning = false;
            }
        }
        if (enemyAudio == null)
            return;

        if (!isWalking && !isRunning)
        {
            enemyAudio.StopFootstep();
        }
        else if (isWalking)
        {
            enemyAudio.StartWalkLoop();
        }
        else if (isRunning)
        {
            enemyAudio.StartRunLoop();
        }
    }

    private void ResetAnimationBools()
    {
        if (anim == null)
        {
            return;
        }

        animIsWalking = false;
        animIsChasing = false;
        animIsAttacking = false;

        anim.SetBool(AnimIsWalking, false);
        anim.SetBool(AnimIsChasing, false);
        anim.SetBool(AnimIsAttacking, false);
        anim.SetFloat(AnimMoveX, 0f);
        anim.SetFloat(AnimMoveY, 0f);
    }

    private IEnumerator ResetAttackAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        attackAnimationPlaying = false;
        UpdateAnimationState(PlayerVisible);
    }

    public virtual void PerformAttack()
    {
        if (enemyStyle == EnemyStyle.Gunner)
        {
            if (!IsGunnerPathing())
            {
                attackAnimationPlaying = true;
                if (anim != null)
                {
                    SetAnimBool(AnimIsAttacking, true, ref animIsAttacking);
                }
                // keep attack animation visible for a short duration related to fireRate
                StartCoroutine(ResetAttackAnimation(Mathf.Max(0.25f, fireRate)));
            }
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
        if (enemyAudio != null)
        {
            enemyAudio.PlayGunnerShoot();
        }
        Bullet bullet = GlobalReferences.Instance.SpawnBullet(
            ResolveBulletPrefab(), gunBarrel.position, transform.rotation, bulletLifeTime);
        if (bullet == null)
        {
            return;
        }

        Vector3 shootDirection = (Player.transform.position - gunBarrel.transform.position).normalized;
        if (bullet.TryGetComponent(out Rigidbody bulletRb))
        {
            bulletRb.velocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40f;
        }
    }

    /// Trả prefab đạn đã gán trong Inspector, thiếu thì nạp một lần từ Resources và dùng chung cho mọi enemy.
    private Bullet ResolveBulletPrefab()
    {
        if (bulletPrefab != null)
        {
            return bulletPrefab;
        }

        if (fallbackBulletPrefab == null)
        {
            GameObject loaded = Resources.Load<GameObject>("Prefabs/Bullet");
            if (loaded != null)
            {
                loaded.TryGetComponent(out fallbackBulletPrefab);
            }
        }

        return fallbackBulletPrefab;
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
                SetAnimBool(AnimIsAttacking, true, ref animIsAttacking);
            }
            StartCoroutine(ResetAttackAnimation(meleeCooldown));

            PlayerHealth playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
            }
            if (enemyAudio != null)
            {
                enemyAudio.PlayAttack();
            }

            nextAttackTime = Time.time + meleeCooldown;
        }
    }
}
