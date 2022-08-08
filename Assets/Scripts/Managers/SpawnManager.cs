using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] GameObject spawnPointPrefab;
    [SerializeField] List<Transform> spawnPos;

    public List<GameObject> allAliveEnemies;

    [Server]
    public override void OnStartServer()
    {
        for (int i = 0; i < spawnPos.Count; i++)
        {
            var spawn = Instantiate(spawnPointPrefab, this.gameObject.transform);
            NetworkServer.Spawn(spawn);
            spawn.transform.position = spawnPos[i].position;
        }
    }
}
