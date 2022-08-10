using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBullet : Bullet
{

    public override void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.GetComponent<IDamageable>() == null) return;

        //other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);
        //Destroy(gameObject);

        transform.parent = other.transform;
        Destroy(this.GetComponent<Rigidbody2D>());
        Destroy(this.GetComponent<Collider2D>());

    }
}




