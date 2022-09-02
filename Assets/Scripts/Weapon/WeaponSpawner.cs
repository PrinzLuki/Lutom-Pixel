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
    //public int waitForAmountOfPlayers = 2;

    public float spawnDelay;
    public bool waitForPlayers = true;
    public bool canRespawn;

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
        {
            weaponSpawnPoints = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                weaponSpawnPoints.Add(transform.GetChild(i));
            }

            SpawnWeapons();
        }
    }

    private void Update()
    {
        if (!isServer) return;

        if (spawnedWeapons.Count >= maxSpawnableWeapons) return;
        if (canRespawn)
            RespawnAWeapon();
    }


    public void CheckPlayers()
    {
        var playerCount = NetworkServer.connections.Count;
        if (playerCount < GameManager.instance.amountOfPlayers)
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
        //Debug.Log("Spawning Weapons");
        for (int i = 0; i < weaponSpawnPoints.Count; i++)
        {
            for (int j = 0; j < weaponSpawnCount; j++)
            {
                GameObject weaponClone = Instantiate(weaponScriptables[Random.Range(0, weaponScriptables.Count)].weaponPrefab, 
                    weaponSpawnPoints[i].position, weaponSpawnPoints[i].rotation);
                NetworkServer.Spawn(weaponClone);
                //weaponClone.GetComponent<Weapon>().weaponSpawnerParent = this;
                CmdSetSpawnParent(weaponClone);
                CmdAddWeaponToList(weaponClone);
                //CmdAddWeaponToList(weaponClone, this);
            }
        }
        canRespawn = true;
    }


    public void RespawnAWeapon()
    {

        int randomI = Random.Range(0, weaponSpawnPoints.Count);

        GameObject weaponClone = Instantiate(weaponScriptables[Random.Range(0, weaponScriptables.Count)].weaponPrefab, weaponSpawnPoints[randomI].position, weaponSpawnPoints[randomI].rotation);
        NetworkServer.Spawn(weaponClone);
        CmdSetSpawnParent(weaponClone);
        //weaponClone.GetComponent<Weapon>().weaponSpawnerParent = this;
        CmdAddWeaponToList(weaponClone);
        //CmdAddWeaponToList(weaponClone, this);
    }

    [Command(requiresAuthority = false)]
    void CmdSetSpawnParent(GameObject weapon)
    {
        RpcSetSpawnParent(weapon);
    }
    [ClientRpc]
    void RpcSetSpawnParent(GameObject weapon)
    {
        weapon.GetComponent<Weapon>().weaponSpawnerParent = this;
        //Debug.Log("RPC Weapon spawner parent set = " + this.gameObject.name);
    }

    [Command(requiresAuthority = false)]
    void CmdAddWeaponToList(GameObject weapon)
    {
        RpcAddWeaponToList(weapon);
    }
    [ClientRpc]
    void RpcAddWeaponToList(GameObject weapon)
    {
        spawnedWeapons.Add(weapon);
    }

    [Command(requiresAuthority = false)]
    public void CmdRemoveWeaponFromList(GameObject weapon)
    {
        RpcRemoveWeaponFromList(weapon);
    }
    [ClientRpc]
    public void RpcRemoveWeaponFromList(GameObject weapon)
    {
        spawnedWeapons.Remove(weapon);
    }


    IEnumerator WaitForPlayers()
    {
        yield return new WaitForSeconds(spawnDelay);
        CheckPlayers();
    }

}
