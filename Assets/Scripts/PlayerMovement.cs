using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _playerRigidbody;

    [Header("Player Overall")]
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool isGrounded;
    [Header("Player Input")]
    [SerializeField] private float horizontalInput;
    [Header("Gizmos")]
    public LayerMask groundLayer;
    [SerializeField] private bool showGizmos;
    [SerializeField] private Vector3 groundCheckPosition = new Vector3(-0.003f, -0.4f, 0);
    [SerializeField] private Vector3 groundCheckScale = new Vector3(0.66f, 0.2f, 0);
    [SerializeField] private float interactionRadius;

    [Header("Animations & Effects")]
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        if (_playerRigidbody == null)
        {
            Debug.LogError("Player is missing a Rigidbody2D component");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Player is missing an Animator component");
        }

        //Child of transform is "Sprite" and the child of that is our SpriteRenderer
        _spriteRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        if (_animator == null)
        {
            Debug.LogError("Player is missing a SpriteRenderer component");
        }
    }
    private void Update()
    {
        MovePlayer();
        IsGrounded();
        Jump();

        IsInteracting();
    }

    /// <summary>
    /// Moves the player with the multiplied amount of "playerSpeed"
    /// </summary>
    private void MovePlayer()
    {
        horizontalInput = InputManager.instance.CurrentPosition();
        _playerRigidbody.velocity = new Vector2(horizontalInput * playerSpeed, _playerRigidbody.velocity.y);

        //Animation
        FlipPlayer();
        AnimatorSetRunAnimation();
    }

    /// <summary>
    /// Jump function for the player - Adds velocity to the y-axis of player
    /// </summary>
    private void Jump()
    {
        if (InputManager.instance.IsJumping())
        {
            if (isGrounded)
            {
                _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
                _playerRigidbody.velocity = (new Vector2(_playerRigidbody.velocity.x, jumpPower));
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
                    _playerRigidbody.velocity = (new Vector2(_playerRigidbody.velocity.x, jumpPower));
                }
            }
        }
    }

    /// <summary>
    /// Returns boolean true - if player is grounded (on the ground)
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        var isTouchingGround = Physics2D.OverlapBox(transform.position + groundCheckPosition, groundCheckScale, 0, groundLayer);

        //velocity.y == 0 damit man nicht auf der wand klettern kann
        if (isTouchingGround && _playerRigidbody.velocity.y == 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
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
                    collider.GetComponent<IInteractable>().Interact(this);
                }
            }
        }
    }

    /// <summary>
    /// Checks Input of player and Flips the Sprite accordingly
    /// </summary>
    /// <returns></returns>
    private void FlipPlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // mouseCircle.position = mousePos;
        if (mousePos.x < transform.position.x) _spriteRenderer.flipX = true;
        if (mousePos.x > transform.position.x) _spriteRenderer.flipX = false;
    }

    /// <summary>
    /// Checks if player is moving, if yes - the animation will be set and activated
    /// </summary>
    private void AnimatorSetRunAnimation()
    {
        //Animation Run
        if (horizontalInput != 0)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = new Color(0, 1, 0, 0.25f);
            Gizmos.DrawCube(transform.position + groundCheckPosition, groundCheckScale);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, interactionRadius);
        }
    }

}
