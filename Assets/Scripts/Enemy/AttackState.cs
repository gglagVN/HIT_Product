using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private const float RepathInterval = 0.2f;

    private float moveTimer;
    private float losePlayerTimer;
    private float attackTimer;
    private float nextMoveDelay;
    private float repathTimer;

    public override void Enter()
    {
        moveTimer = 0f;
        losePlayerTimer = 0f;
        attackTimer = 0f;
        repathTimer = 0f;
        nextMoveDelay = Random.Range(3, 7);

        if (enemy != null)
        {
            enemy.ResetAttackAnimationState();
        }

        // Dừng cách player một quãng để không ủi vào người và đẩy họ xuyên tường
        if (enemy.Agent != null)
        {
            enemy.Agent.stoppingDistance = enemy.meleeRange * 0.75f;
        }
    }

    public override void Exit()
    {
        if (enemy.Agent != null)
        {
            enemy.Agent.stoppingDistance = 0f;
        }
    }

    public override void Perform()
    {
        if (enemy.PlayerVisible)
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;
            FacePlayer();

            if (attackTimer > enemy.fireRate)
            {
                enemy.PerformAttack();
                attackTimer = 0f;
            }

            if (enemy.enemyStyle == Enemy.EnemyStyle.Zombie)
            {
                // Bám sát vị trí hiện tại của player, nếu không stoppingDistance sẽ tính từ đích cũ
                repathTimer += Time.deltaTime;
                if (repathTimer >= RepathInterval)
                {
                    repathTimer = 0f;
                    enemy.Agent.SetDestination(enemy.Player.transform.position);
                }
            }
            else if (moveTimer > nextMoveDelay)
            {
                enemy.SetAttackDestination();
                moveTimer = 0f;
                nextMoveDelay = Random.Range(3, 7);
            }

            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.ChangeState(stateMachine.SearchStateInstance);
            }
        }
    }

    /// <summary>
    /// Xoay dần về phía player khi đang đứng yên, còn lúc đang di chuyển thì để NavMeshAgent tự lái
    /// để hướng nhìn luôn trùng hướng đi, tránh trượt ngang.
    /// </summary>
    private void FacePlayer()
    {
        if (enemy.Agent != null && enemy.Agent.velocity.sqrMagnitude > 0.25f)
        {
            return;
        }

        Vector3 direction = enemy.Player.transform.position - enemy.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        enemy.transform.rotation = Quaternion.RotateTowards(
            enemy.transform.rotation,
            Quaternion.LookRotation(direction),
            enemy.turnSpeed * Time.deltaTime);
    }
}
