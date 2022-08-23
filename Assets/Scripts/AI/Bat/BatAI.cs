using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class BatAI : BaseAI
{
    [Header("DemonAI")]
    public NavMeshAgent agent;
    float dirChangeTimer = 2;
    public float newPositionDistanceOffset;
    Vector3 targetPosition;
    public float detectionRadius;
    public float attackRadius;


    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = stats.Speed;
        dirChangeTimer = Random.Range(2f, 6f);
        stateMachine.ChangeState(FlyingState.Instance);
        SetTargetPosition();
    }

    public override void Update()
    {
        base.Update();
        EnemyDetection();
        FlipSprite();
    }

    public override void Attack(bool isBoom)
    {
        var coll = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (coll == null) return;
        if (coll.GetComponent<IDamageable>() != null)
            coll.GetComponent<IDamageable>().GetDamage(stats.AttackDmg);

    }

    public override void EnemyDetection()
    {
        var coll = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (coll == null) return;
        targetPosition = coll.transform.position;
        stateMachine.ChangeState(ChaseState.Instance);
    }

    public override void Idle()
    {
    }

    public override void Walking()
    {
        if (targetPosition == null) return;

        agent.destination = targetPosition;

        GetNewPosition();
    }

    void GetNewPosition()
    {
        dirChangeTimer -= Time.deltaTime;
        if (dirChangeTimer <= 0)
        {
            dirChangeTimer = Random.Range(2f, 6f);

            SetTargetPosition();
        }
    }

    void SetTargetPosition()
    {
        var agPos = agent.transform.position;
        targetPosition = new Vector3(Random.Range(agPos.x - newPositionDistanceOffset, agPos.x + newPositionDistanceOffset),
            Random.Range(agPos.y - newPositionDistanceOffset, agPos.y + newPositionDistanceOffset),
            0);
    }

    void FlipSprite()
    {
        if(agent.velocity.x < 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
        else if(agent.velocity.x > 0 && sprite.flipX)
        {
            sprite.flipX = false;
        }

        CmdFlipSprite(sprite.flipX, this.gameObject);
    }

    //Server
    [Command(requiresAuthority = false)]
    void CmdFlipSprite(bool isFlipped, GameObject targetObj)
    {
        RpcFlipSprite(isFlipped, targetObj);
    }

    //Client
    [ClientRpc]
    void RpcFlipSprite(bool isFlipped, GameObject targetObj)
    {
        targetObj.GetComponent<SpriteRenderer>().flipX = isFlipped;
    }

}
