using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatformState : State<EnemyAI>
{
    public static JumpPlatformState instance;

    public JumpPlatformState()
    {
        if (instance == null)
            instance = this;
    }

    public static JumpPlatformState Instance
    {
        get
        {
            if(instance == null)
            {
                new JumpPlatformState();
            }
            return instance;
        }
    }

    float jumptime = 0.5f;


    public override void Enter(EnemyAI owner)
    {
        Debug.Log("Enter jumpPlatform");
        jumptime = 0.5f;
    }

    public override void Exit(EnemyAI owner)
    {
        Debug.Log("Exit Attack");

    }

    public override void Update(EnemyAI owner)
    {
        Debug.Log("Update JumpPlatform");
        JumpPlatforms(owner);
    }

    void JumpPlatforms(EnemyAI owner)
    {
        jumptime -= Time.deltaTime;
        owner.transform.Translate(owner.patroulingDirection * Time.deltaTime * owner.Stats.Speed);

        owner.GetComponent<Rigidbody2D>().velocity = Vector2.up * 5;
        if (jumptime <= 0)
            owner.stateMachine.ChangeState(PatroulState.Instance);
    }

}
