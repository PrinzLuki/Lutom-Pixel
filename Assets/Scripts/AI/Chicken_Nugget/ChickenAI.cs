using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class ChickenAI : BaseAI
{
    float idleTime = 0;
    public float maxIdleTime = 5;
    float dirChangeTimer = 2;
    public bool isBoom;
    public float explosionRadial = 3;

    public float fieldOfImpact;
    public float force;
    public LayerMask explodeLayer;
    public NavMeshAgent agent;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.ChangeState(IdleState.Instance);
        walkDirection = Random.Range(0, 100) < 50 ? Vector2.right : Vector2.left;
        dirChangeTimer = Random.Range(2f, 6f);
        agent.speed = stats.Speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Update()
    {
        base.Update();
        FlipSprite();
        EnemyDetection();
    }

    float detectionRange = 10;
    public override void EnemyDetection()
    {
        var coll = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        Debug.Log(coll); 
        if (coll == null) return;

        if(coll.GetComponent<PlayerStats>() != null)
        {
            agent.speed = stats.ChaseSpeed;
            agent.destination = coll.transform.position;
            stateMachine.ChangeState(SelfDestructState.Instance);
        }
        else
            agent.speed = stats.Speed;
    }
    public override void Idle()
    {
        idleTime += Time.deltaTime;

        if (idleTime > maxIdleTime)
        {
            idleTime = 0;
            stateMachine.ChangeState(WalkingState.Instance);
        }
    }

    public override void Walking()
    {
        //bool flip;

        if (ObstacleAvoidance(transform.position, offsetObstAvoid, walkDirection, distance, GroundLayer))
        {
            stateMachine.ChangeState(ObstacleAvoidState.Instance);
        }


        //if (walkDirection == Vector2.left)
        //{
        //    FlipSprite(true, out flip);
        //}
        //else FlipSprite(false, out flip);

        //if (flip)
        //    transform.Translate(Vector2.left * Time.deltaTime * stats.Speed);
        //else
        //    transform.Translate(Vector3.right * Time.deltaTime * stats.Speed);


        dirChangeTimer -= Time.deltaTime;
        if (dirChangeTimer <= 0)
        {
            dirChangeTimer = Random.Range(2f, 6f);
            if (walkDirection == Vector2.right || walkDirection == Vector2.zero)
                walkDirection = Vector2.left;
            else
                walkDirection = Vector2.right;
        }
        //for (int i = 0; i < targetDetectionDirectionList.Count; i++)
        //{
        //    if (Physics2D.Raycast(this.transform.position, targetDetectionDirectionList[i].detectionDirection, 8, playerLayer))
        //    {
        //        //Attack
        //        stateMachine.ChangeState(SelfDestructState.Instance);
        //    }
        //}

    }

    public void IsBoomCheck()
    {
        if(Physics2D.OverlapCircle(transform.position, explosionRadial, playerLayer))
        {
            isBoom = true;
        }
    }

    public override void Attack(bool isBoom)
    {
        if (isBoom)
        {
            StartCoroutine(Kaboom(this));
        }
    }

    IEnumerator Kaboom(BaseAI owner)
    {
        owner.animator.SetTrigger("boom");
        yield return new WaitForSeconds(0.4f);
        Explode();
        NetworkServer.Destroy(this.gameObject);
    }


    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, explosionRadial, playerLayer);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            if (obj.GetComponent<IDamageable>() != null)
            {
                obj.GetComponent<IDamageable>().GetDamage(stats.AttackDmg, this.gameObject);
            }
        }
    }

    //void FlipSprite(bool isFlipped, out bool flip)
    //{
    //    if (isFlipped) sprite.flipX = true;
    //    else sprite.flipX = false;

    //    flip = isFlipped;
    //    CmdFlipSprite(isFlipped, this.gameObject);
    //}

    ////Server
    //[Command(requiresAuthority = false)]
    //void CmdFlipSprite(bool isFlipped, GameObject targetObj)
    //{
    //    RpcFlipSprite(isFlipped, targetObj);
    //}

    ////Client
    //[ClientRpc]
    //void RpcFlipSprite(bool isFlipped, GameObject targetObj)
    //{
    //    targetObj.GetComponent<SpriteRenderer>().flipX = isFlipped;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, explosionRadial);
    //}
    void FlipSprite()
    {
        if (agent.velocity.x < 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
        else if (agent.velocity.x > 0 && sprite.flipX)
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
