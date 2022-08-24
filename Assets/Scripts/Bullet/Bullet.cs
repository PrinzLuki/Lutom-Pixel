using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public BulletScriptableObject bulletScriptable;
    public Transform parent;

    public virtual void Start()
    {
        Destroy(gameObject, bulletScriptable.timeUntilDestroyed);
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IDamageable>() == null) return;

        other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);

        Destroy(gameObject);
    }

}
