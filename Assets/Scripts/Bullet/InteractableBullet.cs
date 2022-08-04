using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractableBullet : NetworkBehaviour
{
    public BulletScriptableObject bulletScriptable;

    public float despawnTime = 5f;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PlayerGun playerGun = other.gameObject.GetComponent<PlayerGun>();
            if (playerGun.currentWeaponGameObject == null) return;

            CmdSetBullet(playerGun.currentWeaponGameObject);
            CmdDeleteGameObject(gameObject);
        }
        if (other.gameObject.layer == 11) //Weapon
        {

            CmdSetBullet(other.gameObject);
            CmdDeleteGameObject(gameObject);
        }
    }


    [Command(requiresAuthority = false)]
    public void CmdSetBullet(GameObject weapon)
    {
        RpcSetBulletWeapon(weapon);
    }

    [ClientRpc]
    public void RpcSetBulletWeapon(GameObject weapon)
    {
        Weapon gun = weapon.GetComponent<Weapon>();
        gun.bulletScriptable = bulletScriptable;
        gun.currentBulletGameObject = bulletScriptable.prefab;
        gun.ReloadWeapon();
    }



    [Command(requiresAuthority = false)]
    public void CmdDeleteGameObject(GameObject trg)
    {
        RpcDeleteGameObject(trg);
    }

    [ClientRpc]
    public void RpcDeleteGameObject(GameObject trg)
    {
        Destroy(trg);
    }

}
