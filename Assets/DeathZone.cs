using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeathZone : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            if (other.gameObject.GetComponent<EnemyAI>() != null)
                other.gameObject.GetComponentInParent<SpawnManager>().allAliveEnemies.Remove(other.gameObject);

            other.gameObject.GetComponent<IDamageable>().GetDamage(10000);
        }
    }

}
