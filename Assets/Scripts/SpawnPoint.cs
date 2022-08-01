using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPoint : NetworkBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField, Range(2.5f, 10)] float spawnDelay = 5;
    [SerializeField] int maxEnemyCount = 10;

    public List<GameObject> aliveEnemys;
    float spawnTime = 5;


    private void Start()
    {
        spawnTime = spawnDelay + (Random.Range(-2f, 2f));
    }

    private void Update()
    {
        StartSpawn();
    }

    //Server
    [Command(requiresAuthority = false)]
    void CmdStartEnemySpawn()
    {
        if (CanSpawn())
        {
            var enemy = Instantiate(enemyPrefab, this.gameObject.transform.position, Quaternion.identity);
            NetworkServer.Spawn(enemy, connectionToClient);
            aliveEnemys.Add(enemy);
        }
    }

    //Client
    void StartSpawn()
    {
        spawnTime -= Time.deltaTime;
        CmdStartEnemySpawn();
    }

    //Check for spawndelay and enemy Count
    bool CanSpawn()
    {
        if (spawnTime <= 0 && aliveEnemys.Count < maxEnemyCount)
        {
            spawnTime = spawnDelay + (Random.Range(-2f, 2f));
            return true;
        }

        return false;
    }
}
