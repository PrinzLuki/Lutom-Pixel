using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBullet : Bullet, IExplosion
{

    public float timer;
    public float explosionTimer;
    public float fieldOfImpact;
    public float force;
    public LayerMask explodeLayer;
    public bool readyToExplode;

    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, explodeLayer);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force , ForceMode2D.Impulse);
            if (obj.GetComponent<IDamageable>() != null)
            {
                obj.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage);
            }
        }

        Destroy(gameObject);

    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        readyToExplode = true;
    }

    private void Update()
    {
        if (!readyToExplode) return;

        timer += Time.deltaTime;
        if (timer >= explosionTimer)
        {
            timer = 0;
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}




