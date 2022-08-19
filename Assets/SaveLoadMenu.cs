using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveLoadMenu : MonoBehaviour
{
    [Header("References")]

    public GameObject statsWindow;
    public GameObject mainMenu;
    public GameObject nameInput;
    public TextMeshProUGUI displayName;
    [SerializeField] TMP_InputField nameInputField;



    [Header("Stats")]
    public string playerName;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI matches;
    public TextMeshProUGUI matchesWon;
    public TextMeshProUGUI matchesLost;


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
        }
    }

    public void ChangeDisplayName(string name)
    {
        displayName.text = name;
        nameInputField.text = name;
    }

    public void GetStats()
    {
        playerName = SaveData.PlayerProfile.playerName;
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
}
