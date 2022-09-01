using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet, IExplosion
{

    public float fieldOfImpact;
    public float force;
    public LayerMask explodeLayer;

    public void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, explodeLayer);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            if (obj.GetComponent<IDamageable>() != null)
            {
                obj.GetComponent<IDamageable>().GetDamage(bulletScriptable.damage, parent.gameObject);
            }
            
        }
        GetComponent<Animator>().enabled = true;

        Destroy(gameObject, GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }


    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (GameManager.instance.isPvE && other.gameObject.GetComponent<PlayerStats>() != null)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), other.gameObject.GetComponent<Collider2D>());
            return;
        }
        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}




