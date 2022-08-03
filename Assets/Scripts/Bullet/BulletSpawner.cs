using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletSpawner : NetworkBehaviour
{
    public List<Transform> bulletSpawnPoints;
    public List<BulletScriptableObject> bulletScriptables;
    public int bulletSpawnCount;

    public float spawnDelay = 10f;
    public float timer;
    public int waitForAmountOfPlayers = 2;
    public float waitForPlayerDelay;
    public bool waitForPlayers = true;


    //[Server]
    private void Start()
    {
        if (!isServer) return;

        if (bulletScriptables == null)
        {
            Debug.LogWarning("BulletScriptables in BulletSpawner Empty");
            return;
        }

        timer = spawnDelay;

    }

    //[Server]
    private void Update()
    {
        if (!isServer) return;
        if (!MoreThanTwoPlayers() && waitForPlayers) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SpawnBullets();
            timer = spawnDelay;
        }

    }


    public bool MoreThanTwoPlayers()
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

    private void SpawnBullets()
    {
        bulletSpawnPoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            bulletSpawnPoints.Add(transform.GetChild(i));
        }

        for (int j = 0; j < bulletSpawnCount; j++)
        {
            var randomI = Random.Range(0, bulletSpawnPoints.Count);
            GameObject weaponClone = Instantiate(bulletScriptables[Random.Range(0, bulletScriptables.Count)].bulletItem, bulletSpawnPoints[randomI].position, bulletSpawnPoints[randomI].rotation);
            NetworkServer.Spawn(weaponClone);
        }
    }

}
