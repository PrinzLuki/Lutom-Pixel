using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthManager : NetworkBehaviour
{
    public List<GameObject> healthbars;



    public int waitForAmountOfPlayers = 2;

    public float spawnDelay;
    public bool waitForPlayers = true;

    //[Server]
    private void Start()
    {
        if (!isServer) return;

        if (waitForPlayers)
            CheckPlayers();
        else
            InitHealthList();
    }


    public void CheckPlayers()
    {
        var playerCount = NetworkServer.connections.Count;
        if (playerCount < waitForAmountOfPlayers)
        {
            StartCoroutine(WaitForPlayers());
        }
        else
        {
            InitHealthList();
        }
    }

    IEnumerator WaitForPlayers()
    {
        yield return new WaitForSeconds(spawnDelay);
        CheckPlayers();
    }


    private void InitHealthList()
    {
        healthbars = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            healthbars.Add(transform.GetChild(i).gameObject);
        }
    }


    public void InitHealthbar(GameObject player)
    {

    }





}
