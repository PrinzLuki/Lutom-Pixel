using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Vector3 mousePos;
    [SerializeField] private Transform player;
    public Transform gunEnd;
    public Transform bulletSpawn;
    public PhysicsMaterial2D bouncyMAT;
    public SpriteRenderer weaponSprite;

    [Header("Weapon")]
    public BulletScriptableObject bulletScriptable;
    public float speed;
    public float munition;

    private void Start()
    {
        player = GetComponentInParent<PlayerMovement>().transform;

        weaponSprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateWeapon();

        if (bulletScriptable.automatic)
        {
            if (Input.GetMouseButton(0) && munition > 0)
            {
                ShootBullet();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && munition > 0)
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


        bulletRigidbody.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);

        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        munition--;
        if (munition <= 0) munition = 0;

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
        Gizmos.DrawLine(gunEnd.position, mousePos);
    }
}
