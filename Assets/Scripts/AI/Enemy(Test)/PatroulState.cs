using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatroulState : State<EnemyAI>
{
    #region Singleton

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

    #endregion

    public override void Enter(EnemyAI owner)
    {
        RandomDirection(owner);
    }

    public override void Exit(EnemyAI owner)
    {

    }

    public override void Update(EnemyAI owner)
    {
        owner.transform.Translate(owner.patroulingDirection * Time.deltaTime * owner.Stats.Speed);
        ObstacleAvoidance(owner);
        RandomDirectionChange(owner, owner.percentDirectionChanging);
        Attack(owner);
        FlipSprite(owner);
    }

    //Change Direction if obstacle is in front
    void ObstacleAvoidance(EnemyAI owner)
    {
        if (Physics2D.Raycast(owner.transform.position + (Vector3.down * 0.2f), Vector3.up * 2f, 2f, owner.platformLayer))
        {
            owner.jumpTimer -= Time.deltaTime;

            if(Random.Range(0, 100) < 10 && owner.jumpTimer <= 0)
            {
                owner.jumpTimer = 5;
                owner.stateMachine.ChangeState(JumpPlatformState.Instance);
            }
        }
        if (Physics2D.Raycast(owner.transform.position + (Vector3.up * 0.4f), owner.patroulingDirection * 0.45f, 0.45f, owner.groundLayer))
        {
            ChangeDirection(owner);
        }
        else if (Physics2D.Raycast(owner.transform.position + (Vector3.down * 0.4f), owner.patroulingDirection * 0.45f, 0.45f, owner.groundLayer))
        {
            Debug.Log("Jump");
            Jump(owner);
        }
    }

    //Sets Random Direction in the EnterState
    void RandomDirection(EnemyAI owner)
    {
        float percent = Random.Range(0f, 100f);
        if (percent > 50)
        {
            owner.patroulingDirection = Vector2.right;
        }
        else
        {
            owner.patroulingDirection = Vector2.left;
        }
    }

    //Randomly Calling ChangeDirection(EnemyAI owner)
    void RandomDirectionChange(EnemyAI owner, float percentChance)
    {
        float percent = Random.Range(0f, 100f);
        if (percent > percentChance)
        {
            ChangeDirection(owner);
        }
    }

    void Jump(EnemyAI owner)
    {
        var rb = owner.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector3.up * owner.jumpPower, ForceMode2D.Impulse);
    }


    //void JumpPlatform(EnemyAI owner)
    //{
    //    var rb = owner.GetComponent<Rigidbody2D>();
    //    rb.velocity = Vector2.up * 3;
    //}


    //Changes Direction
    void ChangeDirection(EnemyAI owner)
    {
        if (owner.patroulingDirection == Vector2.left)
        {
            owner.patroulingDirection = Vector2.right;
        }
        else
        {
            owner.patroulingDirection = Vector2.left;
        }
    }

    void FlipSprite(EnemyAI owner)
    {
        if (owner.patroulingDirection == Vector2.left)
        {
            owner.FlipSprite(true);
        }
        else if (owner.patroulingDirection == Vector2.right)
        {
            owner.FlipSprite(false);
        }
    }

    //OverlapBox For dealing out damage
    void Attack(EnemyAI owner)
    {
        var colls = Physics2D.OverlapBoxAll(owner.transform.position, Vector2.one, 1, owner.playerLayer);
        for (int i = 0; i < colls.Length; i++)
        {
            Debug.Log("Attack");
            if (colls[i].transform.GetComponent<IDamageable>() != null)
            {
                Debug.Log("GetDamage");
                colls[i].transform.GetComponent<IDamageable>().GetDamage(owner.GetComponent<EnemyStats>().AttackDmg);
            }
        }
    }


}
