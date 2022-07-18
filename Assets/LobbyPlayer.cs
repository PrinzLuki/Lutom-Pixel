using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class LobbyPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] NetworkRoomManager networkRoomManager;
    [SerializeField] NetworkIdentity networkIdentity;
    [SerializeField] NetworkRoomPlayer networkRoomPlayer;
    [SerializeField] GameObject roomPlayerPanel;

    [Header("Inputs")]
    [SerializeField] TextMeshProUGUI usernameTMP;
    [SerializeField] TextMeshProUGUI IpTMP;
    [Header("Buttons")]
    [SerializeField] Button hostStopButton;
    [SerializeField] Button clientExitButton;
    [Header("Info")]
    public string username;
    public bool isHosting;


    private void Awake()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();
        if (networkRoomPlayer == null) networkRoomPlayer = GetComponent<NetworkRoomPlayer>();
        if (networkIdentity == null) networkIdentity = GetComponent<NetworkIdentity>();

    }
    public void StopHostingGame()
    {
        networkRoomManager.StopHost();
    }


    public void StopClient()
    {
        networkRoomManager.StopClient();

    }

    public void ReadyClient()
    {
        networkRoomPlayer.readyToBegin = true;
        networkRoomPlayer.CmdChangeReadyState(true);
    }

    public void UnreadyClient()
    {
        networkRoomPlayer.readyToBegin = false;
        networkRoomPlayer.CmdChangeReadyState(false);

    }

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }

   public void SetPlayerInfo()
    {
        IpTMP.text = GetLocalIPv4();

        usernameTMP.text = username;

        if (isHosting)
        {
            hostStopButton.gameObject.SetActive(true);
        }
        else
        {
            clientExitButton.gameObject.SetActive(true);
        }

        roomPlayerPanel.SetActive(true);
    }

}
