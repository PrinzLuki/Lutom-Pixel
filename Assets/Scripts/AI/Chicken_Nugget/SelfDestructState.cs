using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructState : State<BaseAI>
{
    public static SelfDestructState instance;

    public SelfDestructState()
    {
        if (instance == null)
            instance = this;
    }

    public static SelfDestructState Instance
    {
        get
        {
            if (instance == null)
            {
                new SelfDestructState();
            }
            return instance;
        }
    }

    public override void Enter(BaseAI owner)
    {
    }

    public override void Exit(BaseAI owner)
    {
    }

    public override void Update(BaseAI owner)
    {
        RaycastHit2D hit = Physics2D.CircleCast(owner.transform.position, 2, Vector2.zero, 2, owner.playerLayer);
        if (hit.transform != null && Vector3.Distance(owner.transform.position, hit.transform.position) <= 2)
            owner.Attack(true);
    }
}
