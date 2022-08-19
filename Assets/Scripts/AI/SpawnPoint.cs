using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPoint : NetworkBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField, Range(2.5f, 10)] float spawnDelay = 5;
    [SerializeField] int maxEnemyCount = 10;
    float spawnTime = 5;

    private void Start()
    {
        if (!isServer) return;

        spawnTime = spawnDelay + (Random.Range(-2f, 2f));
    }

    private void Update()
    {
        if (!isServer) return;

        if (SpawnDelayTimer())
        {
            SpawnRandomEnemy(GetRandomEnemy());
        }
    }

    bool SpawnDelayTimer()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0) 
        {
            spawnTime = spawnDelay + (Random.Range(-2f, 2f));
            return true;
        } 
 
        else return false;
    }

    GameObject GetRandomEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    void SpawnRandomEnemy(GameObject rndEnemy)
    {
        var tempEnemy = Instantiate(rndEnemy, this.transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempEnemy);
    }
}
