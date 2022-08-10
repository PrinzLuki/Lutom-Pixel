using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : NetworkBehaviour
{
    public List<Transform> weaponSpawnPoints;
    public List<WeaponScriptableObject> weaponScriptables;
    public List<GameObject> spawnedWeapons;
    public int maxSpawnableWeapons = 2;
    public int weaponSpawnCount;
    public int waitForAmountOfPlayers = 2;

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
        if (playerCount < waitForAmountOfPlayers)
        {
            StartCoroutine(WaitForPlayers());
        }
        else
        {
            weaponSpawnPoints = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                weaponSpawnPoints.Add(transform.GetChild(i));
            }

            SpawnWeapons();
        }
    }

    private void SpawnWeapons()
    {

        for (int i = 0; i < weaponSpawnPoints.Count; i++)
        {
            for (int j = 0; j < weaponSpawnCount; j++)
            {
                GameObject weaponClone = Instantiate(weaponScriptables[Random.Range(0, weaponScriptables.Count)].weaponPrefab, weaponSpawnPoints[i].position, weaponSpawnPoints[i].rotation);
                NetworkServer.Spawn(weaponClone);
                weaponClone.GetComponent<Weapon>().weaponSpawnerParent = this;
                spawnedWeapons.Add(weaponClone);
                //CmdAddWeaponToList(weaponClone, this);
            }
        }
    }


    public void RespawnAWeapon()
    {
        if (spawnedWeapons.Count >= maxSpawnableWeapons) return;

        int randomI = Random.Range(0, weaponSpawnPoints.Count);

        GameObject weaponClone = Instantiate(weaponScriptables[Random.Range(0, weaponScriptables.Count)].weaponPrefab, weaponSpawnPoints[randomI].position, weaponSpawnPoints[randomI].rotation);
        NetworkServer.Spawn(weaponClone);
        weaponClone.GetComponent<Weapon>().weaponSpawnerParent = this;
        spawnedWeapons.Add(weaponClone);
        //CmdAddWeaponToList(weaponClone, this);
    }


    //[Command(requiresAuthority = false)]
    //public void CmdAddWeaponToList(GameObject weaponClone, WeaponSpawner spawner)
    //{
    //    RpcAddWeaponToList(weaponClone, spawner);
    //}

    //[ClientRpc]
    //void RpcAddWeaponToList(GameObject weaponClone, WeaponSpawner spawner)
    //{
    //    weaponClone.GetComponent<Weapon>().weaponSpawnerParent = this;
    //    spawner.spawnedWeapons.Add(weaponClone);
    //}

    IEnumerator WaitForPlayers()
    {
        yield return new WaitForSeconds(spawnDelay);
        CheckPlayers();
    }

}
