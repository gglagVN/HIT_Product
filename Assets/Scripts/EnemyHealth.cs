using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;
    private Animator anim;
    public NavMeshAgent agent;
    private bool isDead = false;
    public EnemyAudio enemyAudio;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        if (enemy.enemyStyle == Enemy.EnemyStyle.Zombie)
            enemyAudio.PlayHurt();
        else if (enemy.enemyStyle == Enemy.EnemyStyle.Gunner)
            enemyAudio.PlayGunnerHurt();
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            agent.isStopped = true;
            anim.SetTrigger("DAMAGE");
            StartCoroutine(delay());
        }
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }

    void Die()
    {

        // Dừng NavMeshAgent
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        if (enemy.enemyStyle == Enemy.EnemyStyle.Zombie)
            enemyAudio.PlayDeath();
        else if (enemy.enemyStyle == Enemy.EnemyStyle.Gunner)
            enemyAudio.PlayGunnerDeath();
        // Tắt AI
        if (enemy != null)
        {
            enemy.Die();
        }
        if (enemy != null)
        {
            enemy.enabled = false;
        }

        StateMachine sm = GetComponent<StateMachine>();
        if (sm != null)
        {
            sm.enabled = false;
        }

        // Chọn animation chết
        if (Random.Range(0, 2) == 0)
        {
            anim.SetTrigger("DIE1");
        }
        else
        {
            anim.SetTrigger("DIE2");
        }

        // Tắt collider để không va chạm nữa
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        isDead = true;
    }
}