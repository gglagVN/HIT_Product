using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    public PatrolState patrolState;

    private Enemy owner;
    private PatrolState cachedPatrolState;
    private AttackState cachedAttackState;
    private SearchState cachedSearchState;

    public PatrolState PatrolStateInstance
    {
        get
        {
            if (cachedPatrolState == null)
            {
                cachedPatrolState = new PatrolState();
            }
            return cachedPatrolState;
        }
    }

    public AttackState AttackStateInstance
    {
        get
        {
            if (cachedAttackState == null)
            {
                cachedAttackState = new AttackState();
            }
            return cachedAttackState;
        }
    }

    public SearchState SearchStateInstance
    {
        get
        {
            if (cachedSearchState == null)
            {
                cachedSearchState = new SearchState();
            }
            return cachedSearchState;
        }
    }

    // Start is called before the first frame update
    public void Initialise()
    {
        ChangeState(PatrolStateInstance);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activeState != null)
            activeState.Perform();
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        activeState = newState;
        if (activeState != null)
        {
            if (owner == null)
            {
                owner = GetComponent<Enemy>();
            }
            activeState.stateMachine = this;
            activeState.enemy = owner;
            activeState.Enter();
        }
    }
}
