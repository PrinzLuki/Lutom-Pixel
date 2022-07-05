using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] float spawnDelay;
    float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnDelay;
    }


    void Update()
    {
        spawnTimer -= Time.deltaTime;
        SpawnEnemys();
    }

    void SpawnEnemys()
    {

        if (spawnTimer <= 0)
        {
            var temp = Instantiate(enemyPrefab, enemyContainer.transform);
            temp.transform.position = this.transform.position;
            spawnTimer = spawnDelay;
        }

    }
}
