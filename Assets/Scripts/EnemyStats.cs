using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : NetworkBehaviour, IDamageable
{
    public float health = 10;

    public void GetDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Debug.Log("Enemy Died");
            //Destroy(gameObject);
            NetworkServer.Destroy(gameObject);
        }
    }
}
