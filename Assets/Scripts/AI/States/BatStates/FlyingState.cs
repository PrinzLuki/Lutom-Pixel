using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : State<BaseAI>
{
    public static FlyingState instance;

    public FlyingState()
    {
        if (instance == null)
            instance = this;
    }

    public static FlyingState Instance
    {
        get
        {
            if (instance == null)
            {
                new FlyingState();
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
        ((BatAI)owner).Walking();
    }
}
