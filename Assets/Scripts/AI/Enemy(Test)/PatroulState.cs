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

        Debug.Log("Enter Patrouling");
        RandomDirection(owner);
    }

    public override void Exit(EnemyAI owner)
    {
        Debug.Log("Exit Patrouling");
    }

    public override void Update(EnemyAI owner)
    {
        owner.transform.Translate(owner.patroulingDirection * Time.deltaTime * owner.speed);
        ObstacleAvoidance(owner);

        RandomDirectionChange(owner, owner.percentDirectionChanging);
        Debug.Log("Update Patrouling");
    }

    //Change Direction if obstacle is in front
    void ObstacleAvoidance(EnemyAI owner)
    {
        Debug.Log("Obstacle Avoidance");
        if (Physics2D.Raycast(owner.transform.position, owner.patroulingDirection * 0.5f, 0.5f, owner.obstacleLayer))
        {
            Debug.Log("hit");
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
            owner.FlipSprite(false);
        }
        else
        {
            owner.patroulingDirection = Vector2.left;
            owner.FlipSprite(true);
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
            owner.FlipSprite(false);
            owner.patroulingDirection = Vector2.right;
        }
        else
        {
            owner.FlipSprite(true);
            owner.patroulingDirection = Vector2.left;
        }
    }


}
