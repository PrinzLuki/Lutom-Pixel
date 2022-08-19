using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class RoomPlayer : NetworkRoomPlayer
{
    [SerializeField] MainMenu menu;
    [SerializeField] LobbyMenu lobbyMenu;

    [SyncVar(hook = nameof(OnPlayerNameChange))] public string displayPlayerName;

    private void OnEnable()
    {
        menu.OnReadyPlayer += PlayerSetReady;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        menu.OnReadyPlayer -= PlayerSetReady;
    }

    private void Awake()
    {
        menu = FindObjectOfType<MainMenu>();
        lobbyMenu = FindObjectOfType<LobbyMenu>();
    }

    public void PlayerSetReady()
    {
        if (!hasAuthority) return;

        readyToBegin = !readyToBegin;
        Debug.Log(readyToBegin);
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
            lobbyMenu.startGameButton.gameObject.SetActive(true);
            lobbyMenu.readyPlayerButton.gameObject.SetActive(false);
        }
        else
        {
            lobbyMenu.readyPlayerButton.gameObject.SetActive(true);
            lobbyMenu.startGameButton.gameObject.SetActive(false);
        }
    }

    public override void OnStopClient()
    {
        if (lobbyMenu != null)
        {
            if (lobbyMenu.playerNamesDisplay[index].text == displayPlayerName)
            {
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
