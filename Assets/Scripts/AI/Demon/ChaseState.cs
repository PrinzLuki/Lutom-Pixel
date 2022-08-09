using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State<EnemyAI>
{
    public static ChaseState instance;

    public ChaseState()
    {
        if (instance == null)
            instance = this;
    }

    public static ChaseState Instance
    {
        get
        {
            if (instance == null)
            {
                new ChaseState();
            }
            return instance;
        }
    }

    public override void Enter(EnemyAI owner)
    {
        Debug.Log("Enter CHasing");
        owner.GetComponent<Rigidbody2D>().simulated = false;
        owner.GetComponent<Collider2D>().enabled = false;
    }

    public override void Exit(EnemyAI owner)
    {
        Debug.Log("Exit CHasing");
        owner.GetComponent<Rigidbody2D>().simulated = true;
        owner.GetComponent<Collider2D>().enabled = true;
    }

    public override void Update(EnemyAI owner)
    {
        Debug.Log(owner.target.name);
       
        if (owner.target == null)
            owner.stateMachine.ChangeState(PatroulState.Instance);
    }
}
