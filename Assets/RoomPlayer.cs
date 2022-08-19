using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField] MainMenu menu;
    [SerializeField] LobbyMenu lobbyMenu;

    [SyncVar(hook = nameof(OnPlayerNameChange))] public string displayPlayerName;

    private void Awake()
    {
        menu = FindObjectOfType<MainMenu>();
        lobbyMenu = FindObjectOfType<LobbyMenu>();
    }

    public override void OnClientEnterRoom()
    {
        if (hasAuthority)
        {
            CmdSetName(menu.playerName);
        }
        //Debug.Log(index);

        if (isServer)
        {
            lobbyMenu.startGameButton.GetComponentInChildren<TMP_Text>().text = "Start Game";
        }
    }

    public override void OnStopClient()
    {
        if (lobbyMenu != null)
        {
            if (lobbyMenu.playerNamesDisplay[index].text == displayPlayerName)
            {
                if (isClientOnly)
                {
                    menu.playMenuDisplay.SetActive(true);
                    menu.lobbyParentDisplay.SetActive(false);
                }
                lobbyMenu.playerNamesDisplay[index].text = "Waiting For Players...";
            }
        }
    }


    public override void IndexChanged(int oldIndex, int newIndex)
    {
        if (lobbyMenu.playerNamesDisplay[oldIndex].text == displayPlayerName)
        {
            lobbyMenu.playerNamesDisplay[oldIndex].text = "Waiting For Players...";
        }
        lobbyMenu.playerNamesDisplay[newIndex].text = displayPlayerName;
    }

    public void OnPlayerNameChange(string oldName, string newName)
    {
        playerName = newName;
        gameObject.name = newName;
        lobbyMenu.playerNamesDisplay[index].text = newName;
    }

    [Command]
    public void CmdSetName(string name)
    {
        displayPlayerName = name;
    }
}
