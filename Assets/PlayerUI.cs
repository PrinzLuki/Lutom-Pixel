using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] GameObject ownerHealth;

    [SerializeField] GameObject playerUIObj;

    [SerializeField] List<GameObject> otherPlayerHealths;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!hasAuthority)
        {
            playerUIObj.SetActive(false);
        }
        ownerHealth.SetActive(true);
        CmdSyncHealthBarCount();
    }

    [Command(requiresAuthority = false)]
    void CmdSyncHealthBarCount()
    {
        Debug.Log(NetworkManager.singleton.numPlayers - 1);
        for(int i = 0; i < NetworkManager.singleton.numPlayers - 1; i++)
        {
            otherPlayerHealths[i].SetActive(true);
        }
        RpcSyncHealthBarCout(NetworkManager.singleton.numPlayers - 1);
    }

    [ClientRpc]
    void RpcSyncHealthBarCout(int num)
    {
        for (int i = 0; i < num; i++)
        {
            otherPlayerHealths[i].SetActive(true);
        }
    }


}
