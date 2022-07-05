using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyAI>
{
    public static AttackState instance;

    public AttackState()
    {
        if (instance == null)
            instance = this;
    }

    public static AttackState Instance
    {
        get
        {
            if(instance == null)
            {
                new AttackState();
            }
            return instance;
        }
    }


    public override void Enter(EnemyAI owner)
    {
        Debug.Log("Enter Attack");
    }

    public override void Exit(EnemyAI owner)
    {
        Debug.Log("Exit Attack");

    }

    public override void Update(EnemyAI owner)
    {
        Debug.Log("Update Attack");

    }
}
