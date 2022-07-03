using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timeUntilDestroyed = 4f;
    public bool livingBullet;
    public bool stickyBullet;

    private void Start()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!livingBullet)
            Destroy(gameObject);
        if (stickyBullet)
        {
            transform.parent = collision.transform;
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.GetComponent<Collider2D>());
            stickyBullet = false;
        }
    }
}
