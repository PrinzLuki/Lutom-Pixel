using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManagerLutom : NetworkManager
{
    [SerializeField] GameObject spawnPointPrefab;

    [SerializeField] Transform spawnPos;

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
        var spawn = Instantiate(spawnPointPrefab);
        NetworkServer.Spawn(spawn);
        spawn.transform.position = spawnPos.position;
    }
}
