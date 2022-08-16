using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject lubbyUI = null;
    public Button startGameButton;
    public TMP_Text[] playerNamesDisplay = new TMP_Text[4];

    //private void Start()
    //{
    //    NetworkManagerLutom.ClientOnConnected += HandleClientConnected;
    //    Player.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
    //    Player.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    //}

    //private void OnDestroy()
    //{
    //    NetworkManagerLutom.ClientOnConnected -= HandleClientConnected;
    //    Player.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
    //    Player.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    //}

    private void HandleClientConnected()
    {
        lubbyUI.SetActive(true);
    }

    private void ClientHandleInfoUpdated()
    {
        List<NetworkRoomPlayer> players = ((NetworkRoomManager)NetworkManager.singleton).roomSlots;
        //List<Player> players = ((NetworkManagerLutom)NetworkManager.singleton).playerList;

        for (int i = 0; i < players.Count; i++)
        {
            playerNamesDisplay[i].text = players[i].playerName;
        }
        for (int i = players.Count; i < playerNamesDisplay.Length; i++)
        {
            playerNamesDisplay[i].text = "Waiting For Players...";
        }

        startGameButton.interactable = players.Count > 1;
    }


    private void AuthorityHandlePartyOwnerStateUpdated(bool state)
    {
        startGameButton.gameObject.SetActive(state);
    }

    //public void StartGame()
    //{
    //    NetworkClient.connection.identity.GetComponent<Player>().CmdStartGame();
    //}

}
