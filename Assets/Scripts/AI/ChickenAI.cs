using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChickenAI : BaseAI
{
    float idleTime = 0;
    public float maxIdleTime = 5;
    float dirChangeTimer = 2;
    public bool isBoom;

    public override void Start()
    {
        base.Start();
        stateMachine.ChangeState(IdleState.Instance);
        walkDirection = Random.Range(0, 100) < 50 ? Vector2.right : Vector2.left;
        dirChangeTimer = Random.Range(2f, 6f);
    }

    public override void Update()
    {
        base.Update();
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
        bool flip;

        if (ObstacleAvoidance(transform.position, offsetObstAvoid, walkDirection, distance, GroundLayer))
        {
            stateMachine.ChangeState(ObstacleAvoidState.Instance);
        }


        if (walkDirection == Vector2.left)
        {
            FlipSprite(true, out flip);
        }
        else FlipSprite(false, out flip);

        if (flip)
            transform.Translate(Vector2.left * Time.deltaTime * stats.Speed);
        else
            transform.Translate(Vector3.right * Time.deltaTime * stats.Speed);


        dirChangeTimer -= Time.deltaTime;
        if (dirChangeTimer <= 0)
        {
            dirChangeTimer = Random.Range(2f, 6f);
            if (walkDirection == Vector2.right || walkDirection == Vector2.zero)
                walkDirection = Vector2.left;
            else
                walkDirection = Vector2.right;
        }
    }

    public override void Attack(bool isBoom)
    {
        if (isBoom)
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 2, playerLayer);
            if (coll != null && coll.GetComponent<IDamageable>() != null)
                StartCoroutine(Kaboom(this, coll.transform));
        }
    }

    IEnumerator Kaboom(BaseAI owner, Transform player)
    {
        owner.animator.SetTrigger("boom");
        yield return new WaitForSeconds(1);
        player.GetComponent<IDamageable>().GetDamage(stats.AttackDmg);
        NetworkServer.Destroy(this.gameObject);
    }
    void FlipSprite(bool isFlipped, out bool flip)
    {
        if (isFlipped) sprite.flipX = true;
        else sprite.flipX = false;

        flip = isFlipped;
    }
}
