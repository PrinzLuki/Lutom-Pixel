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

    private void OnDrawGizmos2D()
    {
        Gizmos.color = Color.red;
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
}
