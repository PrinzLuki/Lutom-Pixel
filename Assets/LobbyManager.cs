using Mirror;
using System.Collections;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] NetworkRoomManager networkRoomManager;

    private void Awake()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();

        if (uIManager == null) uIManager = FindObjectOfType<UIManager>();
    }

    public void HostGame()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();

        networkRoomManager.StartHost();
        StartCoroutine(GetHostGameObject());
    }

    public void JoinGame()
    {
        if (networkRoomManager == null) networkRoomManager = FindObjectOfType<NetworkRoomManager>();

        networkRoomManager.StartClient();
        StartCoroutine(GetClientGameObject());
        if (string.IsNullOrEmpty(uIManager.ipAddress.text)) uIManager.ipAddress.text = "localhost";
        networkRoomManager.networkAddress = uIManager.ipAddress.text;
    }

    IEnumerator GetHostGameObject()
    {
        yield return new WaitForSeconds(1f);

        GameObject host = networkRoomManager.roomSlots[0].gameObject;
        LobbyPlayer hostPlayer = host.GetComponent<LobbyPlayer>();
        if (string.IsNullOrEmpty(uIManager.clientUsername.text))
        {
            uIManager.clientUsername.text = "Player[Host]";
        }
        hostPlayer.username = uIManager.clientUsername.text;
        hostPlayer.isHosting = true;
        hostPlayer.SetPlayerInfo();
    }


    IEnumerator GetClientGameObject()
    {
        yield return new WaitForSeconds(1f);

        GameObject host = networkRoomManager.roomSlots[1].gameObject;
        LobbyPlayer hostPlayer = host.GetComponent<LobbyPlayer>();
        if (string.IsNullOrEmpty(uIManager.clientUsername.text))
        {
            uIManager.clientUsername.text = "Player[" + 1 + "]";
        }
        hostPlayer.username = uIManager.clientUsername.text;
        hostPlayer.isHosting = false;
        hostPlayer.SetPlayerInfo();
    }


}
