using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour, IInteractable
{
    public float healValue = 2;

    public void Interact(PlayerStats playerStats)
    {
        playerStats.Health += healValue;
        if (playerStats.Health >= playerStats.MaxHealth)
            playerStats.Health = playerStats.MaxHealth;

        Destroy(gameObject);
    }


}
