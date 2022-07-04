using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour, IInteractable
{
    public float speed = 10;

    public void Interact(PlayerMovement playerSpeed)
    {
        playerSpeed.PlayerSpeed += speed;
        Destroy(gameObject);
    }
}
