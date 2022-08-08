using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    public List<Transform> itemSpawnPoints;
    public List<GameObject> itemGameObjects;
    public int itemSpawnCount;

    public float spawnDelay = 10f;
    public float timer;
    public int waitForAmountOfPlayers = 2;
    public float waitForPlayerDelay;
    public bool waitForPlayers = true;

    private Transform lastSpawnPoint;


    private void Start()
    {
        if (!isServer) return;

        if (itemGameObjects == null)
        {
            Debug.LogWarning("itemGameObjects in ItemSpawner Empty");
            return;
        }

        itemSpawnPoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            itemSpawnPoints.Add(transform.GetChild(i));
        }

        timer = spawnDelay;

    }

    private void Update()
    {
        if (!isServer) return;
        if (!MoreThanAmountOfPlayers() && waitForPlayers) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnItems();
            timer = spawnDelay;
        }

    }


    public bool MoreThanAmountOfPlayers()
    {
        var playerCount = NetworkServer.connections.Count;
        if (playerCount < waitForAmountOfPlayers)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void SpawnItems()
    {
   
        for (int j = 0; j < itemSpawnCount; j++)
        {
            var randomI = Random.Range(0, itemSpawnPoints.Count);
            if (lastSpawnPoint == itemSpawnPoints[randomI])
            {
                randomI++;
                if (randomI >= itemSpawnPoints.Count) randomI = 0;
            }

            GameObject itemClone = Instantiate(itemGameObjects[Random.Range(0, itemGameObjects.Count)], itemSpawnPoints[randomI].position, itemSpawnPoints[randomI].rotation);
            lastSpawnPoint = itemSpawnPoints[randomI];
            NetworkServer.Spawn(itemClone);
        }
    }
}
