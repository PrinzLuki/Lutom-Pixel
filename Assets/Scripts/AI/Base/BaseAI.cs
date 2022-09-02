using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class BaseAI : NetworkBehaviour
{
    public StateMachine<BaseAI> stateMachine { get; set; }

    public Rigidbody2D rb2D;

    public EnemyStats stats;

    public List<DetectionCombination> targetDetectionDirectionList;

    public Vector2 targetDirection;
    public Vector2 walkDirection = Vector2.right;

    public LayerMask playerLayer;
    public LayerMask GroundLayer;

    public SpriteRenderer sprite;

    public Animator animator;

    public Vector3 offsetObstAvoid;
    public float distance;


    public virtual void Start()
    {
        stateMachine = new StateMachine<BaseAI>(this);
        rb2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public abstract void Idle();
    public abstract void Walking();
    public abstract void Attack(bool isBoom);

    public virtual void Update()
    {
        stateMachine.Update();
    }

    public virtual void EnemyDetection()
    {
        for (int i = 0; i < targetDetectionDirectionList.Count; i++)
        {
            if(targetDetectionDirectionList[i] != null)
            {
                if (Physics2D.Raycast(this.transform.position, targetDetectionDirectionList[i].detectionDirection, 8, playerLayer))
                {
                    //Attack
                    stateMachine.ChangeState(SelfDestructState.Instance);
                    targetDirection = targetDetectionDirectionList[i].detectionDirection;
                    FlipSprite(targetDetectionDirectionList[i].flipType);
                    rb2D.AddForce(targetDirection * stats.ChaseSpeed, ForceMode2D.Impulse);

                }else targetDirection = Vector2.zero;
            }
        }
    }

    public virtual void FlipSprite(ESpriteFlip flip)
    {
        switch (flip)
        {
            case ESpriteFlip.FlipLeft:
                sprite.flipX = true;
                break;
            case ESpriteFlip.FlipRight:
                sprite.flipX = false;
                break;
            default:
                break;
        }
    }

    public virtual bool ObstacleAvoidance(Vector3 from, Vector3 offset, Vector3 direction, float distance, LayerMask layer)
    {
        if(Physics2D.Raycast(from + offset, direction * distance, distance, layer))
        {
            //rb2D.velocity = Vector2.up * stats.JumpPower;
            rb2D.AddForce(Vector2.up * stats.JumpPower * 10, ForceMode2D.Impulse);
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < targetDetectionDirectionList.Count; i++)
        {
            Gizmos.DrawRay(this.transform.position, targetDetectionDirectionList[i].detectionDirection);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + offsetObstAvoid, walkDirection * distance);
    }
}

public enum ESpriteFlip { FlipLeft, FlipRight, FlipTop, FlipBottom }

[System.Serializable]
public class DetectionCombination
{
    public Vector2 detectionDirection;
    public ESpriteFlip flipType;
}
