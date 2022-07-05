using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Transform player;
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

    private void Start()
    {
        player = transform.parent.GetComponentInParent<PlayerMovement>().transform;

        currentMunition = weaponScriptable.munition;
        currentSpeed = weaponScriptable.speed;

        weaponPrefab = Instantiate(weaponScriptable.weaponPrefab, this.transform, false);

        gunEnd = weaponPrefab.transform.GetChild(1);

        bulletSpawn = weaponPrefab.transform.GetChild(2);

        //weaponSprite = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        //weaponSprite.sprite = weaponScriptable.sprite;

        //gunEnd.localPosition = weaponScriptable.gunEndPosition;
        //bulletSpawn.localPosition = weaponScriptable.bulletSpawnPosition;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateWeapon();

        if (weaponScriptable.automatic)
        {
            if (Input.GetMouseButton(0) && currentMunition > 0)
            {
                ShootBullet();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentMunition > 0)
            {
                ShootBullet();
            }
        }

    }

    private void ShootBullet()
    {
        Vector3 shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - transform.position;
        GameObject bulletInstance = Instantiate(bulletScriptable.prefab, bulletSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();


        bulletRigidbody.velocity = new Vector2(shootDirection.x * weaponScriptable.speed, shootDirection.y * weaponScriptable.speed);

        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        currentMunition--;
        if (currentMunition <= 0) currentMunition = 0;

    }

    private void RotateWeapon()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        if (mousePos.x > player.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (mousePos.x < player.position.x)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
    }



    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        if (gunEnd != null)
            Gizmos.DrawLine(gunEnd.position, mousePos);
    }
}
