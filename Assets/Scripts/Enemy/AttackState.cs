using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float attackTimer;
    private float nextMoveDelay;

    public override void Enter()
    {
        moveTimer = 0f;
        losePlayerTimer = 0f;
        attackTimer = 0f;
        nextMoveDelay = Random.Range(3, 7);
    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (enemy.PlayerVisible)
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

            if (moveTimer > nextMoveDelay)
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
}
