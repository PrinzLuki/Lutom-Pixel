using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBullet : Bullet
{

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (GameManager.instance.isPvE && other.gameObject.GetComponent<PlayerStats>() != null)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
        if (other.gameObject.GetComponent<IDamageable>() == null) return;

        bulletHit = !bulletHit;
        if (other.gameObject.GetComponent<IDamageable>() != null && !bulletHit)
            other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage, parent.gameObject);
    }

}




