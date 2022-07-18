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



    //[Client]
    //private void Update()
    //{
    //    //IsInteracting();
    //    CmdIsInteracting();

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


    //[Command]
    //private void CmdIsInteracting()
    //{
    //    RpcIsInteracting();
    //}

    //[ClientRpc]
    //private void RpcIsInteracting() => IsInteracting();


    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, interactionRadius);
        }
    }

}
