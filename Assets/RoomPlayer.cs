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

        ChangeReadyButton();
    }

    public void PlayerSetReady()
    {
        if (!hasAuthority) return;

        readyToBegin = !readyToBegin;
        ChangeReadyButton();
        CmdToggleCheckDisplay(readyToBegin);
        CmdChangeReadyState(readyToBegin);
        Debug.Log(readyToBegin);
    }

    [Command]
    public void CmdToggleCheckDisplay(bool value)
    {
        RpcToggleCheckDisplay(value);
    }

    [ClientRpc]
    public void RpcToggleCheckDisplay(bool value)
    {
        if (menu.playerNamesDisplay[index].text == displayPlayerName)
        {
            menu.playerChecksDisplay[index].SetActive(value);
        }
    }

    public void ChangeReadyButton()
    {
        if (!readyToBegin)
        {
            menu.readyPlayerButton.image.sprite = menu.readyImg;
            menu.readyPlayerButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
        }
        else
        {
            menu.readyPlayerButton.image.sprite = menu.notReadyImg;
            menu.readyPlayerButton.GetComponentInChildren<TextMeshProUGUI>().text = "Not Ready";
        }
    }

    public override void OnClientEnterRoom()
    {
        if (hasAuthority)
        {
            CmdSetName(menu.playerName);
        }
        //Debug.Log(index);

        menu.startGameButton.gameObject.SetActive(true);
        menu.readyPlayerButton.gameObject.SetActive(true);
        if (!isServer)
        {
            menu.startGameButton.gameObject.SetActive(false);
        }

    }

    public override void OnStopClient()
    {

        if (menu.playerNamesDisplay[index].text == displayPlayerName)
        {
            menu.playerNamesDisplay[index].text = "Waiting For Players...";
        }

    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        if (menu.playerNamesDisplay[oldIndex].text == displayPlayerName)
        {
            menu.playerNamesDisplay[oldIndex].text = "Waiting For Players...";
        }
        menu.playerNamesDisplay[newIndex].text = displayPlayerName;
    }

    public void OnPlayerNameChange(string oldName, string newName)
    {
        playerName = newName;
        gameObject.name = newName;
        menu.playerNamesDisplay[index].text = newName;
    }

    [Command]
    public void CmdSetName(string name)
    {
        displayPlayerName = name;
    }
}
