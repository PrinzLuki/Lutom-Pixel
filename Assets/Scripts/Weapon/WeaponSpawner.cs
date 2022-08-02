using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : NetworkBehaviour
{
    public List<Transform> weaponSpawnPoints;
    public List<WeaponScriptableObject> weaponScriptables;
    public int weaponSpawnCount;

    public float spawnDelay;
    public bool waitForPlayers = true;

    //[Server]
    private void Start()
    {
        if (!isServer) return;

        if (weaponScriptables == null)
        {
            Debug.LogWarning("WeaponScriptables in WeaponSpawner Empty");
            return;
        }

        if (waitForPlayers)
            CheckPlayers();
        else
            SpawnWeapons();
    }



    public void CheckPlayers()
    {
        var playerCount = NetworkServer.connections.Count;
        if (playerCount < 2)
        {
            StartCoroutine(WaitForPlayers());
        }
        else
        {
            SpawnWeapons();
        }
    }

    private void SpawnWeapons()
    {
        weaponSpawnPoints = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            weaponSpawnPoints.Add(transform.GetChild(i));
        }

        for (int i = 0; i < weaponSpawnPoints.Count; i++)
        {
            for (int j = 0; j < weaponSpawnCount; j++)
            {
                GameObject weaponClone = Instantiate(weaponScriptables[Random.Range(0, weaponScriptables.Count)].weaponPrefab, weaponSpawnPoints[i].position, weaponSpawnPoints[i].rotation);
                NetworkServer.Spawn(weaponClone);
            }
        }
    }

    IEnumerator WaitForPlayers()
    {
        yield return new WaitForSeconds(spawnDelay);
        CheckPlayers();
    }

}
