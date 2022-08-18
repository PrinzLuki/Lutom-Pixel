using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ChatUI : NetworkBehaviour
{

    [Header("UI Elements")]
    public GameObject chatWindow;
    public InputField chatMessage;
    public GameObject smallChat;
    public TextMeshProUGUI smallChatText;

    public float timeToAppear = 7f;
    private float timeWhenDisappear;

    public bool chatIsOpened;

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private PlayerUI playerUI;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        playerUI = GetComponent<PlayerUI>();

        if (!hasAuthority)
        {
            chatWindow.SetActive(false);
        }
    }

    private void Update()
    {
        if (!hasAuthority) return;
        if (InputManager.instance == null) { Debug.LogWarning("Input Instance missing!"); return; }


        if (InputManager.instance.Chat() && isLocalPlayer && !chatIsOpened && !playerUI.paused)
        {
            OpenChat();
        }

        if (smallChatText.enabled && (Time.time >= timeWhenDisappear))
        {
            CmdDisappearMessage();
            chatIsOpened = false;
        }

    }

    void OpenChat()
    {
        chatIsOpened = true;
        chatWindow.SetActive(chatIsOpened);
        playerMovement.CanMove = false;
        GetComponent<Animator>().SetBool("isMoving", false);
        chatMessage.Select();

    }

    [Command(requiresAuthority = false)]
    public void CmdSend(string message)
    {
        RpcReceive(message.Trim());

    }

    [ClientRpc]
    public void RpcReceive(string message)
    {
        smallChatText.enabled = true;
        smallChatText.text = message;
    }

    [Command(requiresAuthority = false)]
    public void CmdDisappearMessage()
    {
        RpcDisappearMessage();
    }

    [ClientRpc]
    public void RpcDisappearMessage()
    {
        smallChatText.enabled = false;
    }

    public void SendPlayerMessage()
    {
        if (!string.IsNullOrWhiteSpace(chatMessage.text))
        {
            if (CheckForCheats(chatMessage.text.ToLower()))
            {
                timeWhenDisappear = Time.time + 0;
            }
            else
            {

                timeWhenDisappear = Time.time + timeToAppear;
            }
            CmdSend(chatMessage.text.Trim());
            chatMessage.text = string.Empty;
            chatMessage.ActivateInputField();
           
            chatWindow.SetActive(false);
            playerMovement.CanMove = true;

        }
        else
        {
            CmdSend(chatMessage.text);
            chatMessage.text = string.Empty;
            chatMessage.ActivateInputField();
            timeWhenDisappear = Time.time + 0;
            chatWindow.SetActive(false);
            playerMovement.CanMove = true;
        }

    }

    public bool CheckForCheats(string text)
    {
        switch (text)
        {
            case "/makeme immortal":
                playerStats.CmdToggleImmortality();
                return true;
            default:
                return false;
        }
    }

}


