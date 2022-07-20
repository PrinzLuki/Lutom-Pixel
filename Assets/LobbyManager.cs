using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] NetworkRoomManager networkRoomManager;

    public List<GameObject> roomPlayers;
    [SyncVar(hook = nameof(IncreaseIndex))]
    public int index;
    public GameObject hostPlayer;

    private void Awake()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();

        if (uIManager == null) uIManager = FindObjectOfType<UIManager>();

        index = 0;
    }

    public void HostGame()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();

        networkRoomManager.StartHost();
    }

    public void JoinGame()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();

        networkRoomManager.StartClient();
        if (string.IsNullOrEmpty(uIManager.ipAddress.text)) uIManager.ipAddress.text = "localhost";
        networkRoomManager.networkAddress = uIManager.ipAddress.text;
    }

    public void GetClientInfo(GameObject client)
    {

        LobbyPlayer clientPlayer = client.GetComponent<LobbyPlayer>();
        //NetworkRoomPlayer roomPlayer = client.GetComponent<NetworkRoomPlayer>();
        if (index == 0)
        {
            clientPlayer.isHosting = true;
            hostPlayer = client;
            if (string.IsNullOrEmpty(uIManager.clientUsername.text))
            {
                uIManager.clientUsername.text = "Player[Host]";
            }

        }
        else if (index > 0)
        {
            clientPlayer.isHosting = false;

            if (string.IsNullOrEmpty(uIManager.clientUsername.text))
            {
                uIManager.clientUsername.text = "Player[" + index + "]";
            }
        }
        clientPlayer.username = uIManager.clientUsername.text;
        clientPlayer.SetPlayerInfo();
    }


    void IncreaseIndex(int oldindex, int newindex)
    {
        index = newindex;
    }

    public void CmdIncreaseIndex()
    {
        index++;

    }

 



}
