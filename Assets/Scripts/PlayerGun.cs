using Mirror;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerParent;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 inputMousePos;


    [Header("Weapon")]
    public WeaponScriptableObject weaponScriptable;
    public GameObject currentWeapon;
    public Transform weaponGunEnd;
    public Transform weaponBulletSpawn;
    public bool hasWeapon;

    [Header("Gizmos")]
    [SerializeField] private float pickUpRadius = 1f;
    [SerializeField] private bool showGizmos;


    private void Start()
    {
        if (!hasAuthority) return;

        #region Shoot
        //if (weaponScriptable != null)
        //{
        //    currentMunition = weaponScriptable.munition;
        //    currentSpeed = weaponScriptable.speed;

        //    weaponPrefab = Instantiate(weaponScriptable.weaponPrefab, this.transform, false);
        //    weaponPrefab.transform.parent = weaponHolder.transform;

        //    gunEnd = weaponPrefab.transform.GetChild(0);
        //    bulletSpawn = weaponPrefab.transform.GetChild(1);
        //}
        #endregion
    }

    [Client]
    void Update()
    {

        if (!hasAuthority) return;
        if (IsMouseOverGameWindow)
        {
            inputMousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(inputMousePos);
        }

        PickUpWeapon(this.transform);

        if (currentWeapon != null)
        {
            RotateWeaponOnClient(this.gameObject, currentWeapon.transform);
            CmdRotateWeaponOnServer(this.gameObject, currentWeapon.transform);
        }

        #region Shoot
        //if (weaponScriptable == null) return;

        //if (weaponScriptable.automatic)
        //{
        //    if (Input.GetMouseButton(0) && currentMunition > 0)
        //    {
        //        //ShootBullet();
        //        //CmdShootBullet();

        //    }
        //}
        //else
        //{
        //    if (Input.GetMouseButtonDown(0) && currentMunition > 0)
        //    {
        //        //ShootBullet();
        //        //CmdShootBullet();
        //    }
        //}
        #endregion

    }

    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    #region Shoot
    //private void ShootBullet()
    //{
    //    var shootDirection = mousePos;
    //    shootDirection = shootDirection - weaponHolder.transform.position;
    //    GameObject bulletInstance = Instantiate(bulletScriptable.prefab, bulletSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0)));

    //    Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
    //    bulletRigidbody.velocity = new Vector2(shootDirection.x * weaponScriptable.speed, shootDirection.y * weaponScriptable.speed);

    //    Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

    //    currentMunition--;
    //    if (currentMunition <= 0) currentMunition = 0;

    //    //NetworkServer.Spawn(bulletInstance, this.gameObject);
    //}

    //[Command]
    //private void CmdShootBullet()
    //{
    //    RpcShootBullet();
    //}

    //[ClientRpc]
    //private void RpcShootBullet() => ShootBullet();
    #endregion

    private Quaternion RotateWeapon(Vector3 inputMouse, Transform rotationTrg)
    {
        var dir = inputMouse - Camera.main.WorldToScreenPoint(rotationTrg.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        return q;
    }

    private void RotateWeaponOnClient(GameObject player, Transform weaponTransform)
    {
        PlayerGun playerGun = player.GetComponent<PlayerGun>();

        Quaternion rotationOfWeapon = playerGun.RotateWeapon(playerGun.inputMousePos, weaponTransform);

        if (playerGun.mousePos.x > player.transform.position.x)
        {
            weaponTransform.localScale = new Vector3(1, 1, 1);
        }

        if (playerGun.mousePos.x < player.transform.position.x)
        {
            weaponTransform.localScale = new Vector3(1, -1, 1);
        }

        weaponTransform.rotation = rotationOfWeapon;
        weaponTransform.position = player.transform.position;
    }


    [Command]
    private void CmdRotateWeaponOnServer(GameObject player, Transform weaponTransform)
    {
        RpcRotateWeaponOnServer(player, weaponTransform);
    }

    [ClientRpc]
    private void RpcRotateWeaponOnServer(GameObject player, Transform weaponTransform)
    {
        PlayerGun playerGun = player.GetComponent<PlayerGun>();
        //Transform weaponTransform = playerGun.currentWeapon.transform;

        Quaternion rotationOfWeapon = playerGun.RotateWeapon(playerGun.inputMousePos, weaponTransform);

        if (playerGun.mousePos.x > player.transform.position.x)
        {
            weaponTransform.localScale = new Vector3(1, 1, 1);
        }

        if (playerGun.mousePos.x < player.transform.position.x)
        {
            weaponTransform.localScale = new Vector3(1, -1, 1);
        }

        weaponTransform.rotation = rotationOfWeapon;
        weaponTransform.position = player.transform.position;

    }

    public void PickUpGunOnClient(GameObject player, GameObject weapon)
    {
        PlayerGun playerGun = player.GetComponent<PlayerGun>();

        playerGun.currentWeapon = weapon;
        Gun gun = playerGun.currentWeapon.GetComponent<Gun>();
        playerGun.weaponGunEnd = gun.gunEnd;
        playerGun.weaponBulletSpawn = gun.bulletSpawn;
        playerGun.hasWeapon = true;

        playerGun.currentWeapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        playerGun.currentWeapon.GetComponent<Collider2D>().enabled = false;
    }

    [Command]
    public void CmdPickUpGunOnServer(GameObject trg, GameObject weapon)
    {
        RpcPickUpGunOnServer(trg, weapon);
    }

    [ClientRpc]
    public void RpcPickUpGunOnServer(GameObject trg, GameObject weapon)
    {
        PlayerGun playerGun = trg.GetComponent<PlayerGun>();

        playerGun.currentWeapon = weapon;
        Gun gun = playerGun.currentWeapon.GetComponent<Gun>();
        playerGun.weaponGunEnd = gun.gunEnd;
        playerGun.weaponBulletSpawn = gun.bulletSpawn;
        playerGun.hasWeapon = true;

        playerGun.currentWeapon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        playerGun.currentWeapon.GetComponent<Collider2D>().enabled = false;
    }

    private void PickUpWeapon(Transform player)
    {
        var playerGun = player.GetComponent<PlayerGun>();
        var colliders = Physics2D.OverlapCircleAll(player.position, pickUpRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<IWeapon>() != null)
            {
                if (InputManager.instance.Interact() && !playerGun.hasWeapon)
                {
                    collider.GetComponent<IWeapon>().PickUp(this);
                    collider.GetComponent<IWeapon>().CmdPickUp(this);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, pickUpRadius);
        if (weaponGunEnd != null)
            Gizmos.DrawLine(weaponGunEnd.position, mousePos);
    }
}
