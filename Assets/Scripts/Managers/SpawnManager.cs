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
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            spawnPos.Add(child);
        }
    }


    [Server]
    public override void OnStartServer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            spawnPos.Add(child);
        }
        for (int i = 0; i < spawnPos.Count; i++)
        {
            var spawn = Instantiate(spawnPointPrefab);
            spawn.GetComponent<EnemyStats>().spawnManager = this;
            NetworkServer.Spawn(spawn);
            spawn.transform.position = spawnPos[i].position;
        }
    }
}
