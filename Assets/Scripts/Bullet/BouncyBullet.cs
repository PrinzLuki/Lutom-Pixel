using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBullet : Bullet
{

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IDamageable>() == null) return;

        other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);
    }

}




