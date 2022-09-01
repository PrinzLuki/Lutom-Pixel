using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State<BaseAI>
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

    public override void Enter(BaseAI owner)
    {
        ((BatAI)owner).agent.speed = owner.stats.ChaseSpeed;
    }

    public override void Exit(BaseAI owner)
    {
        ((BatAI)owner).agent.speed = owner.stats.Speed;
    }

    public override void Update(BaseAI owner)
    {
        owner.Walking();

        owner.Attack(true);
    }
}
