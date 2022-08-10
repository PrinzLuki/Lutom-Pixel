using Mirror;
using UnityEngine;

public class Weapon : NetworkBehaviour, IWeapon
{
    public WeaponScriptableObject weaponScriptableObject;
    public Transform gunEnd;
    public Transform bulletSpawn;

    public Transform parent;
    public WeaponSpawner weaponSpawnerParent;

    public float currentMunition;
    public float currentSpeed;

    [Header("Bullet")]
    public BulletScriptableObject bulletScriptable;
    public GameObject currentBulletGameObject;


    public virtual void Update()
    {
        if (parent != null)
        {
            transform.position = parent.position;
        }
    }

    public virtual void ReloadWeapon()
    {
        currentMunition = weaponScriptableObject.munition;
        currentSpeed = weaponScriptableObject.speed;
    }

    [Command(requiresAuthority = false)]
    public virtual void CmdShootBullet(GameObject player, Vector3 direction)
    {
        RpcShootBullet(player, direction);
    }

    [ClientRpc]
    public virtual void RpcShootBullet(GameObject player, Vector3 direction)
    {
        PlayerGun playerGun = player.GetComponent<PlayerGun>();
        Weapon gun = playerGun.currentWeaponGameObject.GetComponent<Weapon>();

        if (gun.bulletSpawn == null) return;
        GameObject bulletInstance = Instantiate(gun.bulletScriptable.prefab, gun.bulletSpawn.position, gun.bulletSpawn.rotation);

        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = new Vector2(direction.x * gun.currentSpeed, direction.y * gun.currentSpeed);

        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        gun.currentMunition--;
        if (gun.currentMunition <= 0) gun.currentMunition = 0;
    }

    #region PickUp

    [Command(requiresAuthority = false)]
    public virtual void CmdPickUp(PlayerGun playerGun)
    {
        //netIdentity.AssignClientAuthority(playerGun.netIdentity.connectionToClient);
        RpcPickUp(playerGun);
    }

    [ClientRpc]
    public virtual void RpcPickUp(PlayerGun playerGun)
    {
        playerGun.CmdPickUpGunOnServer(playerGun.gameObject, this.gameObject);
        parent = playerGun.transform;
    }
    #endregion

    #region Drop
    [Command(requiresAuthority = false)]
    public virtual void CmdDrop(PlayerGun playerGun, Vector2 direction)
    {
        RpcDrop(playerGun, direction);
        //netIdentity.RemoveClientAuthority();
    }

    [ClientRpc]
    public virtual void RpcDrop(PlayerGun playerGun, Vector2 direction)
    {
        //parent = playerGun.transform;
        playerGun.CmdDropGunOnServer(playerGun.gameObject, direction);
    }


    #endregion
}
