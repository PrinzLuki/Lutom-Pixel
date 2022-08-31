using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NetworkRoomManagerLutom : NetworkRoomManager
{

    public Gamemodetype gamemode;

    public MainMenu mainMenu;

    //[Server]
    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    //{
    //    base.OnServerAddPlayer(conn);
    //    GameManager.players.Add(conn.identity.GetComponent<PlayerStats>());
    //    Debug.Log("OnServerAddPlayer: " + conn.identity.name);
    //}

    public override void OnRoomServerPlayersReady()
    {
        if (mainMenu == null) return;
        mainMenu.ToggleStartGameButton(allPlayersReady);
    }

    public override void OnRoomServerPlayersNotReady()
    {
        if (mainMenu == null) return;

        mainMenu.ToggleStartGameButton(allPlayersReady);
    }


    public void SetLevelScene(string name)
    {
        GameplayScene = name;
    }

}
public enum Gamemodetype
{
    PVE,
    PVP,
    MAX

}
