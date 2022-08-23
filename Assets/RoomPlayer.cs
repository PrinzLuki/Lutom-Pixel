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
    }

    [Command(requiresAuthority = false)]
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
            menu.playerChecksDisplay[index].SetActive(false);
        }
        else
        {
            menu.readyPlayerButton.image.sprite = menu.notReadyImg;
            menu.readyPlayerButton.GetComponentInChildren<TextMeshProUGUI>().text = "Not Ready";
            menu.playerChecksDisplay[index].SetActive(true);

        }
    }


    public override void OnClientEnterRoom()
    {

        if (hasAuthority)
        {
            CmdSetName(menu.playerName);
        }

        menu.startGameButton.gameObject.SetActive(true);
        menu.readyPlayerButton.gameObject.SetActive(true);
        if (!isServer)
        {
            menu.startGameButton.gameObject.SetActive(false);
        }

        foreach (var check in menu.playerChecksDisplay)
        {
            check.SetActive(false);
        }


    }


    public override void OnStopClient()
    {
        if(menu == null)
        {
            Debug.Log("MainMenu not in Scene");
            return;
        }

        if (menu.playerNamesDisplay[index].text == displayPlayerName)
        {
            menu.playerNamesDisplay[index].text = "Waiting For Players...";
            menu.playerChecksDisplay[index].SetActive(false);
            //menu.sameNameIndex--;
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

        //for (int i = 0; i < menu.playerNamesDisplay.Length; i++)
        //{
        //    if (menu.playerNamesDisplay[i].text == name)
        //    {
        //        name = menu.playerNamesDisplay[index].text + "(" + menu.sameNameIndex + ")";
        //        displayPlayerName = name;
        //        menu.sameNameIndex++;
        //    }
        //}
        //displayPlayerName = name;
    }
}
