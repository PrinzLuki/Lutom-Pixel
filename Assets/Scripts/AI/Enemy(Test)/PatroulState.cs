using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroulState : State<EnemyAI>
{
    public static PatroulState instance;

    public PatroulState()
    {
        if (instance == null)
            instance = this;
    }

    public static PatroulState Instance
    {
        get
        {
            if (instance == null)
            {
                new PatroulState();
            }
            return instance;
        }
    }

    public override void Enter(EnemyAI owner)
    {
        Debug.Log("Enter Patrouling");
    }

    public override void Exit(EnemyAI owner)
    {
        Debug.Log("Exit Patrouling");
    }

    public override void Update(EnemyAI owner)
    {
        owner.Patrouling();
        Debug.Log("Update Patrouling");
    }
}
