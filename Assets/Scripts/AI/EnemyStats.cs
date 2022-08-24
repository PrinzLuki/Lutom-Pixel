using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    //public SpawnManager spawnManager;

    [SerializeField] float health = 10;
    [SerializeField] float attackDmg = 1;
    [SerializeField] float speed = 1;
    [SerializeField] float chaseSpeed = 2;
    [SerializeField] float jumpPower = 2;

    public static Action OnKill;

    bool isDead;

    public float Speed { get => speed; }
    public float AttackDmg { get => attackDmg; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float JumpPower { get => jumpPower; }
    public float Health { get => health; }

    public void GetDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                OnKill?.Invoke();
            }

            NetworkServer.Destroy(gameObject);
        }
    }
}
