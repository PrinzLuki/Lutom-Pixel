using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class GameManager : BaseSingelton<GameManager>
{
    public static List<PlayerStats> players = new List<PlayerStats>();
    [SerializeField] GameObject inputMan;
    public bool isPvE;
    public int killsToWin;

    private void OnEnable()
    {
        PlayerStats.OnGameOver += IsGameOver;
        GameHostSettings.OnGameModeChanged += SetGameMode;
        PlayerStats.OnPlayerWin += DidPlayerWin;
    }
    private void OnDisable()
    {
        PlayerStats.OnGameOver -= IsGameOver;
        GameHostSettings.OnGameModeChanged -= SetGameMode;
        PlayerStats.OnPlayerWin -= DidPlayerWin;
    }

    public bool IsGameOver()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (!players[i].isDead) return false;

            continue;
        }
        return true;
    }

    public bool DidPlayerWin(int kills)
    {
        bool didWin = kills >= killsToWin ? true : false;
        return didWin;
    }

    public void SetGameMode(Gamemodetype type)
    {
        switch (type)
        {
            case Gamemodetype.PVE:
                isPvE = true;
                Debug.Log("IsPvE");
                break;
            case Gamemodetype.PVP:
                isPvE = false;
                Debug.Log("isPvP");
                break;
            case Gamemodetype.MAX:
                Debug.LogError("No available GameMode Setted");
                break;
            default:
                Debug.LogWarning("GameMode change is called but didnt Set");
                break;
        }
    }

    public void RestartInputManager()
    {
        inputMan.SetActive(false);
        inputMan.SetActive(true);
    }

}
