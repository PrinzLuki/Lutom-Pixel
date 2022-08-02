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

    //Ohne authority daweil, da ich nicht weiﬂ wie das funktioniert
    [Command(requiresAuthority = false)]
    public void CmdPickUp(PlayerGun playerGun)
    {
        RpcPickUp(playerGun);
    }

    [ClientRpc]
    public void RpcPickUp(PlayerGun playerGun)
    {
        parent = playerGun.transform;
        playerGun.CmdPickUpGunOnServer(playerGun.gameObject, this.gameObject);
    }

    //[Command(requiresAuthority = false)]
    //public void CmdDrop(PlayerGun playerGun)
    //{
    //    RpcDrop(playerGun);

    //}

    //[ClientRpc]
    //public void RpcDrop(PlayerGun playerGun)
    //{
    //    playerGun.CmdDropGunOnServer(playerGun.gameObject);
    //    parent = null;
    //}



}
