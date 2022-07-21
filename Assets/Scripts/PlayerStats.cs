using Mirror;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxJumpPower;
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private bool showGizmos;

    public float Health { get => health; set => health = value; }
    public float Speed { get => speed; set => speed = value; }
    public float JumpPower { get => jumpPower; set => jumpPower = value; }

    public float MaxHealth { get => maxHealth; }
    public float MaxSpeed { get => maxSpeed; }
    public float MaxJumpPower { get => maxJumpPower; }

    public WeaponScriptableObject weaponScriptable;
    public GameObject currentWeapon;

    private void Start()
    {
        if (!hasAuthority) return;

        if (weaponScriptable != null)
        {
            CmdSpawnGunOnServer(this.gameObject);
        }

    }


    [Command]
    void CmdSpawnGunOnServer(GameObject trg)
    {
        PlayerStats player = trg.GetComponent<PlayerStats>();
        player.currentWeapon = Instantiate(player.weaponScriptable.weaponPrefab/*, trg.transform, false*/);
        Gun currentWeaponStats = player.currentWeapon.GetComponent<Gun>();
        currentWeaponStats.isEquipped = true;
        //currentWeaponStats.currentMunition = weaponScriptable.munition;
        //currentWeaponStats.currentSpeed = weaponScriptable.speed;

        currentWeaponStats.gunEnd = player.currentWeapon.transform.GetChild(0);
        currentWeaponStats.bulletSpawn = player.currentWeapon.transform.GetChild(1);
        player.currentWeapon.transform.SetParent(trg.transform);
        player.currentWeapon.transform.localPosition = Vector3.zero;
        //currentWeapon.GetComponent<NetworkIdentity>().AssignClientAuthority(netIdentity.connectionToClient);
        NetworkServer.Spawn(player.currentWeapon, trg);
        //RpcSpawnGun(trg);
    }

    //[ClientRpc]
    //void RpcSpawnGun(GameObject trg)
    //{
    //    if (NetworkServer.active) return;
    //    PlayerStats player = trg.GetComponent<PlayerStats>();
    //    player.currentWeapon = Instantiate(player.weaponScriptable.weaponPrefab/*, trg.transform, false*/);
    //    player.currentWeapon.transform.SetParent(trg.transform);
    //    player.currentWeapon.transform.localPosition = Vector3.zero;
    //}

    //[Client]
    //private void Update()
    //{
    //    //IsInteracting();

    //}


    //private void IsInteracting()
    //{
    //    var colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
    //    foreach (var collider in colliders)
    //    {
    //        if (collider.GetComponent<IInteractable>() != null)
    //        {
    //            if (InputManager.instance.Interact())
    //            {
    //                collider.GetComponent<IInteractable>().Interact(this);
    //            }
    //        }
    //    }
    //}


    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, interactionRadius);
        }
    }

}
