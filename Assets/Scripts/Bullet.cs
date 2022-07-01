using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float timeUntilDestroyed = 4f;
    public bool livingBullet;

    private void Start()
    {
        Destroy(gameObject, timeUntilDestroyed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!livingBullet)
            Destroy(gameObject);
    }
}
