using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkManagerLutom : NetworkManager
{
    //public static event Action ClientOnConnected;
    //public static event Action ClientOnDisconnected;

    //private bool isGameInProgress = false;
    //public List<RoomPlayer> playerList { get; set; } = new();

    //#region Server
    //[Server]
    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    //{
    //    base.OnServerAddPlayer(conn);
    //    RoomPlayer player = conn.identity.GetComponent<RoomPlayer>();
    //    playerList.Add(player);
    //    player.gameObject.SetActive(false);
    //    //player.SetPartyOwner(playerList.Count == 1);
    //    Debug.Log(playerList.Count);

    //}

    //public override void OnServerDisconnect(NetworkConnectionToClient conn)
    //{
    //    playerList.Remove(conn.identity.GetComponent<RoomPlayer>());

    //    base.OnServerDisconnect(conn);

    //}

    //public override void OnStopServer()
    //{
    //    base.OnStopServer();
    //    playerList.Clear();
    //    isGameInProgress = false;
    //}

    //public void StartGame()
    //{
    //    if (playerList.Count < 2) return;

    //    isGameInProgress = true;

    //    ServerChangeScene("Snow_Map");
    //}

    //public override void OnServerConnect(NetworkConnectionToClient conn)
    //{
    //    base.OnServerConnect(conn);

    //    if (!isGameInProgress) return;

    //    conn.Disconnect();
    //}

    //public override void OnServerChangeScene(string newSceneName)
    //{
    //    base.OnServerChangeScene(newSceneName);
    //}

    //#endregion

    #region Client

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        //ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        //ClientOnDisconnected?.Invoke();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        //playerList.Clear();
    }

    #endregion



}
