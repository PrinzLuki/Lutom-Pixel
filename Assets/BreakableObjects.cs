using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour, IDamageable
{

    public float health;
    public Animator animator;

     public void GetDamage(float dmg)
    {

        health -= dmg;

        if (health <= 0)
        {
            health = 0;

            animator.enabled = true;
            Destroy(gameObject, animator.runtimeAnimatorController.animationClips[0].length);
        }

    }
}
