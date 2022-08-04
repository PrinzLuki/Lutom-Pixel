using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public BulletScriptableObject bulletScriptable;

    private void Start()
    {
        Destroy(gameObject, bulletScriptable.timeUntilDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);
            Destroy(gameObject);
        }

        if (bulletScriptable.bouncy && bulletScriptable.bouncyMAT != null)
        {
            var bulletRigidbody = GetComponent<Rigidbody2D>();
            bulletRigidbody.sharedMaterial = bulletScriptable.bouncyMAT;
        }

        if (!bulletScriptable.living && other.gameObject.layer != 7 && other.gameObject.layer != 12) //Bullet + Platforms
            Destroy(gameObject);
        if (bulletScriptable.sticky)
        {
            transform.parent = other.transform;
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.GetComponent<Collider2D>());
            //stickyBullet = false;
        }
    }
}
