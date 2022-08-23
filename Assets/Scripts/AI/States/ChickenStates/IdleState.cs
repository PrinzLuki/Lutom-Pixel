using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<BaseAI>
{
    public static IdleState instance;

    public IdleState()
    {
        if (instance == null)
            instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (instance == null)
            {
                new IdleState();
            }
            return instance;
        }
    }

    public override void Enter(BaseAI owner)
    {
        owner.animator.SetBool("isIdle", true);
        owner.rb2D.velocity = Vector2.zero;
    }

    public override void Exit(BaseAI owner)
    {
        owner.animator.SetBool("isIdle", false);
    }

    public override void Update(BaseAI owner)
    {
        owner.Idle();
    }
}
