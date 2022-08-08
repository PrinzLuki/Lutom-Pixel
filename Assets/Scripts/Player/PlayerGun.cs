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


    [Header("Weapon")]
    public WeaponScriptableObject weaponScriptable;
    public Weapon gun;
    [SyncVar]
    public GameObject currentWeaponGameObject;
    public LayerMask weaponMask;
    public float throwForce;

    public Quaternion weaponRotation;
    public bool flipWeapon;

    public Vector2 throwDirection;
    public Vector3 shootDirection;



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
            mousePos = playerCamera.ScreenToWorldPoint(inputMousePos);
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


    #region Shoot

    private void Shoot()
    {
        if (currentWeaponGameObject == null) return;

        shootDirection = mousePos - currentWeaponGameObject.transform.position;

        if (Input.GetMouseButton(0) && gun.currentMunition > 0 && weaponScriptable.automatic)
        {
            gun.CmdShootBullet(this.gameObject, shootDirection);
        }
        else if (Input.GetMouseButtonDown(0) && gun.currentMunition > 0)
        {
            gun.CmdShootBullet(this.gameObject, shootDirection);
        }
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
        Weapon gun = playerGun.currentWeaponGameObject.GetComponent<Weapon>();
        playerGun.gun = gun;
        playerGun.canShoot = true;


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
            currentWeaponGameObject.GetComponent<Weapon>().CmdDrop(this, throwDirection);
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

        playerGun.canShoot = false;

        if (playerGun.currentWeaponGameObject == null) return;

        var gunRigid = playerGun.currentWeaponGameObject.GetComponent<Rigidbody2D>();

        gunRigid.velocity = direction * playerGun.throwForce;

        gunRigid.bodyType = RigidbodyType2D.Dynamic;
        playerGun.currentWeaponGameObject.GetComponent<Collider2D>().enabled = true;

        playerGun.currentWeaponGameObject.GetComponent<Weapon>().parent = null;
        playerGun.gun = null;
        playerGun.currentWeaponGameObject = null;
    }

    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, pickUpRadius);
        if (gun != null)
            Gizmos.DrawLine(gun.gunEnd.position, mousePos);
    }

    #endregion
}
