using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float attackTimer;

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;
            Vector3 targetPos = enemy.Player.transform.position;
            targetPos.y = enemy.transform.position.y;

            enemy.transform.LookAt(targetPos);

            if (attackTimer > enemy.fireRate)
            {
                enemy.PerformAttack();
                attackTimer = 0f;
            }

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.SetAttackDestination();
                moveTimer = 0f;
            }

            enemy.LastKnownPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }
}
