using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : NetworkBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private bool isImmortal;
    [SerializeField, SyncVar(hook = nameof(CmdServerSyncMaxHealth))] private float maxHealth;
    [SerializeField, SyncVar(hook = nameof(CmdServerSyncHealth))] private float health;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float minSpeed = 5.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxJumpPower = 12.0f;
    [SerializeField] private float minJumpPower = 6.0f;
    [SerializeField] private float jumpPower = 6.0f;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactionMask;
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos;


    [Header("Respawn/Death")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float respawnDelay;
    //[SerializeField] private float deathUpForce = 1f;

    public UnityEvent<float, float> onHealthChanged;

    public float Health { get => health; set => health = value; }
    public float Speed { get => speed; set => speed = value; }
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public float MaxHealth { get => maxHealth; }
    public float MaxSpeed { get => maxSpeed; }
    public float MinSpeed { get => minSpeed; }
    public float MaxJumpPower { get => maxJumpPower; }
    public float MinJumpPower { get => minJumpPower; }

    public ItemType currentItemType;

    public void GetDamage(float dmg)
    {
        if (!hasAuthority) return;
        if (isImmortal) return;

        health -= dmg;

        if (health <= 0)
        {
            health = 0;
            CmdKillPlayer(this.gameObject);
            StartCoroutine(WaitTillRespawn());
        }
        onHealthChanged?.Invoke(Health, MaxHealth);
    }

    public void GetHealth(float healValue)
    {
        if (!hasAuthority) return;
        if (isImmortal) return;

        health += healValue;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        onHealthChanged?.Invoke(Health, MaxHealth);
    }

    [Command]
    private void CmdKillPlayer(GameObject player)
    {
        RpcKillPlayer(player);
    }

    [ClientRpc]
    private void RpcKillPlayer(GameObject player)
    {
        var playerUI = player.GetComponent<PlayerUI>();
        var playerGun = player.GetComponent<PlayerGun>();
        playerUI.deadImage.gameObject.SetActive(true);

        playerUI.enabled = false;

        player.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerStats>().enabled = false;
        playerGun.CmdDropGunOnServer(this.gameObject, playerGun.throwDirection);
        playerGun.enabled = false;

        player.GetComponent<SpriteRenderer>().sprite = playerUI.deadImage.sprite;

        player.GetComponent<Animator>().enabled = false;

        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    [Command]
    private void CmdRespawnPlayer(GameObject player)
    {
        RpcRespawnPlayer(player);
    }

    [ClientRpc]
    private void RpcRespawnPlayer(GameObject player)
    {
        player.transform.position = player.GetComponent<PlayerStats>().spawnPoint;

        var playerUI = player.GetComponent<PlayerUI>();
        var playerStats = player.GetComponent<PlayerStats>();

        playerUI.deadImage.gameObject.SetActive(false);

        playerUI.enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        playerStats.enabled = true;
        player.GetComponent<PlayerGun>().enabled = true;

        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;

        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        playerStats.ResetStats();
        playerStats.GetHealth(MaxHealth);
    }

    IEnumerator WaitTillRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        CmdRespawnPlayer(this.gameObject);
    }

    public void ResetStats()
    {
        speed = minSpeed;
        jumpPower = minJumpPower;

        GetComponent<PlayerUI>().UpdateUIStats(this, ItemType.None);
    }


    [Command(requiresAuthority = false)]
    public void CmdServerSyncHealth(float oldHealth, float newHealth)
    {
        health = newHealth;
    }

    [Command]
    public void CmdServerSyncMaxHealth(float oldHealth, float newHealth)
    {
        maxHealth = newHealth;
    }

    private void Start()
    {
        spawnPoint = transform.position;
    }

    [Client]
    private void Update()
    {
        IsInteracting();
    }


    private void IsInteracting()
    {
        var collider = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionMask);

        if (collider == null) return;

        if (collider.GetComponent<IInteractable>() != null)
        {
            if (InputManager.instance.Interact())
            {
                collider.GetComponent<IInteractable>().Interact(this.gameObject);
            }
        }

    }


    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, interactionRadius);
        }
    }

}

public enum ItemType
{
    HealItem,
    SpeedItem,
    JumpItem,
    None
}
