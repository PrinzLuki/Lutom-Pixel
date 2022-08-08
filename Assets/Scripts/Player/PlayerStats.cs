using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : NetworkBehaviour, IDamageable
{
    [SerializeField, SyncVar(hook = nameof(CmdServerSyncMaxHealth))] private float maxHealth;
    [SerializeField, SyncVar(hook = nameof(CmdServerSyncHealth))] private float health;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float minSpeed = 5.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float maxJumpPower = 12.0f;
    [SerializeField] private float minJumpPower = 6.0f;
    [SerializeField] private float jumpPower = 6.0f;
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private bool isImmortal;
    [SerializeField] private bool showGizmos;

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
            this.gameObject.SetActive(false);
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
