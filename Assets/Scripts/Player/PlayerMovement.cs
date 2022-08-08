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

    [Header("Gizmos")]
    public LayerMask groundLayer;
    [SerializeField] private Vector3 groundCheckPosition = new Vector3(-0.003f, -0.4f, 0);
    [SerializeField] private Vector3 groundCheckScale = new Vector3(0.66f, 0.2f, 0);

    [Header("Animations & Effects")]
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool flipX;
    [SerializeField] private bool isMoving;

    public bool CanDoubleJump { get => CanDoubleJump; set => CanDoubleJump = value; }
    public bool FlipX { get => flipX; set => flipX = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    [Client]
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

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_animator == null)
        {
            Debug.LogError("Player is missing a SpriteRenderer component");
        }
    }

    [Client]
    private void Update()
    {
        if (!hasAuthority) return;
        if (InputManager.instance == null) { Debug.LogWarning("Input Instance missing!"); return; }
        horizontalInput = InputManager.instance.CurrentPosition();
        MovePlayer();
        IsGrounded();
        Jump();
    }

    #region Movement


    /// <summary>
    /// Moves the player with the multiplied amount of "playerSpeed"
    /// </summary>
    private void MovePlayer()
    {
        _playerRigidbody.velocity = new Vector2(horizontalInput * _playerStats.Speed, _playerRigidbody.velocity.y);

        #region Flip By Mouse
        //if (mousePos.x > transform.position.x)
        //{
        //    flipX = false;
        //}

        //if (mousePos.x < transform.position.x)
        //{
        //    flipX = true;

        //}
        #endregion

        #region Flip By Input
        if (horizontalInput > 0)
        {
            FlipX = false;
        }
        if (horizontalInput < 0)
        {
            FlipX = true;
        }
        #endregion

        //Execute on Client
        CmdFlipXOnClient();
        //Execute on Server
        CmdFlipXOnServer(FlipX, this.gameObject);

        SetRunAnimationOnClient();
    }

    #endregion

    #region Flip Character
    /// <summary>
    /// Executes the function on the client
    /// </summary>
    void CmdFlipXOnClient()
    {
        _spriteRenderer.flipX = FlipX;
    }

    /// <summary>
    /// Sends this function to the server from THIS client
    /// </summary>
    /// <param name="flipValue"></param>
    /// <param name="trg"></param>
    [Command]
    void CmdFlipXOnServer(bool flipValue, GameObject trg)
    {
        RpcFlipXOnServer(flipValue, trg);
    }

    /// <summary>
    /// The Server executes this function to ALL clients
    /// </summary>
    /// <param name="flipValue"></param>
    /// <param name="trg"></param>
    [ClientRpc]
    void RpcFlipXOnServer(bool flipValue, GameObject trg)
    {
        trg.GetComponent<SpriteRenderer>().flipX = flipValue;
    }
    #endregion

    #region Jump
    /// <summary>
    /// Jump function for the player - Adds velocity to the y-axis of player
    /// </summary>
    private void Jump()
    {
        if (InputManager.instance.IsJumping() && isLocalPlayer)
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
    #endregion

    #region Check If Grounded
    /// <summary>
    /// Returns boolean true - if player is grounded (on the ground)
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        var isTouchingGround = Physics2D.OverlapBox(transform.position + groundCheckPosition, groundCheckScale, 0, groundLayer);

        if (isTouchingGround /*&& (_playerRigidbody.velocity.y == 0 || _playerRigidbody.velocity.y <= 0)*/)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }
    #endregion

    #region Animation
    /// <summary>
    /// Checks if player is moving, if yes - the animation will be set and activated
    /// </summary>
    private void SetRunAnimationOnClient()
    {
        if (horizontalInput != 0)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }

        _animator.SetBool("isMoving", IsMoving);
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + groundCheckPosition, groundCheckScale);
    }

    #endregion

}
