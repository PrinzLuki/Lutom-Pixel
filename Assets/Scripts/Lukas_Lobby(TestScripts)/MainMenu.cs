using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using System;

public class MainMenu : NetworkBehaviour
{
    public string playerName;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField addressInput;
    [SerializeField] NetworkRoomManager roomManager;

    public event Action OnReadyPlayer;

    [Header("References")]
    [SerializeField] GameObject panel;
    public GameObject playMenuDisplay;
    public GameObject lobbyParentDisplay;
    //[SerializeField] GameObject lubbyUI = null;
    public TMP_Text[] playerNamesDisplay = new TMP_Text[4];
    public GameObject[] playerChecksDisplay = new GameObject[4];

    [Header("Buttons")]
    public Button hostButton;
    public Button joinButton;
    public Button startGameButton;
    public Button readyPlayerButton;

    public Sprite readyImg;
    public Sprite notReadyImg;


    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(SaveData.PlayerProfile.playerName))
        {
            playerName = SaveData.PlayerProfile.playerName;
            nameInputField.text = playerName;
        }
    }

    public void ChangeName()
    {
        playerName = nameInputField.text;

        SaveData.PlayerProfile.playerName = playerName;
        SerializationManager.Save("playerData", SaveData.PlayerProfile);

        FindObjectOfType<SaveLoadMenu>().ChangeDisplayName(playerName);
    }

    public void Join()
    {
        string address = addressInput.text;

        NetworkManager.singleton.networkAddress = address;
        ((NetworkRoomManager)NetworkManager.singleton).StartClient();
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
            CmdBackToMenuDisplay();
            StartCoroutine(LeaveDelay());
        }
        else
        {
            NetworkClient.Disconnect();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdBackToMenuDisplay()
    {
        Debug.Log("CmdCallBack");
        RpcBackToMenuDisplay();
    }

    [ClientRpc]
    public void RpcBackToMenuDisplay()
    {
        Debug.Log("ClientCallBack");
        lobbyParentDisplay.gameObject.SetActive(false);
        playMenuDisplay.SetActive(true);
    }

    public void StartGame(string levelName)
    {
        Debug.Log("Anzahl der Spieler: " + NetworkServer.connections.Count);
        if (NetworkServer.connections.Count < 2) return;

        roomManager.ServerChangeScene(levelName);
    }

    public void ReadyPlayer()
    {
        OnReadyPlayer?.Invoke();
    }

    public void ToggleStartGameButton(bool value)
    {
        if (!isServer) return;
        if (startGameButton == null) return;
        startGameButton.interactable = value;
    }



    IEnumerator LeaveDelay()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Client: " + NetworkClient.active);
        Debug.Log("Server: " + NetworkServer.active);
        NetworkServer.Shutdown();
        NetworkClient.Disconnect();
        Debug.Log("Client: " + NetworkClient.active);
        Debug.Log("Server: " + NetworkServer.active);
    }

}
