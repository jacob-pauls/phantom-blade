using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character_Controller
 * Controls character actions, movements, and performs checks for various collision types
 */

public class TPB_Character_Controller : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float crouchResistance = 0.1f;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private Collider2D disabledColliderOnCrouch;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;

    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    CircleCollider2D cc2D;
    SpriteRenderer spriteRenderer;
    
    public UnityEvent ON_GROUND_EVENT;
    public UnityEvent ON_CROUCH_EVENT;

    bool isGrounded;
    bool isCrouching;
    bool canStandUp = true;
    bool isFacingRight = true;
    
    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        cc2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize necessary events
        if (ON_GROUND_EVENT == null)        
            ON_GROUND_EVENT = new UnityEvent();
        if (ON_CROUCH_EVENT == null)
            ON_CROUCH_EVENT = new UnityEvent();
    }

    void FixedUpdate() {
        MovementCheck();
        GroundCheck();
        JumpCheck();
        CrouchCheck();
    }

    void MovementCheck() {
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

    void GroundCheck() {
        // Perform a linecast to the ground in reference to the "ground" layer mask
        if (Physics2D.Linecast(transform.position, groundCheck.position, environmentLayer)) {
            isGrounded = true;
            ON_GROUND_EVENT.Invoke();
        } else {
            isGrounded = false;
        }
    }

    void JumpCheck() {
        if ((Input.GetKey("space")) && isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
    }

    void CrouchCheck() {
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

    void DisableCrouchColliderCheck() {
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

    void FlipCharacter() {
        // Flip character direction and apply transform
        isFacingRight = !isFacingRight;
        Vector3 flippedScale = this.transform.localScale;
        flippedScale.x *= -1;
        this.transform.localScale = flippedScale;
    }

}
