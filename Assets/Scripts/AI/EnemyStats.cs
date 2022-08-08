using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    [SerializeField] float health = 10;
    [SerializeField] float attackDmg = 1;
    [SerializeField] float speed = 1;

    public float Speed { get => speed; set => speed = value; }
    public float AttackDmg { get => attackDmg; set => attackDmg = value; }

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
