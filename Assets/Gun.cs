using Mirror;
using UnityEngine;

public class Gun : NetworkBehaviour, IWeapon
{
    public WeaponScriptableObject weaponScriptableObject;
    public Transform gunEnd;
    public Transform bulletSpawn;

    public Transform parent;


    private void Update()
    {
        if (parent != null)
        {
            transform.position = parent.position;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdPickUp(PlayerGun playerGun)
    {
        //netIdentity.AssignClientAuthority(playerGun.netIdentity.connectionToClient);
        RpcPickUp(playerGun);
    }

    [ClientRpc]
    public void RpcPickUp(PlayerGun playerGun)
    {
        playerGun.CmdPickUpGunOnServer(playerGun.gameObject, this.gameObject);
        parent = playerGun.transform;
    }

    [Command(requiresAuthority = false)]
    public void CmdDrop(PlayerGun playerGun, Vector2 direction)
    {
        RpcDrop(playerGun, direction);
        //netIdentity.RemoveClientAuthority();
    }

    [ClientRpc]
    public void RpcDrop(PlayerGun playerGun, Vector2 direction)
    {
        parent = playerGun.transform;
        playerGun.CmdDropGunOnServer(playerGun.gameObject, direction);
    }
}
