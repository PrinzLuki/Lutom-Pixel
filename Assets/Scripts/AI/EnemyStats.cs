using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    [SerializeField] float health = 10;
    [SerializeField] float attackDmg = 1;
    [SerializeField] float speed = 1;
    [SerializeField] float chaseSpeed = 2;

    public float Speed { get => speed; }
    public float AttackDmg { get => attackDmg; }
    public float ChaseSpeed { get => chaseSpeed; }

    public void GetDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            //Destroy(gameObject);
            NetworkServer.Destroy(gameObject);
        }
    }
}
