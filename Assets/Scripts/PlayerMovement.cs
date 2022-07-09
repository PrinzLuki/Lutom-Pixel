using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _playerRigidbody;
    [SerializeField] private PlayerStats _playerStats;


    [Header("Player Input")]
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool isGrounded;
    public bool CanDoubleJump { get => CanDoubleJump; set => CanDoubleJump = value; }

    [Header("Gizmos")]
    public LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckPosition = new Vector3(-0.003f, -0.4f, 0);
    [SerializeField] private Vector3 groundCheckScale = new Vector3(0.66f, 0.2f, 0);

    [Header("Animations & Effects")]
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;



    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        if (_playerRigidbody == null)
        {
            Debug.LogError("Player is missing a Rigidbody2D component");
        }

        _playerStats = GetComponent<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("Player is missing a PlayerStats component");
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

    [Client]
    private void Update()
    {
        if (!hasAuthority) return;

        MovePlayer();
        IsGrounded();
        Jump();
    }

    /// <summary>
    /// Moves the player with the multiplied amount of "playerSpeed"
    /// </summary>
    private void MovePlayer()
    {
        horizontalInput = InputManager.instance.CurrentPosition();
        _playerRigidbody.velocity = new Vector2(horizontalInput * _playerStats.Speed, _playerRigidbody.velocity.y);

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
                _playerRigidbody.velocity = (new Vector2(_playerRigidbody.velocity.x, _playerStats.JumpPower));
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
                    _playerRigidbody.velocity = (new Vector2(_playerRigidbody.velocity.x, _playerStats.JumpPower));
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


}
