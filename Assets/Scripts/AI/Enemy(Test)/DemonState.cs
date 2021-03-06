using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonState : State<EnemyAI>
{
    #region Singleton

    public static DemonState instance;
    public DemonState()
    {
        if (instance == null)
            instance = this;
    }

    public static DemonState Instance
    {
        get
        {
            if (instance == null)
            {
                new DemonState();
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
        FlipSprite(owner);
        Attack(owner);
    }

    //Change Direction if obstacle is in front
    void ObstacleAvoidance(EnemyAI owner)
    {
        if (Physics2D.Raycast(owner.transform.position, owner.patroulingDirection * 0.5f, 0.5f, owner.obstacleLayer))
        {
            ChangeDirection(owner);
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
        if(owner.patroulingDirection == Vector2.left)
        {
            owner.FlipSprite(true);
        }
        else if(owner.patroulingDirection == Vector2.right)
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
