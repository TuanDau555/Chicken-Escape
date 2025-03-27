using System;
using System.Collections;
using UnityEngine;

public class PlayerController_WorkScene : MonoBehaviour
{
    #region Component
    [HideInInspector]
    public Rigidbody2D playerRb { get; private set; }

    [Header("Animation")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private bool facingRight = true; 

    [Header("Movement")]
    [SerializeField] private float playerSpeed;
    private Vector2 _moveX;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCut; // decrease a half of jumpForce in inspector 0.5f
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBuffer;
    private float jumpBufferCounter;
    [SerializeField] private float groundCheckRadius;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Moving Platform")]
    public bool isOnPlatform;
    public Rigidbody2D platformRb;
    #endregion

    #region MonoBehaviour func
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        MoveHorizontal();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacles"))
            Die();

        PlayerCheckPoint(other);
    }
    #endregion

    #region Update Cehck Point
    void PlayerCheckPoint(Collider2D other)
    {
        if (other.CompareTag("Extra Checkpoint")) UpdateCheckPoint(transform.position);


        if (other.CompareTag("Main Checkpoint"))
        {
            GameManager_WorkScene.Instance.ChangeScene(gameObject);
        }
    }
    #endregion

    #region Player Input
    void PlayerInput()
    {
        // A & D input
        _moveX = new Vector2(Input.GetAxis("Horizontal"), 0);
        playerAnim.SetBool("isWalk", Math.Abs(_moveX.x) > 0.1f); //Could check if _moveX.x != 0, just a small difference but both is ok

        #region Player Facing Direction
        if (_moveX.x > 0 && !facingRight)
            FacingDirection(); // Facing Right
        else if (_moveX.x < 0 && facingRight)
            FacingDirection(); // Facing Left
        #endregion

        #region Jump

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            playerAnim.SetBool("isJump", false);
        }

        else
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBuffer;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);

            jumpBufferCounter = 0f; // reset jump buffer as soon as player jump
            playerAnim.SetBool("isJump", true); //Jump Animator
            AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSFX); // jump sound
        }

        // If the player release a jump button soon so it will jump shorter than normal
        if (Input.GetButtonUp("Jump") && playerRb.velocity.y > 0f)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * jumpCut);
            playerAnim.SetBool("isJump", true); //Jump Animator
            coyoteTimeCounter = 0f; // reset coyoteTimeCounter
        }
        #endregion

        //Paused Game (ESC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager_WorkScene.Instance.PausedGame();
        }
    }

    #endregion

    #region Movement

    void MoveHorizontal()
    {
        float targetSpeed = _moveX.x * playerSpeed;

        // To make sure that the player will stay on the platform
        if (isOnPlatform)
        {
            playerRb.velocity = new Vector2(targetSpeed + platformRb.velocity.x, playerRb.velocity.y);
        }

        else
        {
            playerRb.velocity = new Vector2(targetSpeed, playerRb.velocity.y);
        }
    }

    bool IsGrounded() 
        => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    void FacingDirection()
    {
        Vector2 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        facingRight = !facingRight;
    }
    #endregion

    #region Respawn
    private Coroutine Die()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.playerDeath);
        return StartCoroutine(Respawn(0.5f));
    }

    private void UpdateCheckPoint(Vector2 newCheckPointPos) => GameManager_WorkScene.Instance.checkPointPos = newCheckPointPos;

    /// <summary>
    /// this code is referenced this video: https://youtu.be/odStG_LfPMQ?si=ekShqQF8ok5RLQkk
    /// playerRb.simulated = fasle (make player cannot move when drop down)
    /// playerRb.velocity = new Vector2 0 0(set player speed to 0 when die)
    /// </summary>
    private IEnumerator Respawn(float respawnDuration)
    {
        // If Player die...
        // ...player cant move anymore...
        playerRb.simulated = false;
        //... so reset player speed
        playerRb.velocity = new Vector2(0, 0);
        // ... unvisualized player when player die...
        transform.localScale = new Vector3(0, 0, 0); 
        yield return new WaitForSeconds(respawnDuration);
        transform.position = GameManager_WorkScene.Instance.checkPointPos;
        transform.localScale = new Vector3(1, 1, 1); 
        playerRb.simulated = true;
        // Set the facing direction to right again, else it will make the sprite switch by player input while respawning
        facingRight = true; 
    }
    #endregion
}
