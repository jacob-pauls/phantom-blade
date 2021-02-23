using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Player_Controller.cs
 * Controls character actions, movements, and performs checks for various collision types
 */

public class TPB_Player_Controller : MonoBehaviour
{
    [Header ("Basic Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float fallMultiplier = 1.5f;
    [SerializeField] private float shortHopMultiplier = 3f;
    [SerializeField] private float crouchResistance = 0.1f;
    [SerializeField] private float wallSlideSpeed = 0.1f;

    [Header ("Collision Detection")]
    [SerializeField] private Collider2D disabledColliderOnCrouch;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private LayerMask phaseShiftWallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;

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

    public UnityEvent OnGroundEvent;
    public UnityEvent OnCrouchEvent;
    public UnityEvent OnWallEvent;
    public UnityEvent OffWallEvent;
    
    private Animator anim;  

    void Awake() 
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        cc2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() 
    {
        MovementCheck();
        GroundCheck();
        Jump();
        Crouch();
        WallSlide();
    }

    void MovementCheck() 
    {
        float movement = Input.GetAxisRaw("Horizontal");

        // Check player input, apply velocity
        //if (movement > 0) {
        //rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        //anim.SetBool("isRunning", true);
        //} else if (movement < 0) {
        //rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
        //anim.SetBool("isRunning", true);
        //} else {
        //rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        //anim.SetBool("isRunning", false);
        //}
        rb2D.velocity = new Vector2(speed * movement, rb2D.velocity.y);
        anim.SetBool("isRunning", Mathf.Abs(movement) > 0);

        if (movement > 0 && !isFacingRight) {
            FlipCharacter();
        } else if (movement < 0 && isFacingRight) {
            FlipCharacter();
        }

        anim.SetFloat("runningSpeed", Mathf.Abs(movement));
        anim.SetFloat("yVel", rb2D.velocity.y);
    }

    void GroundCheck() 
    {
        // Perform a linecast to the ground in reference to the "ground" layer mask
        if (Physics2D.Linecast(transform.position, groundCheck.position, environmentLayer)) {
            isGrounded = true;
            OnGroundEvent?.Invoke();
        } else {
            isGrounded = false;
        }

        anim.SetBool("isGrounded", isGrounded);
    }

    void Jump() 
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            
        }
        if (rb2D.velocity.y < 0) {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb2D.velocity.y > 0 && !Input.GetKey("space")) {
            // Apply more gravty if the jump button is released (short hop)
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (shortHopMultiplier - 1) * Time.deltaTime;
            
        }
    }

    void Crouch() 
    {
        if ((Input.GetKey("s") && isGrounded) || !canStandUp) {
            rb2D.velocity = new Vector2(rb2D.velocity.x * crouchResistance, 0f);
            isCrouching = true;
            OnCrouchEvent?.Invoke();
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

        if (isTouchingWall && !isGrounded) {
            isWallSliding = true;
            OnWallEvent?.Invoke();
        } else {
            isWallSliding = false;
            OffWallEvent?.Invoke();
        }

        // Modifying the material friction in order to 'latch' onto the wall
        if (isWallSliding && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            rb2D.sharedMaterial.friction = 0.4f;
        } else if (isWallSliding) {
            rb2D.sharedMaterial.friction = 0.0f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlideSpeed/10, float.MaxValue));
        }        
    }

    void FlipCharacter() 
    {
        // Flip character direction and apply transform
        isFacingRight = !isFacingRight;
        Vector3 flippedScale = this.transform.localScale;
        flippedScale.x *= -1;
        this.transform.localScale = flippedScale;
    }
}