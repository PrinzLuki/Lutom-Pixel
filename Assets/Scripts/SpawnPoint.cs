using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPoint : NetworkBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnDelay = 5;
    public List<GameObject> aliveEnemys;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpawn();
        }
    }

   

    //Server
    [Command (requiresAuthority = false)]
    void CmdStartEnemySpawn()
    {
        var enemy = Instantiate(enemyPrefab, this.gameObject.transform.position, Quaternion.identity);
        NetworkServer.Spawn(enemy, connectionToClient);
    }

    //Client
    void StartSpawn()
    {
        Debug.Log("StartSpawn");
        CmdStartEnemySpawn();
    }

}
