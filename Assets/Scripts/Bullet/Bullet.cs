using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public BulletScriptableObject bulletScriptable;

    public virtual void Start()
    {
        Destroy(gameObject, bulletScriptable.timeUntilDestroyed);
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IDamageable>() == null) return;

        other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);
        Destroy(gameObject);

        //if (!bulletScriptable.living && other.gameObject.layer != 7 && other.gameObject.layer != 12) //Bullet + Platforms
        //    Destroy(gameObject);


    }

}
