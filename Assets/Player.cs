using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player : NetworkBehaviour
{
    //[SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    //private bool isPartyOwner = false;
    //[SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    //private string displayName;

    //public string GetDisplayName()
    //{
    //    return displayName;
    //}
    //public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    //public static event Action ClientOnInfoUpdated;
    
    //public bool GetIsPartyOwner()
    //{
    //    return isPartyOwner;
    //}

    //public override void OnStartClient()
    //{
    //    base.OnStartClient();
    //    if (NetworkServer.active) return;

    //    DontDestroyOnLoad(this);

    //    //((NetworkManagerLutom)NetworkManager.singleton).playerList.Add(this);
    //    Debug.Log(((NetworkManagerLutom)NetworkManager.singleton).playerList.Count);
    //}

    //public override void OnStopClient()
    //{
    //    ClientOnInfoUpdated?.Invoke();

    //    if (!isClientOnly) return;
    //    //((NetworkManagerLutom)NetworkManager.singleton).playerList.Remove(this);
    //}


    //public override void OnStartServer()
    //{
    //    DontDestroyOnLoad(this);
    //}

    //[Server]
    //public void SetPartyOwner(bool state)
    //{
    //    isPartyOwner = state;
    //}

    //[Server]
    //public void SetDisplayName(string dispName)
    //{
    //    this.displayName = dispName;
    //}


    //[Command]
    //public void CmdStartGame()
    //{
    //    if(!isPartyOwner) return;

    //    ((NetworkManagerLutom)NetworkManager.singleton).StartGame();
    //}

    //private void ClientHandleDisplayNameUpdated(string oldName, string newName)
    //{
    //    ClientOnInfoUpdated?.Invoke();
    //}

    //private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    //{
    //    if(!hasAuthority) return;

    //    AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    //}

}
