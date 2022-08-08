using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shotgun : Weapon
{
    public Transform[] bulletSpawns;

    [ClientRpc]
    public override void RpcShootBullet(GameObject player, Vector3 direction)
    {
        PlayerGun playerGun = player.GetComponent<PlayerGun>();
        Shotgun gun = playerGun.currentWeaponGameObject.GetComponent<Shotgun>();

        if (gun.bulletSpawns.Length <= 0) return;


        foreach (Transform bulletSpawn in gun.bulletSpawns)
        {
            GameObject bulletInstance = Instantiate(gun.bulletScriptable.prefab, bulletSpawn.position, bulletSpawn.rotation);

            Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = new Vector2(direction.x * gun.currentSpeed, direction.y * gun.currentSpeed);


            Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

            gun.currentMunition--;
            if (gun.currentMunition <= 0) gun.currentMunition = 0;
        }



    }
}
