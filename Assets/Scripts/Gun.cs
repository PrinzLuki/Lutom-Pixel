using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Transform player;
    [SerializeField] private Transform weaponHolder;
    [Header("Instantiated Prefab")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private Transform bulletSpawn;
    public PhysicsMaterial2D bouncyMAT;


    [Header("Weapon")]
    public WeaponScriptableObject weaponScriptable;
    public BulletScriptableObject bulletScriptable;
    public float currentMunition;
    public float currentSpeed;
    //public bool automatic;

    public override void OnStartLocalPlayer()
    {
        //Camera.main.transform.SetParent(transform);
        //Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }


    private void Start()
    {
        player = this.transform;
        //weaponHolder = transform.GetChild(1);

        if (weaponScriptable != null)
        {
            currentMunition = weaponScriptable.munition;
            currentSpeed = weaponScriptable.speed;

            weaponPrefab = Instantiate(weaponScriptable.weaponPrefab, this.transform, false);
            weaponPrefab.transform.parent = weaponHolder.transform;

            //NetworkServer.Spawn(weaponPrefab);
            gunEnd = weaponPrefab.transform.GetChild(1);

            bulletSpawn = weaponPrefab.transform.GetChild(2);
        }
        //weaponSprite = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        //weaponSprite.sprite = weaponScriptable.sprite;

        //gunEnd.localPosition = weaponScriptable.gunEndPosition;
        //bulletSpawn.localPosition = weaponScriptable.bulletSpawnPosition;
    }

    [Client]
    void Update()
    {

        if (!hasAuthority) return;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CmdRotateWeapon();
        //RotateWeapon();

        if (weaponScriptable == null) return;

        if (weaponScriptable.automatic)
        {
            if (Input.GetMouseButton(0) && currentMunition > 0)
            {
                //ShootBullet();
                CmdShootBullet();

            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentMunition > 0)
            {
                //ShootBullet();
                CmdShootBullet();
            }
        }


    }

    private void ShootBullet()
    {
        //Vector3 shootDirection = Input.mousePosition;
        //shootDirection.z = 0.0f;
        //shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        var shootDirection = mousePos;
        shootDirection = shootDirection - weaponHolder.transform.position;
        GameObject bulletInstance = Instantiate(bulletScriptable.prefab, bulletSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0)));


        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = new Vector2(shootDirection.x * weaponScriptable.speed, shootDirection.y * weaponScriptable.speed);

        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        currentMunition--;
        if (currentMunition <= 0) currentMunition = 0;

        //NetworkServer.Spawn(bulletInstance, this.gameObject);
    }

    [Command]
    private void CmdShootBullet()
    {
        RpcShootBullet();
    }

    [ClientRpc]
    private void RpcShootBullet() => ShootBullet();

    private void RotateWeapon()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(weaponHolder.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        weaponHolder.transform.rotation = q;
        if (mousePos.x > player.position.x)
        {
            weaponHolder.transform.localScale = new Vector3(1, 1, 1);
        }

        if (mousePos.x < player.position.x)
        {
            weaponHolder.transform.localScale = new Vector3(1, -1, 1);
        }
    }


    [Command]
    private void CmdRotateWeapon()
    {
        RpcRotateWeapon();
    }

    [ClientRpc]
    private void RpcRotateWeapon() => RotateWeapon();


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        if (gunEnd != null)
            Gizmos.DrawLine(gunEnd.position, mousePos);

        //Gizmos.DrawLine(transform.position, Input.mousePosition - Camera.main.WorldToScreenPoint(weaponHolder.transform.position));
    }
}
