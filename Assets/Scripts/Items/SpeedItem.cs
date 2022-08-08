using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : Item
{

    [ClientRpc]
    public override void RpcSetStat(GameObject player, float speedValue, ItemType type)
    {
        var playerStats = player.GetComponent<PlayerStats>();
        if (playerStats.currentItemType != type)
        {
            playerStats.ResetStats();
        }
        playerStats.Speed += speedValue;
        if (playerStats.Speed > playerStats.MaxSpeed) playerStats.Speed = playerStats.MaxSpeed;
        playerStats.currentItemType = type;
    }

}
