using UnityEngine;
using UnityEngine.AI;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;
    private float nextMoveTime;

    public override void Enter()
    {
        searchTimer = 0f;
        moveTimer = 0f;
        nextMoveTime = Random.Range(3f, 5f);

        enemy.Agent.stoppingDistance = 0f;
        enemy.Agent.SetDestination(enemy.LastKnownPos);
    }

    public override void Perform()
    {
        if (enemy.PlayerVisible)
        {
            stateMachine.ChangeState(stateMachine.AttackStateInstance);
            return;
        }

        if (!enemy.Agent.pathPending &&
            enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer >= nextMoveTime)
            {
                MoveToRandomPoint();

                moveTimer = 0f;
                nextMoveTime = Random.Range(3f, 5f);
            }

            if (searchTimer >= 10f)
            {
                stateMachine.ChangeState(stateMachine.PatrolStateInstance);
            }
        }
    }

    private void MoveToRandomPoint()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * 10f + enemy.transform.position;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(
                randomDirection,
                out hit,
                10f,
                NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }
    }

    public override void Exit()
    {

    }
}