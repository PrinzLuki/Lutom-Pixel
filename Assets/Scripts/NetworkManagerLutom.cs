using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerLutom : NetworkManager
{
    [SerializeField] GameObject spawnPointPrefab;

    [SerializeField] List<Transform> spawnPos;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }

    [Server]
    public override void OnStartHost()
    {
        base.OnStartHost();
        for (int i = 0; i < spawnPos.Count; i++)
        {
            var spawn = Instantiate(spawnPointPrefab);
            NetworkServer.Spawn(spawn);
            spawn.transform.position = spawnPos[i].position;

        }
    }
}
