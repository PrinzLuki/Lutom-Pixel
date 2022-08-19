using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameHostSettings : MonoBehaviour
{
    public NetworkRoomManagerLutom roomManager;

    [Header("Amount of Players")]
    public TextMeshProUGUI amountOfPlayers;
    [Header("Levels")]
    public LevelScriptable[] levels;
    public TextMeshProUGUI levelsName;
    public Image levelImg;
    public int levelIndex;
    [Header("Gamemode")]
    public TextMeshProUGUI gamemodeName;


    private void Start()
    {
        amountOfPlayers.text = roomManager.minPlayers.ToString();
        levelIndex = Random.Range(0, levels.Length);
        ChangeLevel(levelIndex);
    }


    #region Gamemode

    public void PrevGamemode()
    {
        roomManager.gamemode -= 1;
        if (roomManager.gamemode < 0)
        {
            roomManager.gamemode = Gamemodetype.MAX - 1;
        }
        ChangeGamemode(roomManager.gamemode);
    }

    public void NextGamemode()
    {
        roomManager.gamemode += 1;
        if (roomManager.gamemode >= Gamemodetype.MAX)
        {
            roomManager.gamemode = 0;
        }
        ChangeGamemode(roomManager.gamemode);

    }

    public void ChangeGamemode(Gamemodetype type)
    {
        gamemodeName.text = type.ToString();
        roomManager.gamemode = type;
    }



    #endregion

    #region Levels

    public void PrevLevel()
    {
        levelIndex -= 1;
        if (levelIndex < 0)
        {
            levelIndex = levels.Length - 1;
        }
        ChangeLevel(levelIndex);
    }

    public void NextLevel()
    {
        levelIndex += 1;
        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }
        ChangeLevel(levelIndex);

    }

    public void ChangeLevel(int index)
    {
        levelsName.text = levels[index].levelName;
        levelImg.sprite = levels[index].levelImage;
        roomManager.SetLevelScene(levels[index].sceneName);
    }



    #endregion


    #region Amount of players

    public void DecreaseAmountPlayers()
    {
        roomManager.minPlayers -= 1;
        if (roomManager.minPlayers <= 0)
        {
            roomManager.minPlayers = roomManager.maxConnections;
        }
        amountOfPlayers.text = roomManager.minPlayers.ToString();
    }

    public void IncreaseAmountPlayers()
    {
        roomManager.minPlayers += 1;
        if (roomManager.minPlayers > roomManager.maxConnections)
        {
            roomManager.minPlayers = 1;
        }
        amountOfPlayers.text = roomManager.minPlayers.ToString();

    }

    #endregion
}
