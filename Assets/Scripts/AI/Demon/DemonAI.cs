using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAI : BaseAI
{
    [Header("DemonAI")]
    float attackDelay;
    float walkTimer;
    float attackRadius = 1;
    public float frequency = 20f;
    public float magnitude = 0.5f;

    public bool isDirectionRight = true;

    Vector3 position;

    public override void Start()
    {
        base.Start();
        position = transform.position;
        magnitude = Random.Range(0.2f, 1.5f);
        walkTimer = Random.Range(2f, 10f);
    }

    public override void Update()
    {
        attackDelay -= Time.deltaTime;
        base.Update();
        //sprite.flipX = ObstacleAvoidance(transform.position, offsetObstAvoid, walkDirection, distance, GroundLayer);
        Walking();
        Attack(false);
    }

    public override bool ObstacleAvoidance(Vector3 from, Vector3 offset, Vector3 direction, float distance, LayerMask layer)
    {
        if (Physics2D.Raycast(from + offset, direction * distance, distance, layer))
        {
            Debug.Log("Hittet ground");
            return true;
        }
        return false;
    }

    public override void Attack(bool isBoom)
    {
        Collider2D coll = Physics2D.OverlapCircle(position, attackRadius, playerLayer);
        if (coll != null)
        {
            if (attackDelay <= 0)
            {
                attackDelay = 1;
                if (coll.GetComponent<IDamageable>() != null)
                {
                    coll.GetComponent<IDamageable>().GetDamage(stats.AttackDmg);
                }
            }
        }
    }

    public override void Idle()
    {
        Debug.LogWarning("No Idle in Demon");
    }

    public override void Walking()
    {
        ChangeDirection();
        if (walkDirection == Vector2.right)
        {
            MoveRight();
        }
        else if (walkDirection == Vector2.left)
        {
            MoveLeft();
        }
    }

    public void MoveRight()
    {
        position += transform.right * Time.deltaTime * stats.Speed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        if (ObstacleAvoidance(transform.position, offsetObstAvoid, walkDirection, distance, GroundLayer))
        {
            walkDirection = Vector2.left;
            sprite.flipX = true;
        }
    }

    public void MoveLeft()
    {
        position -= transform.right * Time.deltaTime * stats.Speed;
        transform.position = position + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        if (ObstacleAvoidance(transform.position, offsetObstAvoid, walkDirection, distance, GroundLayer))
        {
            walkDirection = Vector2.right;
            sprite.flipX = false;
        }
    }

    void ChangeDirection()
    {
        walkTimer -= Time.deltaTime;
        if (walkTimer <= 0)
        {
            walkTimer = Random.Range(7f, 15f);
            walkDirection = walkDirection == Vector2.left ? Vector2.right : Vector2.left;

            if (walkDirection == Vector2.right)
                sprite.flipX = false;
            else if (walkDirection == Vector2.left)
                sprite.flipX = true;

            magnitude = Random.Range(0.2f, 2f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position + offsetObstAvoid, walkDirection * distance);
    }
}
