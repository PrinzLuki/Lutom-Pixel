using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UI;

public class PlayerUI : NetworkBehaviour
{
    //public Image healthFill;

    //public PlayerStats stats;

    //private void Start()
    //{
    //    stats = GetComponent<PlayerStats>();
    //    stats.onHealthChanged.AddListener(CmdSetHealthFill);
    //}

    //[Command]
    //public void CmdSetHealthFill(float health, float maxHealth)
    //{
    //    RpcSetHealthFill(health, maxHealth);
    //}

    //[ClientRpc]
    //public void RpcSetHealthFill(float health, float maxHealth)
    //{
    //    healthFill.fillAmount = health / maxHealth;
    //}
}
