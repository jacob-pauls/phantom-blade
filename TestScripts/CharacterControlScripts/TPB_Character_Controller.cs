using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character_Controller
 * Controls character actions and movements
 */

public class TPB_Character_Controller : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _crouchResistance = 0.1f;

    [SerializeField] private LayerMask _groundLayerMask;
    
    [SerializeField] private Collider2D _disabledColliderOnCrouch;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ceilingCheck;

    Rigidbody2D rb2D;
    BoxCollider2D bc2D;
    CircleCollider2D cc2D;
    SpriteRenderer spriteRenderer;
    
    public UnityEvent _OnGroundEvent;
    public UnityEvent _OnCrouchEvent;

    bool _isGrounded;
    bool _isCrouching;
    bool _canStandUp = true;
    bool _isFacingRight = true;
    
    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        cc2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize necessary events
        if (_OnGroundEvent == null)        
            _OnGroundEvent = new UnityEvent();
        if (_OnCrouchEvent == null)
            _OnCrouchEvent = new UnityEvent();
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
            rb2D.velocity = new Vector2(_speed, rb2D.velocity.y);
        } else if (movement < 0) {
            rb2D.velocity = new Vector2(-_speed, rb2D.velocity.y);
        } else {
            rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        }

        if (movement > 0 && !_isFacingRight) {
            FlipCharacter();
        } else if (movement < 0 && _isFacingRight) {
            FlipCharacter();
        }
    }

    void GroundCheck() {
        // Perform a linecast to the ground in reference to the "ground" layer mask
        if (Physics2D.Linecast(transform.position, _groundCheck.position, _groundLayerMask)) {
            _isGrounded = true;
            _OnGroundEvent.Invoke();
        } else {
            _isGrounded = false;
        }
        Debug.Log("_isGrounded Status -> " + _isGrounded);
    }

    void JumpCheck() {
        if ((Input.GetKey("space")) && _isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, _jumpForce);
        }
    }

    void CrouchCheck() {
        if ((Input.GetKey("s") && _isGrounded) || !_canStandUp) {
            rb2D.velocity = new Vector2(rb2D.velocity.x * _crouchResistance, 0f);
            _isCrouching = true;
            _OnCrouchEvent.Invoke();
        } else {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y);
            _isCrouching = false;
        }

        DisableCrouchColliderCheck();
    }

    void DisableCrouchColliderCheck() {
        RaycastHit2D ceilingRaycast = Physics2D.Raycast(_ceilingCheck.position, Vector2.up, 0.1f);

        // If we're not crouching, check if we can stand up
        if (!_isCrouching) {
            if (ceilingRaycast.collider != null) 
                _canStandUp = false;
        } else {
            if (ceilingRaycast.collider == null) 
                _canStandUp = true;
        }

        // Disable the top collider if we're crouching under an object
        if (_isCrouching && _disabledColliderOnCrouch != null) {
            _disabledColliderOnCrouch.enabled = false;
        } else {
            _disabledColliderOnCrouch.enabled = true;
        }
    }

    void FlipCharacter() {
        // Flip character direction and apply transform
        _isFacingRight = !_isFacingRight;
        Vector3 flippedScale = this.transform.localScale;
        flippedScale.x *= -1;
        this.transform.localScale = flippedScale;
    }

}
