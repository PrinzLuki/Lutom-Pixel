using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : MonoBehaviour, IInteractable
{
    public float speed = 10;

    public void Interact(PlayerStats playerStats)
    {
        playerStats.Speed += speed;
        Destroy(gameObject);
    }
}
