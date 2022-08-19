using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NetworkRoomManagerLutom : NetworkRoomManager
{

    public MainMenu mainMenu;

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

    public void SetLevelScene(Scene scene)
    {
        GameplayScene = scene.name;
    }

}
