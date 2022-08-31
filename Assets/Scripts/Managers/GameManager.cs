using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    public static List<PlayerStats> players = new List<PlayerStats>();

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(instance);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        var inputMan = FindObjectOfType<InputManager>();
        inputMan.gameObject.SetActive(false);
        inputMan.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        PlayerStats.OnGameOver += IsGameOver;
    }
    private void OnDisable()
    {
        PlayerStats.OnGameOver -= IsGameOver;
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

}
