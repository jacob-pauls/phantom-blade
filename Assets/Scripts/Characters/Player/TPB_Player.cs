using UnityEngine;

/**
 * Jake Pauls
 * TPB_Player.cs
 * Controls monobehaviour for ability casting
 */

public class TPB_Player : TPB_Character
{   
    [Header("Reference Data")]
    [SerializeField] private GameObject player;

    /**
     * Ability References
     * Each ability is containerized by a particular TPB_Ability ScriptableObject
     */
    [Header("Ability Definitions")]
    [SerializeField] private PhaseShift phaseShift;
    [SerializeField] private WallJump wallJump;

    private TPB_Ability_Controller abilities;
    private Animator anim;
    private Rigidbody2D rb2D;

    protected override void Awake()
    {
        base.Awake();        
        abilities = new TPB_Ability_Controller();
        InitializeCurrentAbilities();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        anim = GetComponent<Animator>();
        PhaseShift();
        WallJump();
    }

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

    /**
     *  Ability Definitions/Casting Logic
     */
    void PhaseShift()
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.PhaseShift)) {
            // Check if the character is mid-phaseshift, casting continues if shift is not complete
            if (Input.GetKeyDown(phaseShift.buttonAssignment) && !phaseShift.isPhaseShifting) {
                phaseShift.Cast();
                anim.SetBool("phaseShift", true);

            } else if (phaseShift.isPhaseShifting) {
                phaseShift.Cast();
                anim.SetBool("phaseShift", false);


            }
        }
    }

    void WallJump() 
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.WallJump)) {
            if (Input.GetKeyDown(wallJump.buttonAssignment) && wallJump.isWallSliding) {
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

}
