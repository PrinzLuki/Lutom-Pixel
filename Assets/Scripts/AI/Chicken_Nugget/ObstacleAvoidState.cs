using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidState : State<BaseAI>
{
    public static ObstacleAvoidState instance;

    public ObstacleAvoidState()
    {
        if (instance == null)
            instance = this;
    }

    public static ObstacleAvoidState Instance
    {
        get
        {
            if (instance == null)
            {
                new ObstacleAvoidState();
            }
            return instance;
        }
    }

    public override void Enter(BaseAI owner)
    {
        Debug.Log("Jump");
    }

    public override void Exit(BaseAI owner)
    {
        Debug.Log("Exit Jump");
    }

    public override void Update(BaseAI owner)
    {
        if (!owner.ObstacleAvoidance(owner.transform.position, owner.offsetObstAvoid, owner.rb2D.velocity, owner.distance, owner.GroundLayer))
            owner.stateMachine.ChangeState(WalkingState.Instance);
    }
}
