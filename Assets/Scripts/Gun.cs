using Mirror;
using UnityEngine;

public class Gun : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 inputMousePos;
    //[SerializeField] private Transform weaponHolder;
    [Header("Instantiated Prefab")]
    [SerializeField] private GameObject weaponPrefab;
    public Transform gunEnd;
    public Transform bulletSpawn;
    public PhysicsMaterial2D bouncyMAT;


    [Header("Weapon")]
    public BulletScriptableObject bulletScriptable;
    public float currentMunition;
    public float currentSpeed;
    //public bool automatic;
    public bool flipY;


    private void Start()
    {
        //if (weaponScriptable != null)
        //{
        //    currentMunition = weaponScriptable.munition;
        //    currentSpeed = weaponScriptable.speed;

        //    weaponPrefab = Instantiate(weaponScriptable.weaponPrefab, this.transform, false);
        //    weaponPrefab.transform.parent = weaponHolder.transform;

        //    gunEnd = weaponPrefab.transform.GetChild(0);
        //    bulletSpawn = weaponPrefab.transform.GetChild(1);
        //}

    }

    [Client]
    void FixedUpdate()
    {

        if (!hasAuthority) return;
        if (IsMouseOverGameWindow)
        {
            inputMousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(inputMousePos);
        }
        RotateWeaponOnClient(RotateWeapon(inputMousePos, transform));
        CmdRotateWeaponOnServer(this.gameObject);

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


    }

    bool IsMouseOverGameWindow { get { return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); } }


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

    private Quaternion RotateWeapon(Vector3 mouseInput, Transform rotationTrg)
    {
        var dir = mouseInput - Camera.main.WorldToScreenPoint(rotationTrg.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //weaponHolder.transform.rotation = q;
        return q;

        //if (mousePos.x > transform.position.x)
        //{
        //    weaponHolder.transform.localScale = new Vector3(1, 1, 1);
        //}

        //if (mousePos.x < transform.position.x)
        //{
        //    weaponHolder.transform.localScale = new Vector3(1, -1, 1);

        //}
    }


    private void RotateWeaponOnClient(Quaternion rotationOfWeapon)
    {
        //if (mousePos.x > transform.position.x)
        //{
        //    //transform.localScale = new Vector3(1, 1, 1);
        //    flipY = false;
        //}

        //if (mousePos.x < transform.position.x)
        //{
        //    //transform.localScale = new Vector3(1, -1, 1);
        //    flipY = true;
        //}

        transform.rotation = rotationOfWeapon;
    }


    [Command]
    private void CmdRotateWeaponOnServer(GameObject trg)
    {
        RpcRotateWeaponOnServer(trg);
    }

    [ClientRpc]
    private void RpcRotateWeaponOnServer(GameObject trg)
    {
        var trgGun = trg.GetComponent<Gun>();

        //if (trgGun.mousePos.x > trg.transform.position.x)
        //{
        //    //trg.transform.localScale = new Vector3(1, 1, 1);
        //    trgGun.flipY = false;
        //}

        //if (trgGun.mousePos.x < trg.transform.position.x)
        //{
        //    //trg.transform.localScale = new Vector3(1, -1, 1);
        //    trgGun.flipY = true;
        //}

        trgGun.transform.rotation = RotateWeapon(trgGun.inputMousePos, trgGun.transform);
        //var dir = trg.GetComponent<Gun>().inputMousePos - Camera.main.WorldToScreenPoint(trg.GetComponent<Gun>().weaponHolder.position);
        //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //trg.GetComponent<Gun>().weaponHolder.rotation = q;
    }



    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        if (gunEnd != null)
            Gizmos.DrawLine(gunEnd.position, mousePos);

        //Gizmos.DrawLine(transform.position, Input.mousePosition - Camera.main.WorldToScreenPoint(weaponHolder.transform.position));
    }
}
