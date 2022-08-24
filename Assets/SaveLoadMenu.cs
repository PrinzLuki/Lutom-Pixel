using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadMenu : MonoBehaviour
{
    [Header("References")]

    public GameObject statsWindow;
    public GameObject mainMenu;
    public GameObject nameInput;
    public TextMeshProUGUI displayName;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Button nameCheckButton;



    [Header("Stats")]
    public string playerName;
    public string playerid;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI matches;
    public TextMeshProUGUI matchesWon;
    public TextMeshProUGUI matchesLost;
    public TextMeshProUGUI guid;


    private void Start()
    {
        OnLoad();
        GetStats();
        if (string.IsNullOrEmpty(playerName))
        {
            nameInput.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            nameInput.SetActive(false);
            mainMenu.SetActive(true);
            ChangeDisplayName(playerName);
            ChangeDisplayId(playerid);
        }
    }

    public void ChangeDisplayName(string name)
    {
        displayName.text = name;
        nameInputField.text = name;
    }

    public void ChangeDisplayId(string id)
    {
        playerid = id;
        guid.text = "ID: " + id;
        Debug.Log("Changing guid text: " + id);

    }

    public void CheckName()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            nameCheckButton.interactable = false;
        }
        else
        {
            nameCheckButton.interactable = true;
        }
    }



    public void GetStats()
    {
        playerName = SaveData.PlayerProfile.playerName;
        playerid = SaveData.PlayerProfile.playerid;
        kills.text = SaveData.PlayerProfile.kills.ToString();
        deaths.text = SaveData.PlayerProfile.deaths.ToString();
        matches.text = SaveData.PlayerProfile.matchesPlayed.ToString();
        matchesWon.text = SaveData.PlayerProfile.matchesWon.ToString();
        matchesLost.text = SaveData.PlayerProfile.matchesLost.ToString();
    }

    public void OnSave()
    {
        SerializationManager.Save("playerData", SaveData.PlayerProfile);
    }

    public void OnLoad()
    {
        SaveData.PlayerProfile = (PlayerProfile)SerializationManager.Load(Application.persistentDataPath + "/saves/playerData.lutompixel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
