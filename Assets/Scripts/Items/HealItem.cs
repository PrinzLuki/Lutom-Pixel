using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{

    [ClientRpc]
    public override void RpcSetStat(GameObject player, float healingValue, ItemType type)
    {
        var playerStats = player.GetComponent<PlayerStats>();
        playerStats.GetHealth(healingValue);
    }

}
