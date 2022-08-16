using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_InputField addressInput;
    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] Button joinButton;
    public string playerName;
  
    public void Join()
    {
        string address = addressInput.text;

        NetworkManager.singleton.networkAddress = address;
        joinButton.interactable = false;
        ((NetworkRoomManager)NetworkManager.singleton).StartClient();
        //((NetworkRoomManager)NetworkManager.singleton).OnRoomClientConnect();
    }
    public void ChangeName()
    {
        if (playerNameInput.text == string.Empty) joinButton.interactable = false;
        else joinButton.interactable = true;
        playerName = playerNameInput.text;
    }

    public void ChangeIp()
    {
        if (addressInput.text == string.Empty) joinButton.interactable = false;
        else joinButton.interactable = true;

    }
}
