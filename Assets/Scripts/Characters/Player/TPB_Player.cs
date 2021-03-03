using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Player.cs
 * Definition for player specifics in TPB
 */

public class TPB_Player : TPB_Character
{   
    [Header ("Player Specific Movement")]
    [SerializeField] private float wallSlideSpeed = 0.1f;
    [SerializeField] private float crouchResistance = 0.1f;

    [Header ("Player Specific Collision Detection")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask phaseShiftWallLayer;
    [SerializeField] private Collider2D disabledColliderOnCrouch;

    [Header ("Attack Data")]
    [SerializeField] int attackDamage;
    private float delayBetweenAttacks = 0f;
    [SerializeField] float attackDelay = 0.3f;
    [SerializeField] float attackHitBoxHeight;
    [SerializeField] float attackHitBoxWidth;
    [SerializeField] Transform attackCollider;
    [SerializeField] LayerMask enemyLayer;
    [HideInInspector] public int comboCounter = 0;

    /**
     * Ability References
     * Each ability is containerized by a particular TPB_Ability ScriptableObject
     */
    [Header("Ability Data")]
    [SerializeField] private GameObject player;
    [SerializeField] private PhaseShift phaseShift;
    [SerializeField] private WallJump wallJump;

    [Header ("Player Specific Events")]
    public UnityEvent onWallEvent;
    public UnityEvent offWallEvent;
    public UnityEvent onCrouchEvent;
    public UnityEvent onEnemyHit;

    private TPB_Ability_Controller abilities;
    private TPB_Ability_Cooldown abilityCooldownManager;
    private CircleCollider2D cc2D;

    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isCrouching;
    private bool canStandUp = true;
    [HideInInspector] public bool isPhaseShifting { get; private set; }

    protected override void Awake()
    {
        base.Awake();        
        cc2D = GetComponent<CircleCollider2D>();

        abilities = new TPB_Ability_Controller();
        abilityCooldownManager = new TPB_Ability_Cooldown();
        InitializeCurrentAbilities();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PickupItem pickup = collision.gameObject.GetComponent<PickupItem>();
        if (pickup != null)
        {
            Item item = pickup.Collect();

            // Health Orb
            //if (item.Id == "consume_the_living")
            if (item.Type == Item.ItemType.InstantConsumable)
            {
                // Finding Health
                Item.Attribute attribute = item.GetAttribute("Health", false);
                int health = attribute != null ? attribute.GetValueAsInt : 0;
                ChangeHealthAmount(health);

                // Finding Essence
                attribute = item.GetAttribute("Essence", false);
                int essence = attribute != null ? attribute.GetValueAsInt : 0;
                ChangeHealthAmount(essence);
            }
            else
            {
                GameManager.Load().Inventory.Add(item);
                //inventory.Add(item);
            }

            onItemCollected?.Invoke();

            // Destroy the object
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Chest chest = collision.GetComponent<Chest>();
        if (chest != null)
        {
            Item key = GameManager.Load().Inventory.Get("new_opportunities");
            bool isKeyAvailable = false;
            if (key != null)
            {
                if (key.CurrentStackAmount > 0)
                {
                    key.ChangeStackAmount(-1);
                    isKeyAvailable = true;
                }
            }

            onItemUsed?.Invoke();

            chest.Open(isKeyAvailable);
        }
    }

    /**
     * Player Specific Movement Logic
     */
    public void WallSlide(float input)
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, environmentLayer);
        
        // Check if character is trying to interact with a phase shift wall
        isTouchingWall = isTouchingWall ? isTouchingWall : Physics2D.OverlapCircle(wallCheck.position, 0.1f, phaseShiftWallLayer);

        if (isTouchingWall && !base.isGrounded) {
            isWallSliding = true;
            onWallEvent?.Invoke();
        } else {
            isWallSliding = false;
            offWallEvent?.Invoke();
        }

        // Modifying the material friction in order to 'latch' onto the wall
        if (isWallSliding && ((input == 1) || (input == -1))) {
            rb2D.sharedMaterial.friction = 0.4f;
        } else if (isWallSliding) {
            rb2D.sharedMaterial.friction = 0.0f;
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -wallSlideSpeed/10, float.MaxValue));
        }        
    }

    public void Crouch(float input)
    {
        if (((input < -0.75 && input >= -1) && isGrounded) || !canStandUp) {
            rb2D.velocity = new Vector2(rb2D.velocity.x * crouchResistance, rb2D.velocity.y);
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

    /**
     * Attacking Definitions/Logic
     */
    public void MeleeAttack(bool isAttackKeyPressed)
    {
        if(delayBetweenAttacks <= 0) {
            if (isAttackKeyPressed) {
                Collider2D[] enemyColliders = Physics2D.OverlapBoxAll(attackCollider.position, new Vector2(attackHitBoxWidth, attackHitBoxHeight), enemyLayer);
                for (int i = 0; i < enemyColliders.Length; i++) {
                    TPB_Enemy enemy = enemyColliders[i].GetComponent<TPB_Enemy>();
                    if (enemy) {
                        // TODO: The logic is here to hit ONE enemy, modify this to multiple?
                        enemy.ChangeHealthAmount(-attackDamage);
                        onEnemyHit?.Invoke();
                        // gameObject.SetActive(false);
                        break;
                    }
                }
                delayBetweenAttacks = attackDelay;
            }
        } else {
            delayBetweenAttacks -= Time.deltaTime;
        }
    }

    public void RangedAttack(bool isAttack)
    {
        // TODO: Abstract Imput, Implement Ranged Attack Logic
        if (Input.GetButton("Range"))
            Debug.Log("Ranged Attack!");
    }

    /**
     *  Ability Definitions/Casting Logic
     */
    void InitializeCurrentAbilities()
    {
        // Only initialize the abilities that are unlocked
        foreach(TPB_Ability_Controller.AbilityTypes ability in abilities.unlockedAbilities) {
            if (ability == TPB_Ability_Controller.AbilityTypes.PhaseShift) {
                phaseShift.Initialize(player);
            } else if (ability == TPB_Ability_Controller.AbilityTypes.WallJump) {
                wallJump.Initialize(player);
            }
        }
    }

    public void PhaseShift(bool isPhaseShiftKeyPressed)
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.PhaseShift)) {
            isPhaseShifting = phaseShift.isPhaseShifting;
            // Check if the character is mid-phaseshift, casting continues if shift is not complete
            if (isPhaseShiftKeyPressed && !abilityCooldownManager.isAbilityOnCooldown(phaseShift) && !isPhaseShifting) {
                phaseShift.Cast();
                
                // Start the phase shift cooldown, lock player input
                abilityCooldownManager.StartCooldown(phaseShift);

                base.anim.SetBool("phaseShift", true);
            } else if (phaseShift.isPhaseShifting) {
                phaseShift.Cast();
            } else {
                base.anim.SetBool("phaseShift", false);
                base.anim.SetBool("phaseShiftUp", false);
            }
        }
    }

    public void WallJump(bool isWallJumpKeyPressed) 
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.WallJump)) {
            if (isWallJumpKeyPressed && wallJump.isWallSliding) {
                wallJump.Cast();
            }
            wallJump.AddForceIfWallJumping();
        }
    }

    [ContextMenu ("Test Damage")]
    void TestDamage() 
    {
        ChangeHealthAmount(-10);
    }

    [ContextMenu ("Test Heal")]
    void TestHeal()
    {
        ChangeHealthAmount(10);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackCollider.position, new Vector3(attackHitBoxWidth, attackHitBoxHeight, 1));
    }

}
