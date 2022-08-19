using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class MainMenu : NetworkBehaviour
{
    public Button hostButton;
    [SerializeField] TMP_InputField nameInputFieldHost;
    public string playerName;
    [SerializeField] GameObject panel;
    [SerializeField] TMP_InputField addressInput;
    [SerializeField] TMP_InputField playerNameInputJoin;
    [SerializeField] Button joinButton;
    [SerializeField] NetworkRoomManager roomManager;


    [Header("References")]
    public GameObject playMenuDisplay;
    public GameObject lobbyParentDisplay;

    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();

    }

    public void ChangeNameHost()
    {
        if (nameInputFieldHost.text == string.Empty) hostButton.interactable = false;
        else hostButton.interactable = true;
        playerName = nameInputFieldHost.text;
    }

    public void Join()
    {

        string address = addressInput.text;

        NetworkManager.singleton.networkAddress = address;
        joinButton.interactable = false;
        ((NetworkRoomManager)NetworkManager.singleton).StartClient();

    }
    public void ChangeNameJoin()
    {
        if (playerNameInputJoin.text == string.Empty) joinButton.interactable = false;
        else joinButton.interactable = true;
        playerName = playerNameInputJoin.text;
    }

    public void ChangeIp()
    {
        if (addressInput.text == string.Empty) joinButton.interactable = false;
        else joinButton.interactable = true;
    }

    public void LeaveLobby()
    {
        if (isServer)
        {
            NetworkServer.Shutdown();
            NetworkClient.Disconnect();


        }
        else
        {
            NetworkClient.Disconnect();
        }

    }



    public void StartGame(string levelName)
    {
        Debug.Log("Anzahl der Spieler: " + NetworkServer.connections.Count);
        if (NetworkServer.connections.Count < 2) return;

        roomManager.ServerChangeScene(levelName);
    }

}
