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
    [SerializeField] GameObject levelInit;
    [HideInInspector] public Transform enemyContainer;

    public int MaxEnemyCount { get => maxEnemyCount; set => maxEnemyCount = value; }

    private void Start()
    {
        levelInit = GameObject.FindGameObjectWithTag("LevelInit");
        StopSpawnInPvP();

        if (!isServer) return;

        spawnTime = spawnDelay + (Random.Range(-2f, 2f));
    }

    [Server]
    void StopSpawnInPvP()
    {
        Debug.Log("Level Init isPve = " + levelInit.GetComponent<LevelInitializer>().IsPvE);
        Debug.Log("GameManager isPve = " + GameManager.instance.isPvE);

        if (levelInit.GetComponent<LevelInitializer>().IsPvE) return;
        
        this.gameObject.SetActive(false);
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
        if (spawnTime <= 0 && enemyContainer.childCount < maxEnemyCount) 
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
        var tempEnemy = Instantiate(rndEnemy, this.transform.position, Quaternion.identity, enemyContainer);
        NetworkServer.Spawn(tempEnemy);
    }
}

