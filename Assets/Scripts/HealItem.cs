using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour, IHealabe
{
    public float healValue = 2;

    public void GetHealth(PlayerStats playerHealth)
    {
        playerHealth.Health += healValue;
        if (playerHealth.Health >= playerHealth.MaxHealth)
            playerHealth.Health = playerHealth.MaxHealth;

        Destroy(gameObject);
    }
}
