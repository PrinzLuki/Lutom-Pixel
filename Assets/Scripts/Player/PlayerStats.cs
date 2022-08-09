using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos;

    [Header("Respawn/Death")]
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float deathUpForce = 1f;

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

        onHealthChanged?.Invoke(Health, MaxHealth);

        if (health <= 0)
        {
            health = 0;
            KillPlayer();
            StartCoroutine(WaitTillRespawn());
        }
    }

    public void GetHealth(float healValue)
    {
        if (!hasAuthority) return;
        if (isImmortal) return;

        health += healValue;

        onHealthChanged?.Invoke(Health, MaxHealth);

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }


    private void KillPlayer()
    {
        var playerUI = GetComponent<PlayerUI>();
        playerUI.deadImage.gameObject.SetActive(true);

        playerUI.enabled = false;

        //maybe make Skull jump a bit

        GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerStats>().enabled = false;
        GetComponent<PlayerGun>().enabled = false;

        GetComponent<SpriteRenderer>().sprite = playerUI.deadImage.sprite;

        GetComponent<Animator>().enabled = false;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void RespawnPlayer()
    {
        transform.position = spawnPoint;

        var playerUI = GetComponent<PlayerUI>();

        playerUI.deadImage.gameObject.SetActive(false);

        playerUI.enabled = true;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerStats>().enabled = true;
        GetComponent<PlayerGun>().enabled = true;

        GetComponent<Animator>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


        GetHealth(MaxHealth);
    }

    IEnumerator WaitTillRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnPlayer();
    }

    public void ResetStats()
    {
        speed = minSpeed;
        jumpPower = minJumpPower;
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
        var colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<IInteractable>() != null)
            {
                if (InputManager.instance.Interact())
                {
                    collider.GetComponent<IInteractable>().Interact(this.gameObject);
                }
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
