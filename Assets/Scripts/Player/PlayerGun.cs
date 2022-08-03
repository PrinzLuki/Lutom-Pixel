using Mirror;
using UnityEngine;

public class PlayerGun : NetworkBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 inputMousePos;

    [Header("Stats")]
    public bool canShoot;
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
    public Vector3 shootDirection;

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
        if (playerCamera == null) { Debug.Log("Player Camera is missing"); return; } 
        if (IsMouseOverGameWindow)
        {
            inputMousePos = Input.mousePosition;
            mousePos = playerCamera.ScreenToWorldPoint(inputMousePos); /*Camera.main.ScreenToWorldPoint(inputMousePos);*/
        }
        if (currentWeaponGameObject == null)
        {
            PickUpWeapon(this.transform);
        }
        else
        {
            RotateWeapon();
            DropWeapon();
            if (!canShoot) return;
            Shoot();
        }


    }

    #region IsMouseOverGameWindow Check
    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }

    #endregion

    #region Get Set WeaponStats

    /// <summary>
    /// Gets the weapons stats from the picked up scriptable weapon object (sets the current munition and speed)
    /// </summary>
    /// <param name="weapon"></param>
    public void GetWeaponStats(GameObject weapon)
    {
        weaponScriptable = weapon.GetComponent<Gun>().weaponScriptableObject;

        currentMunition = weaponScriptable.munition;
        currentSpeed = weaponScriptable.speed;
    }

    /// <summary>
    /// Resets the weapons stats from the (sets the current munition and speed to null/0)
    /// </summary>
    /// <param name="weapon"></param>
    public void ResetWeaponStats(GameObject weapon)
    {
        weaponScriptable = null;

        currentMunition = 0;
        currentSpeed = 0;
    }

    public void ReloadWeapon()
    {
        currentMunition = weaponScriptable.munition;
    }

    #endregion

    #region Shoot

    private void Shoot()
    {
        if (currentWeaponGameObject == null && currentBulletGameObject == null) return;

        shootDirection = mousePos - currentWeaponGameObject.transform.position;

        if (Input.GetMouseButton(0) && currentMunition > 0 && weaponScriptable.automatic)
        {
            CmdShootBullet(this.gameObject, shootDirection);
        }
        else if (Input.GetMouseButtonDown(0) && currentMunition > 0)
        {
            CmdShootBullet(this.gameObject, shootDirection);
        }
    }

    [Command]
    private void CmdShootBullet(GameObject player, Vector3 direction)
    {
        RpcShootBullet(player, direction);
    }

    [ClientRpc]
    private void RpcShootBullet(GameObject player, Vector3 direction)
    {
        PlayerGun playerGun = player.GetComponent<PlayerGun>();

        if (playerGun.weaponBulletSpawn == null) return;
        GameObject bulletInstance = Instantiate(playerGun.bulletScriptable.prefab, playerGun.weaponBulletSpawn.position, playerGun.weaponBulletSpawn.rotation);

        Rigidbody2D bulletRigidbody = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = new Vector2(direction.x * playerGun.currentSpeed, direction.y * playerGun.currentSpeed);

        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        playerGun.currentMunition--;
        if (playerGun.currentMunition <= 0) playerGun.currentMunition = 0;
    }
    #endregion

    #region Weapon Rotation

    /// <summary>
    /// Returns the quaternion rotation of the 2D weapon  (target)
    /// </summary>
    /// <param name="inputMouse"></param>
    /// <param name="rotationTrg"></param>
    /// <returns></returns>
    private Quaternion GetWeaponRotation(Vector3 inputMouse, Transform rotationTrg)
    {
        var dir = inputMouse - playerCamera.WorldToScreenPoint(rotationTrg.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        return q;
    }

    /// <summary>
    /// Rotates the weapon and calls the function on the server too
    /// </summary>
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


    /// <summary>
    /// Rotates the Weapon on the server and calls a clientrpc function
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="flipValue"></param>
    /// <param name="rotation"></param>
    [Command]
    private void CmdRotateWeaponOnServer(GameObject weapon, bool flipValue, Quaternion rotation)
    {
        RpcRotateWeaponOnServer(weapon, flipValue, rotation);
    }

    /// <summary>
    /// Rotates the same weapon as on the client but so, that everyone else sees it
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="flipValue"></param>
    /// <param name="rotation"></param>

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

    #region PickUp

    /// <summary>
    /// Picks up the weapon - calls the IWeapon Pick Up function
    /// </summary>
    /// <param name="player"></param>
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

    /// <summary>
    /// Picks up the weapon on the Server
    /// </summary>
    /// <param name="trg"></param>
    /// <param name="weapon"></param>
    [Command(requiresAuthority = false)]
    public void CmdPickUpGunOnServer(GameObject trg, GameObject weapon)
    {
        RpcPickUpGunOnServer(trg, weapon);
    }


    /// <summary>
    /// Sends everybody an rpc that the weapon has been picked up
    /// </summary>
    /// <param name="trg"></param>
    /// <param name="weapon"></param>
    [ClientRpc]
    public void RpcPickUpGunOnServer(GameObject trg, GameObject weapon)
    {
        PlayerGun playerGun = trg.GetComponent<PlayerGun>();

        playerGun.currentWeaponGameObject = weapon;
        Gun gun = playerGun.currentWeaponGameObject.GetComponent<Gun>();
        playerGun.weaponGunEnd = gun.gunEnd;
        playerGun.weaponBulletSpawn = gun.bulletSpawn;
        playerGun.canShoot = true;

        playerGun.GetWeaponStats(weapon);

        playerGun.currentWeaponGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        playerGun.currentWeaponGameObject.GetComponent<Collider2D>().enabled = false;
    }
    #endregion

    #region Drop

    /// <summary>
    /// Drops the weapon - calls the IWeapon Drop function
    /// </summary>
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


    /// <summary>
    /// Drops the weapon on the server
    /// </summary>
    /// <param name="trg"></param>
    /// <param name="direction"></param>
    [Command(requiresAuthority = false)]
    public void CmdDropGunOnServer(GameObject trg, Vector2 direction)
    {
        RpcDropGunOnServer(trg, direction);
    }


    /// <summary>
    /// Send everybody an rpc that the weapon has been dropped
    /// </summary>
    /// <param name="trg"></param>
    /// <param name="direction"></param>
    [ClientRpc]
    public void RpcDropGunOnServer(GameObject trg, Vector2 direction)
    {
        PlayerGun playerGun = trg.GetComponent<PlayerGun>();

        playerGun.weaponGunEnd = null;
        playerGun.weaponBulletSpawn = null;
        playerGun.canShoot = false;

        if (playerGun.currentWeaponGameObject == null) return;

        var gunRigid = playerGun.currentWeaponGameObject.GetComponent<Rigidbody2D>();

        playerGun.ResetWeaponStats(playerGun.currentWeaponGameObject);

        gunRigid.velocity = direction * playerGun.throwForce;

        gunRigid.bodyType = RigidbodyType2D.Dynamic;
        playerGun.currentWeaponGameObject.GetComponent<Collider2D>().enabled = true;

        playerGun.currentWeaponGameObject.GetComponent<Gun>().parent = null;
        playerGun.currentWeaponGameObject = null;
    }

    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, pickUpRadius);
        if (weaponGunEnd != null)
            Gizmos.DrawLine(weaponGunEnd.position, mousePos);
    }

    #endregion
}
