using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mirror
{
    public class NetworkManagerLutom : NetworkManager
    {
        [Server]
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            GameManager.players.Add(conn.identity.GetComponent<PlayerStats>());
            Debug.Log("OnServerAddPlayer: " + conn.identity.name);

        }

        #region Client

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            //ClientOnConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            //ClientOnDisconnected?.Invoke();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            //playerList.Clear();
        }

        #endregion

    }
}
