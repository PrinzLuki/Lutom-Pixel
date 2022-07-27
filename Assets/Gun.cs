using Mirror;
using UnityEngine;

public class Gun : NetworkBehaviour, IWeapon
{
    public WeaponScriptableObject weaponScriptableObject;
    public Transform gunEnd;
    public Transform bulletSpawn;

    public void Drop(PlayerGun playerGun)
    {
        throw new System.NotImplementedException();
    }

    public void PickUp(PlayerGun playerGun)
    {
        playerGun.PickUpGunOnClient(playerGun.gameObject, this.gameObject);
    }

    [Command]
    public void CmdPickUp(PlayerGun playerGun)
    {
        RpcPickUp(playerGun);
    }

    [ClientRpc]
    public void RpcPickUp(PlayerGun playerGun)
    {
        playerGun.CmdPickUpGunOnServer(playerGun.gameObject, this.gameObject);
    }



}
