using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BaseSingelton<T> : NetworkBehaviour where T : BaseSingelton<T>
{
    public static T instance;

    private void Awake()
    {
        //if(instance == null)
        //{
        //    instance = (T)this;
        //}
        //else if(instance != this)
        //{
        //    Destroy(gameObject);
        //}
        //DontDestroyOnLoad(gameObject);




        instance = FindObjectOfType<T>();

        if (instance == null)
        {
            var instanceObj = new GameObject("Instance_Singleton");
            instance = gameObject.AddComponent<T>();
        }
        else if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
