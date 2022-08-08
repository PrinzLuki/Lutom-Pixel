using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : NetworkBehaviour, IInteractable
{
    public int value;
    public float despawnTime = 10f;
    public ItemType type;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    public virtual void Interact(GameObject player)
    {
        CmdSetStat(player, value, type);
        CmdDeleteGameObject(gameObject);
    }


    [Command(requiresAuthority = false)]
    public virtual void CmdSetStat(GameObject player, float value, ItemType type)
    {
        RpcSetStat(player, value, type);
    }

    [ClientRpc]
    public virtual void RpcSetStat(GameObject player, float value, ItemType type)
    {
        //Set Method / Value
        Debug.LogWarning("Set Value - Inherit this or use another script");
    }



    [Command(requiresAuthority = false)]
    public virtual void CmdDeleteGameObject(GameObject trg)
    {
        RpcDeleteGameObject(trg);
    }

    [ClientRpc]
    public virtual void RpcDeleteGameObject(GameObject trg)
    {
        Destroy(trg);
    }


}
