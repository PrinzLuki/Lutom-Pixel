using Mirror;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerParent;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 inputMousePos;

    [Header("Stats")]
    public float currentMunition;
    public float currentSpeed;

    [Header("Weapon")]
    public WeaponScriptableObject weaponScriptable;
    [SyncVar]
    public GameObject currentWeaponGameObject;
    public Transform weaponGunEnd;
    public Transform weaponBulletSpawn;
    public LayerMask weaponMask;
    public float throwForce;

    public Quaternion weaponRotation;
    public bool flipWeapon;

    public Vector2 throwDirection;

    [Header("Bullet")]
    public BulletScriptableObject bulletScriptable;
    public GameObject currentBulletGameObject;


    [Header("Gizmos")]
    [SerializeField] private float pickUpRadius = 1f;
    [SerializeField] private bool showGizmos;

    [Client]
    void Update()
    {

        if (!hasAuthority) return;
        if (IsMouseOverGameWindow)
        {
            inputMousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(inputMousePos);
        }
        if (currentWeaponGameObject == null)
        {
            PickUpWeapon(this.transform);
        }
        if (currentWeaponGameObject != null)
        {
            RotateWeapon();
            DropWeapon();
            //RotateWeaponOnClient(this.gameObject, currentWeapon.transform);
            //CmdRotateWeaponOnServer(this.gameObject);

            //if (weaponScriptable.automatic)
            //{
            //    if (Input.GetMouseButton(0) && currentMunition > 0)
            //    {
            //        CmdShootBullet();

            //    }
            //}
            //else
            //{
            //    if (Input.GetMouseButtonDown(0) && currentMunition > 0)
            //    {
            //        CmdShootBullet();
            //    }
            //}
        }



    }

    #region IsMouseOverGameWindow Check
    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    #endregion

    #region Get Set WeaponStats
    public void GetWeaponStats(GameObject weapon)
    {
        weaponScriptable = weapon.GetComponent<Gun>().weaponScriptableObject;

        currentMunition = weaponScriptable.munition;
        currentSpeed = weaponScriptable.speed;
    }

    public void ResetWeaponStats(GameObject weapon)
    {
        weaponScriptable = null;

        currentMunition = 0;
        currentSpeed = 0;
    }

    #endregion

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
    private void CmdShootBullet()
    {
        RpcShootBullet();
    }

    [ClientRpc]
    private void RpcShootBullet()
    {

        var shootDirection = mousePos;
        shootDirection = shootDirection - currentWeaponGameObject.transform.position;
        GameObject bulletInstance = Instantiate(bulletScriptable.prefab, weaponBulletSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0)));

        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = new Vector2(shootDirection.x * currentSpeed, shootDirection.y * currentSpeed);

        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        currentMunition--;
        if (currentMunition <= 0) currentMunition = 0;
    }
    #endregion

    #region Weapon Rotation
    private Quaternion GetWeaponRotation(Vector3 inputMouse, Transform rotationTrg)
    {
        var dir = inputMouse - Camera.main.WorldToScreenPoint(rotationTrg.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        return q;
    }


    public void RotateWeapon()
    {
        weaponRotation = GetWeaponRotation(inputMousePos, currentWeaponGameObject.transform);

        if (mousePos.x > transform.position.x)
        {
            flipWeapon = false;
        }

        if (mousePos.x < transform.position.x)
        {
            flipWeapon = true;
        }


        CmdRotateWeaponOnServer(currentWeaponGameObject.gameObject, flipWeapon, weaponRotation);
    }


    [Command]
    private void CmdRotateWeaponOnServer(GameObject weapon, bool flipValue, Quaternion rotation)
    {
        RpcRotateWeaponOnServer(weapon, flipValue, rotation);
    }

    [ClientRpc]
    private void RpcRotateWeaponOnServer(GameObject weapon, bool flipValue, Quaternion rotation)
    {
        if (weapon == null) return;
        Transform weaponTransform = weapon.transform;

        if (!flipValue)
        {
            weaponTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            weaponTransform.localScale = new Vector3(1, -1, 1);
        }


        weaponTransform.localRotation = rotation;

    }

    #endregion

    [Command(requiresAuthority = false)]
    public void CmdPickUpGunOnServer(GameObject trg, GameObject weapon)
    {
        RpcPickUpGunOnServer(trg, weapon);
    }

    [ClientRpc]
    public void RpcPickUpGunOnServer(GameObject trg, GameObject weapon)
    {
        PlayerGun playerGun = trg.GetComponent<PlayerGun>();

        playerGun.currentWeaponGameObject = weapon;
        Gun gun = playerGun.currentWeaponGameObject.GetComponent<Gun>();
        playerGun.weaponGunEnd = gun.gunEnd;
        playerGun.weaponBulletSpawn = gun.bulletSpawn;

        playerGun.GetWeaponStats(weapon);

        playerGun.currentWeaponGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        playerGun.currentWeaponGameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void PickUpWeapon(Transform player)
    {
        var playerGun = player.GetComponent<PlayerGun>();
        var collider = Physics2D.OverlapCircle(player.position, pickUpRadius, weaponMask);

        if (collider == null) return;

        if (collider.GetComponent<IWeapon>() != null)
        {

            if (InputManager.instance.Interact() && playerGun.currentWeaponGameObject == null)
            {
                collider.GetComponent<IWeapon>().CmdPickUp(this);
            }
        }

    }

    private void DropWeapon()
    {
        throwDirection = new Vector2(
           mousePos.x - transform.position.x,
           mousePos.y - transform.position.y
       ).normalized;

        if (InputManager.instance.Drop() && currentWeaponGameObject != null)
        {
            //CmdDropGunOnServer(this.gameObject, throwDirection);
            currentWeaponGameObject.GetComponent<Gun>().CmdDrop(this, throwDirection);
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdDropGunOnServer(GameObject trg, Vector2 direction)
    {
        RpcDropGunOnServer(trg, direction);
    }

    [ClientRpc]
    public void RpcDropGunOnServer(GameObject trg, Vector2 direction)
    {
        PlayerGun playerGun = trg.GetComponent<PlayerGun>();

        playerGun.weaponGunEnd = null;
        playerGun.weaponBulletSpawn = null;

        if (playerGun.currentWeaponGameObject == null) return;

        var gunRigid = playerGun.currentWeaponGameObject.GetComponent<Rigidbody2D>();

        playerGun.ResetWeaponStats(playerGun.currentWeaponGameObject);

        gunRigid.velocity = direction * playerGun.throwForce;

        gunRigid.bodyType = RigidbodyType2D.Dynamic;
        playerGun.currentWeaponGameObject.GetComponent<Collider2D>().enabled = true;

        playerGun.currentWeaponGameObject.GetComponent<Gun>().parent = null;
        playerGun.currentWeaponGameObject = null;
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
