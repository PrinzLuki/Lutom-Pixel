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
            PlayerGun playerGun = other.transform.GetComponent<PlayerGun>();
            if (playerGun.currentWeaponGameObject == null) return;

            playerGun.bulletScriptable = bulletScriptable;
            playerGun.ReloadWeapon();
            CmdDeleteGameObject(gameObject);
        }
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
