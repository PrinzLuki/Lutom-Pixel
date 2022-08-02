using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPoint : NetworkBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField, Range(2.5f, 10)] float spawnDelay = 5;
    [SerializeField] int maxEnemyCount = 10;
    float spawnTime = 5;


    private void Start()
    {
        spawnTime = spawnDelay + (Random.Range(-2f, 2f));
    }

    [Server]
    private void Update()
    {
        CmdStartEnemySpawn();
    }

    //Server
    void CmdStartEnemySpawn()
    {
        spawnTime -= Time.deltaTime;
        if (CanSpawn())
        {
            var enemy = Instantiate(enemyPrefab, this.gameObject.transform.position, Quaternion.identity, this.transform);
            NetworkServer.Spawn(enemy);
            GetComponentInParent<SpawnManager>().allAliveEnemies.Add(enemy);
        }
    }

    //Check for spawndelay and enemy Count
    bool CanSpawn()
    {
        if (spawnTime <= 0)
        {
            spawnTime = spawnDelay + (Random.Range(-2f, 2f));
            return true;
        }
        return false;
    }
}
