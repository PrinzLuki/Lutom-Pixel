using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
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
}
