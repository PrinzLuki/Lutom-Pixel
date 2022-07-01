using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingelton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    private void Awake()
    {
        instance = FindObjectOfType<T>();

        if (instance == null)
        {
            var instanceObj = new GameObject("Instance of a Singleton");
            instance = gameObject.AddComponent<T>();
        }
        else if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
