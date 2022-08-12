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

        var bullets = new List<Collider2D>();

        foreach (Transform bulletSpawn in gun.bulletSpawns)
        {
            GameObject bulletInstance = Instantiate(gun.bulletScriptable.prefab, bulletSpawn.position, bulletSpawn.rotation);

            Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
            bulletInstance.GetComponent<Bullet>().parent = gun.parent;
            bulletRigidbody.velocity = new Vector2(direction.x * gun.currentSpeed, direction.y * gun.currentSpeed);

            bullets.Add(bulletInstance.GetComponent<Collider2D>());

            Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

            gun.currentMunition--;
            if (gun.currentMunition <= 0) gun.currentMunition = 0;
        }

        for (int i = 0; i < bullets.Count; i++)
        {
            for (int j = 0; j < bullets.Count; j++)
            {
                Physics2D.IgnoreCollision(bullets[i].GetComponent<Collider2D>(), bullets[j].GetComponent<Collider2D>());
            }
        }





    }
}
