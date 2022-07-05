using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private bool showGizmos;

    public float Health { get => health; set => health = value; }
    public float MaxHealth { get => maxHealth; }

    private void Update()
    {
        IsInteracting();
    }

    private void IsInteracting()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<IHealabe>() != null)
            {
                if (InputManager.instance.Interact())
                {
                    collider.GetComponent<IHealabe>().GetHealth(this);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, interactionRadius);
        }
    }

}
