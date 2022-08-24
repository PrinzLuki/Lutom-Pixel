using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseSingelton<T> : MonoBehaviour where T : BaseSingelton<T>
{
    public static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
