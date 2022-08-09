using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State<BaseAI>
{
    public static WalkingState instance;

    public WalkingState()
    {
        if (instance == null)
            instance = this;
    }

    public static WalkingState Instance
    {
        get
        {
            if (instance == null)
            {
                new WalkingState();
            }
            return instance;
        }
    }


    public override void Enter(BaseAI owner)
    {
        owner.animator.SetBool("isWalking", true);
        //dirChange = Random.Range(2f, 6f);
    }

    public override void Exit(BaseAI owner)
    {
        owner.animator.SetBool("isWalking", false);
    }

    public override void Update(BaseAI owner)
    {
        owner.Walking();
    }
}
