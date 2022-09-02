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
        Debug.Log("Enter Debug");
    }

    public override void Exit(BaseAI owner)
    {
    }

    public override void Update(BaseAI owner)
    {
        ((ChickenAI)owner).IsBoomCheck();
        ((ChickenAI)owner).Attack(((ChickenAI)owner).isBoom);
        ((ChickenAI)owner).stateMachine.ChangeState(IdleState.Instance);
        //RaycastHit2D hit = Physics2D.CircleCast(owner.transform.position, ((ChickenAI)owner).explosionRadial, Vector2.zero, 2, owner.playerLayer);
        //if (hit.transform != null && Vector3.Distance(owner.transform.position, hit.transform.position) <= ((ChickenAI)owner).explosionRadial)
        //{
        //    Debug.Log("Distance explode: " + (Vector3.Distance(owner.transform.position, hit.transform.position) <= ((ChickenAI)owner).explosionRadial));
        //    ((ChickenAI)owner).isBoom = true;
        //    owner.Attack(((ChickenAI)owner).isBoom);
        //}
    }
}
