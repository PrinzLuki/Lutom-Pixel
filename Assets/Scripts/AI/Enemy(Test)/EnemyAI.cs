using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyAI : NetworkBehaviour
{
    Rigidbody2D enemyRb;
    public float jumpPower;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float percentDirectionChanging = 60;
    public Transform target;
    public bool isChasing;
    //Patroluing
    public float jumpTimer = 5;
    float directionLenght;
    public Vector2 patroulingDirection = Vector2.zero;
    public SpriteRenderer enemySprite;

    EnemyStats stats;
    public StateMachine<EnemyAI> stateMachine { get; set; }
    public EnemyStats Stats { get => stats; set => stats = value; }

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        enemyRb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine<EnemyAI>(this);
        stateMachine.ChangeState(PatroulState.Instance);
    }


    private void Update()
    {
        stateMachine.Update();
        ChaseDetection();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            var vel = target.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    //Flipping the enemySprite
    [Command(requiresAuthority = false)]
    public void FlipSprite(bool isflipped)
    {
        enemySprite.flipX = isflipped;
        FlipRpcSprite(isflipped);
    }

    [ClientRpc]
    void FlipRpcSprite(bool isflipped)
    {
        enemySprite.flipX = isflipped;
    }

    public void ChaseDetection()
    {
        Collider2D coll = Physics2D.OverlapCircle(transform.position, 4, playerLayer);
        if(coll != null)
        {
            target = coll.transform;
            if(!isChasing)
            {
                isChasing = true;
                GetComponent<Collider2D>().enabled = false;
                enemyRb.simulated = false;
            }
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
            enemyRb.simulated = true;
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.4f), patroulingDirection * 0.45f);
        Gizmos.DrawRay(transform.position + (Vector3.down * 0.45f) , patroulingDirection * 0.45f);
        Gizmos.DrawRay(transform.position + (Vector3.down * 0.3f), Vector2.up * 2f);
    }
}
