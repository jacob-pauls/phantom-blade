using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character.cs
 * Class definition, physics, health, and essence for characters in TPB
 */

public class TPB_Character : MonoBehaviour
{
    [Header ("Health and Essence")]
    public int maxHealth = 100;
    public int maxEssence = 50;
    public int currentHealth { get; private set; }
    public int currentEssence { get; private set; }

    [Header ("Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float fallMultiplier = 1.5f;
    [SerializeField] private float shortHopMultiplier = 3f;
    [SerializeField] private float crouchResistance = 0.1f;
    [SerializeField] private float wallSlideSpeed = 0.1f;

    [Header ("Collision Detection")]
    [SerializeField] private Collider2D disabledColliderOnCrouch;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private LayerMask phaseShiftWallLayer;

    [Header ("Character Events")]
    public UnityEvent onHealthChange;
    public UnityEvent onEssenceChange;
    public UnityEvent onDeath;
    public UnityEvent onGroundEvent;
    public UnityEvent onCrouchEvent;
    public UnityEvent onWallEvent;
    public UnityEvent offWallEvent;

    protected Animator anim;  
    protected Rigidbody2D rb2D;
    private BoxCollider2D bc2D;
    private CircleCollider2D cc2D;
    private SpriteRenderer spriteRenderer;
    
    private bool isGrounded;
    private bool isFacingRight = true;    
    private bool canStandUp = true;
    private bool isCrouching;
    private bool isTouchingWall;
    private bool isWallSliding;

    protected virtual void Awake() 
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        cc2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        currentEssence = maxEssence;
    }

    protected virtual void Move(float input) 
    {
        rb2D.velocity = new Vector2(speed * input, rb2D.velocity.y);
        anim.SetBool("isRunning", Mathf.Abs(input) > 0);
        
        if (input > 0 && !isFacingRight) {
            FlipCharacter();
        } else if (input < 0 && isFacingRight) {
            FlipCharacter();
        }

        anim.SetFloat("runningSpeed", Mathf.Abs(input));
        anim.SetFloat("yVel", rb2D.velocity.y);
    }

    protected virtual void GroundCheck() 
    {
        // Perform a linecast to the ground in reference to the "Environment" layer mask
        if (Physics2D.Linecast(transform.position, groundCheck.position, environmentLayer)) {
            isGrounded = true;
            onGroundEvent?.Invoke();
        } else {
            isGrounded = false;
        }

        anim.SetBool("isGrounded", isGrounded);
    }

    protected virtual void Jump(bool onPress)
    {
        if (onPress && isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }

        if (rb2D.velocity.y < 0) {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb2D.velocity.y > 0 && !onPress) {
            // Apply more gravty if the jump button is released (short hop)
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (shortHopMultiplier - 1) * Time.deltaTime;
        }
    }

    protected virtual void Crouch(bool onPress)
    {
        if ((onPress && isGrounded) || !canStandUp) {
            rb2D.velocity = new Vector2(rb2D.velocity.x * crouchResistance, 0f);
            isCrouching = true;
            onCrouchEvent?.Invoke();
        } else {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
            isCrouching = false;
        }
        
        // TODO: Revise logic for removing colliders on crouch (per enemy basis?)
        if (cc2D != null) 
            DisableCrouchColliderCheck();
    }
    protected virtual void WallSlide(bool onPressKey1, bool onPressKey2)
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, environmentLayer);
        
        // Check if character is trying to interact with a phase shift wall
        isTouchingWall = isTouchingWall ? isTouchingWall : Physics2D.OverlapCircle(wallCheck.position, 0.1f, phaseShiftWallLayer);

        if (isTouchingWall && !isGrounded) {
            isWallSliding = true;
            onWallEvent?.Invoke();
        } else {
            isWallSliding = false;
            offWallEvent?.Invoke();
        }

        // Modifying the material friction in order to 'latch' onto the wall
        if (isWallSliding && onPressKey1 || onPressKey2) {
            rb2D.sharedMaterial.friction = 0.4f;
        } else if (isWallSliding) {
            rb2D.sharedMaterial.friction = 0.0f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlideSpeed/10, float.MaxValue));
        }        
    }

    private void DisableCrouchColliderCheck() 
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

    private void FlipCharacter() 
    {
        // Flip character direction and apply transform
        isFacingRight = !isFacingRight;
        Vector3 flippedScale = this.transform.localScale;
        flippedScale.x *= -1;
        this.transform.localScale = flippedScale;
    }

    public void ChangeHealthAmount(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        onHealthChange?.Invoke();
        if (currentHealth <= 0)
            onDeath?.Invoke();
    }

    public void ChangeEssenceAmount(int amount)
    {
        currentEssence = Mathf.Clamp(currentEssence + amount, 0, maxEssence);
        onEssenceChange?.Invoke();
    }
}