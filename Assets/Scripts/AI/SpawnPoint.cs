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
    GameObject spawnManager;

    private void Start()
    {
        if (isServer)
            spawnTime = spawnDelay + (Random.Range(-2f, 2f));

        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
    }

    private void Update()
    {
        if (isServer)
            CmdStartEnemySpawn();
    }

    //Server
    void CmdStartEnemySpawn()
    {
        spawnTime -= Time.deltaTime;
        if (CanSpawn())
        {
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], this.gameObject.transform.position, Quaternion.identity);
            NetworkServer.Spawn(enemy);
            GetComponentInParent<SpawnManager>().allAliveEnemies.Add(enemy);
        }
    }

    //Check for spawndelay and enemy Count
    bool CanSpawn()
    {
        var spawner = spawnManager.GetComponent<SpawnManager>();

        if (spawnTime <= 0 && maxEnemyCount > spawner.allAliveEnemies.Count)
        {
            spawnTime = spawnDelay + (Random.Range(-2f, 2f));
            return true;
        }
        return false;
    }
}
