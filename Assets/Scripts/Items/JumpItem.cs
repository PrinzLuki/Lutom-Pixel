using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpItem : Item
{

    [ClientRpc]
    public override void RpcSetStat(GameObject player, float jumpValue, ItemType type)
    {
        var playerStats = player.GetComponent<PlayerStats>();
        if (playerStats.currentItemType != type)
        {
            playerStats.ResetStats();
        }
        playerStats.JumpPower += jumpValue;
        if (playerStats.JumpPower > playerStats.MaxJumpPower) playerStats.JumpPower = playerStats.MaxJumpPower;
        playerStats.currentItemType = type;
    }

}
