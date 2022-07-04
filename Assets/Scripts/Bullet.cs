using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletScriptableObject bulletScriptable;


    private void Start()
    {
        Destroy(gameObject, bulletScriptable.timeUntilDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<IDamageable>() != null)
        {
            other.gameObject.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);
        }

        if (!bulletScriptable.living)
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
