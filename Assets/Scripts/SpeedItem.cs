using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : NetworkBehaviour, IInteractable
{
    public float speed = 10;
    public void Interact(PlayerStats playerStats)
    {
        playerStats.Speed += speed;
        NetworkServer.Destroy(gameObject);
    }
}
