using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] List<DifficultyOverTime> difficulties;

    [SerializeField] GameObject spawnPointPrefab;
    [SerializeField] List<Transform> spawnPos;
    [SerializeField] List<SpawnPoint> spawnPoints;
    public GameObject enemySpawnContainer;

    float diffTime;
    int diffIdx;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            spawnPos.Add(child);
        }
        StartSpawnSpawnPoints();
    }

    private void Update()
    {
        UpdateDifficulty();
    }

    void StartSpawnSpawnPoints()
    {
        if (!isServer) return;
        for (int i = 0; i < spawnPos.Count; i++)
        {
            var spawnPoint = Instantiate(spawnPointPrefab, spawnPos[i].position, Quaternion.identity);
            spawnPoint.GetComponent<SpawnPoint>().enemyContainer = enemySpawnContainer.transform;
            NetworkServer.Spawn(spawnPoint);
            spawnPoints.Add(spawnPoint.GetComponent<SpawnPoint>());
        }
    }

    void UpdateDifficulty()
    {
        diffTime += Time.deltaTime;

        if (diffIdx >= difficulties.Count || difficulties[diffIdx] == null) return;

        if (difficulties[diffIdx].executionTime < diffTime)
        {
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                spawnPoints[i].MaxEnemyCount = difficulties[diffIdx].maxEnemies;
            }
            diffIdx++;
        }
    }
}

[System.Serializable]
public class DifficultyOverTime
{
    public float executionTime;
    public int maxEnemies;
}
