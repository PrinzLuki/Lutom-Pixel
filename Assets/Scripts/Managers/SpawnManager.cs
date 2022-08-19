using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] GameObject spawnPointPrefab;
    [SerializeField] List<Transform> spawnPos;

    public List<GameObject> allAliveEnemies;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            spawnPos.Add(child);
        }
        StartSpawnSpawnPoints();
    }

    void StartSpawnSpawnPoints()
    {
        if (!isServer) return;
        for (int i = 0; i < spawnPos.Count; i++)
        {
            var spawnPoint = Instantiate(spawnPointPrefab, spawnPos[i].position, Quaternion.identity);
            NetworkServer.Spawn(spawnPoint);
        }
    }
}
