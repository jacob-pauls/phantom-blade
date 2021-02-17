using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character_Controller
 * Controls character actions, movements, and performs checks for various collision types
 */

public class TPB_Character_Controller : MonoBehaviour
{
    [Header ("Basic Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float crouchResistance = 0.1f;

    [Header ("Wall Movement")]
    [SerializeField] private float wallSlideSpeed = 0.1f;
    [SerializeField] private float horizontalWallForce = 0f;
    [SerializeField] private float verticalWallForce = 0f;
    [SerializeField] private float wallJumpDuration = 0f;

    [Header ("Collision Detection")]
    [SerializeField] private Collider2D disabledColliderOnCrouch;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private LayerMask phaseShiftWallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform wallCheck;

    private Rigidbody2D rb2D;
    private BoxCollider2D bc2D;
    private CircleCollider2D cc2D;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool isCrouching;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool canStandUp = true;
    private bool isFacingRight = true;    

    public UnityEvent ON_GROUND_EVENT;
    public UnityEvent ON_CROUCH_EVENT;
    public UnityEvent ON_WALL_EVENT;
    public UnityEvent OFF_WALL_EVENT;
    
    void Awake() 
    {
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        cc2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeCharacterControllerEvents();
    }

    void FixedUpdate() 
    {
        MovementCheck();
        GroundCheck();
        Jump();
        Crouch();
        WallSlide();
        // WallJump();
    }

    void MovementCheck() 
    {
        float movement = Input.GetAxisRaw("Horizontal");

        // Check player input, apply velocity
        if (movement > 0) {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        } else if (movement < 0) {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
        } else {
            rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        }

        if (movement > 0 && !isFacingRight) {
            FlipCharacter();
        } else if (movement < 0 && isFacingRight) {
            FlipCharacter();
        }
    }

    void GroundCheck() 
    {
        // Perform a linecast to the ground in reference to the "ground" layer mask
        if (Physics2D.Linecast(transform.position, groundCheck.position, environmentLayer)) {
            isGrounded = true;
            ON_GROUND_EVENT.Invoke();
        } else {
            isGrounded = false;
        }
    }

    void Jump() 
    {
        if ((Input.GetKey("space")) && isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
    }

    void Crouch() 
    {
        if ((Input.GetKey("s") && isGrounded) || !canStandUp) {
            rb2D.velocity = new Vector2(rb2D.velocity.x * crouchResistance, 0f);
            isCrouching = true;
            ON_CROUCH_EVENT.Invoke();
        } else {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
            isCrouching = false;
        }

        DisableCrouchColliderCheck();
    }

    void DisableCrouchColliderCheck() 
    {
        RaycastHit2D ceilingRaycast = Physics2D.Raycast(ceilingCheck.position, Vector2.up, 0.1f);

        // If we're not crouching, check if we can stand up
        if (!isCrouching) {
            if (ceilingRaycast.collider != null) 
                canStandUp = false;
        } else {
            if (ceilingRaycast.collider == null) 
                canStandUp = true;
        }

        // Disable the top collider if we're crouching under an object
        if (isCrouching && disabledColliderOnCrouch != null) {
            disabledColliderOnCrouch.enabled = false;
        } else {
            disabledColliderOnCrouch.enabled = true;
        }
    }

    void WallSlide()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, environmentLayer);
        
        // Check if character is trying to interact with a phase shift wall
        isTouchingWall = isTouchingWall ? isTouchingWall : Physics2D.OverlapCircle(wallCheck.position, 0.1f, phaseShiftWallLayer);

        if (isTouchingWall && !isGrounded && movement != 0) {
            isWallSliding = true;
            ON_WALL_EVENT.Invoke();
        } else {
            isWallSliding = false;
            OFF_WALL_EVENT.Invoke();
        }

        if(isWallSliding) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlideSpeed/10, float.MaxValue));
        }        
    }

    void WallJump()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown("space") && isWallSliding) {
            isWallJumping = true;
            StartCoroutine("WallJumpCoroutine");
        }

        if (isWallJumping) {
                        Debug.Log("Horizontal Value -> " + (horizontalWallForce * -movement));
            rb2D.velocity = new Vector2(horizontalWallForce * -movement, verticalWallForce);
        }
    }

    IEnumerator WallJumpCoroutine() 
    {
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }

    void FlipCharacter() 
    {
        // Flip character direction and apply transform
        isFacingRight = !isFacingRight;
        Vector3 flippedScale = this.transform.localScale;
        flippedScale.x *= -1;
        this.transform.localScale = flippedScale;
    }

    void InitializeCharacterControllerEvents() 
    {
        if (ON_GROUND_EVENT == null)        
            ON_GROUND_EVENT = new UnityEvent();
        if (ON_CROUCH_EVENT == null)
            ON_CROUCH_EVENT = new UnityEvent();
        if (ON_WALL_EVENT == null)
            ON_WALL_EVENT = new UnityEvent();
        if (OFF_WALL_EVENT == null)
            OFF_WALL_EVENT = new UnityEvent();
    }
}