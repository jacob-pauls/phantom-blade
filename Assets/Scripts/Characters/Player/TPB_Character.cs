using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character.cs
 * Class definition, physics, health, and essence for characters in TPB
 */

public class TPB_Character : MonoBehaviour
{
    [Header ("Character Health and Essence")]
    public int maxHealth = 100;
    public int maxEssence = 50;
    public int currentHealth { get; private set; }
    public int currentEssence { get; private set; }

    [Header ("General Character Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float fallMultiplier = 1.5f;
    [SerializeField] private float shortHopMultiplier = 3f;

    [Header ("General Character Collision Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] protected LayerMask environmentLayer;

    [Header ("General Character Events")]
    public UnityEvent onHealthChange;
    public UnityEvent onEssenceChange;
    public UnityEvent onDeath;
    public UnityEvent onGroundEvent;
    public UnityEvent onItemCollected;
    public UnityEvent onItemUsed;

    [Header("Inventory")]
    [SerializeField] public Inventory inventory;

    protected Animator anim;  
    protected Rigidbody2D rb2D;
    protected BoxCollider2D bc2D;
    private SpriteRenderer spriteRenderer;
    
    protected bool isGrounded;
    [HideInInspector] public bool isFacingRight = true;    

    protected virtual void Awake() 
    {
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        bc2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        currentEssence = maxEssence;
    }

    void Update()
    {
        GroundCheck();
    }

    public void Move(float input) 
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

    public void Jump(float input)
    {
        if ((input > 0) && isGrounded) {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }

        if (rb2D.velocity.y < 0) {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb2D.velocity.y > 0 && (input == 0)) {
            // Apply more gravty if the jump button is released (short hop)
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (shortHopMultiplier - 1) * Time.deltaTime;
        }
    }

    private void GroundCheck() 
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

    public void FlipCharacter() 
    {
        // Flip character direction and apply transform
        isFacingRight = !isFacingRight;
        Vector3 flippedScale = this.transform.localScale;
        flippedScale.x *= -1;
        this.transform.localScale = flippedScale;
    }

    public void ChangeHealthAmount(int amount)
    {
        if (amount == 0) { return; }
        if (amount <= 0) { anim.SetTrigger("isHit"); }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        onHealthChange?.Invoke();
        if (currentHealth <= 0)
            onDeath?.Invoke();
    }

    public void ChangeEssenceAmount(int amount)
    {
        if (amount == 0) { return; }

        currentEssence = Mathf.Clamp(currentEssence + amount, 0, maxEssence);
        onEssenceChange?.Invoke();
    }

    public void EndHitAnimation() 
    {
        Debug.Log("Ending Hit Animation...");
        anim.ResetTrigger("isHit");
    }
}