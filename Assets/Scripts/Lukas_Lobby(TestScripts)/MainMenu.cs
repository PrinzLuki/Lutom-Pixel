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
    public string playerId;
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

    //public int sameNameIndex = 1;

    public static event Func<int> OnNeedKillsSet;


    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
        GameManager.instance.killsToWin = OnNeedKillsSet.Invoke();
    }

    private void Start()
    {

        if (!string.IsNullOrEmpty(SaveData.PlayerProfile.playerName))
        {
            playerName = SaveData.PlayerProfile.playerName;
            nameInputField.text = playerName;
        }


        playerId = FindObjectOfType<SaveLoadMenu>().playerid;
    }

    public void ChangeName()
    {
        playerName = nameInputField.text;

        SaveData.PlayerProfile.playerName = playerName;
        SerializationManager.Save("playerData", SaveData.PlayerProfile);

        FindObjectOfType<SaveLoadMenu>().ChangeDisplayName(playerName);
    }

    public void GetGuid()
    {
        if (string.IsNullOrEmpty(SaveData.PlayerProfile.playerid))
        {
            Guid guid = Guid.NewGuid();
            playerId = guid.ToString();
            SaveData.PlayerProfile.playerid = playerId;
            Debug.Log("Creating new guid: " + playerId);
        }
        else
        {
            playerId = SaveData.PlayerProfile.playerid;
            Debug.Log("Getting guid from savedata: " + playerId);
        }
        SerializationManager.Save("playerData", SaveData.PlayerProfile);
        FindObjectOfType<SaveLoadMenu>().ChangeDisplayId(playerId);

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
            StartCoroutine(LeaveDelayHost());
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

        foreach (var check in playerChecksDisplay)
        {
            check.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        CmdSetPVE(GameManager.instance.isPvE, ((NetworkRoomManagerLutom)roomManager).gamemode);
        CmdSetMinPlayer(roomManager.minPlayers);
        CmdSetKillsToWin(GameManager.instance.killsToWin);
        StartCoroutine(WaitBeforeStart());

    }

    [Command(requiresAuthority = false)]
    public void CmdSetPVE(bool PVEValue, Gamemodetype type)
    {
        RpcSetPVE(PVEValue, type);
    }
    [ClientRpc]
    public void RpcSetPVE(bool PVEValue, Gamemodetype type)
    {
        GameManager.instance.isPvE = PVEValue;
        ((NetworkRoomManagerLutom)roomManager).gamemode = type;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetMinPlayer(int minPlayers)
    {
        RpcSetMinPlayer(minPlayers);
    }

    [ClientRpc]
    public void RpcSetMinPlayer(int minPlayers)
    {
        roomManager.minPlayers = minPlayers;
        GameManager.instance.amountOfPlayers = minPlayers;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetKillsToWin(int amountKills)
    {
        RpcSetKillsToWin(amountKills);
    }

    [ClientRpc]
    public void RpcSetKillsToWin(int amountKills)
    {
        GameManager.instance.killsToWin = amountKills;
    }


    IEnumerator WaitBeforeStart()
    {
        yield return new WaitForSeconds(0.5f);
        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }


    public void ReadyPlayer()
    {
        OnReadyPlayer?.Invoke();

        if (!isServer) return;

        CmdSendNeededKills(GameManager.instance.killsToWin);
    }

    [Command(requiresAuthority = false)]
    public void CmdSendNeededKills(int kills)
    {
        RpcSendNeededKills(kills);
    }

    [ClientRpc]
    public void RpcSendNeededKills(int kills)
    {
        GameManager.instance.killsToWin = kills;
        //Debug.Log($"Main Menu neededKills: {kills}");
    }


    public void ToggleStartGameButton(bool value)
    {
        if (!isServer) return;
        if (startGameButton == null) return;
        startGameButton.interactable = value;
    }



    IEnumerator LeaveDelayHost()
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
