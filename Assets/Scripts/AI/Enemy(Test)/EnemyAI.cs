using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyAI : NetworkBehaviour
{
    Rigidbody2D enemyRb;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;
    public float percentDirectionChanging = 60;
    //Patroluing
    float patroulingTimer;
    float directionLenght;
    public Vector2 patroulingDirection = Vector2.zero;
    public SpriteRenderer enemySprite;

    EnemyStats stats;
    StateMachine<EnemyAI> stateMachine { get; set; }
    public EnemyStats Stats { get => stats; set => stats = value; }

    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        patroulingTimer = Random.Range(1f, 7f);
        enemyRb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine<EnemyAI>(this);
        stateMachine.ChangeState(DemonState.Instance);
    }


    private void Update()
    {
        stateMachine.Update();
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
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.4f), patroulingDirection * 0.8f);
        Gizmos.DrawRay(transform.position + (Vector3.down * 0.4f) , patroulingDirection * 0.6f);
        Gizmos.DrawRay(transform.position, Vector2.up * 5);
    }
}
