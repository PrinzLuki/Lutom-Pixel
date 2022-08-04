using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shotgun : Weapon
{


    [ClientRpc]
    public override void RpcShootBullet(GameObject player, Vector3 direction)
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
}
