using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timeUntilDestroyed = 4f;
    public bool livingBullet;
    public bool stickyBullet;
    public float bulletDmg = 2;


    private void Start()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<IDamageable>() != null)
        {
            other.gameObject.GetComponent<IDamageable>().GetDamage(bulletDmg);
        }

        if (!livingBullet)
            Destroy(gameObject);
        if (stickyBullet)
        {
            transform.parent = other.transform;
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.GetComponent<Collider2D>());
            stickyBullet = false;
        }
    }
}
