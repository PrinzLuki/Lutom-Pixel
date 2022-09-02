using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeathZone : NetworkBehaviour
{
    public float damage;
    public float damageDelay;
    public float timer;

    public bool isInWorld;


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<IWeapon>() != null && !isInWorld)
        {
            var weaponSpawner = other.GetComponent<Weapon>().weaponSpawnerParent;
            weaponSpawner.CmdRemoveWeaponFromList(other.gameObject);
            //weaponSpawner.spawnedWeapons.Remove(other.gameObject);
            //weaponSpawner.RespawnAWeapon();
            other.GetComponent<Weapon>().CmdDeleteGameObject(other.gameObject);
        }

        if (other.gameObject.GetComponent<IDamageable>() == null) return;

        if (isInWorld)
            GetDamage(other, damage);
        else
        {
            GetDamage(other, 999);
        }


    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() == null) return;

        timer += Time.deltaTime;
        if (timer >= damageDelay)
        {
            timer = 0;
            GetDamage(collision, damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        timer = 0;

    }

    void GetDamage(Collider2D other, float damage)
    {
        other.GetComponent<IDamageable>().GetDamage(damage, this.gameObject);
    }


}
