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
                new AttackState();
            }
            return instance;
        }
    }

    public override void Enter(EnemyAI owner)
    {
    }

    public override void Exit(EnemyAI owner)
    {
    }

    public override void Update(EnemyAI owner)
    {
    }
}
