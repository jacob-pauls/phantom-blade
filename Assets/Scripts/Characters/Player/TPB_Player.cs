using UnityEngine;

/**
 * Jake Pauls
 * TPB_Player.cs
 * Definition for player specifics in TPB
 */

public class TPB_Player : TPB_Character
{   
    /**
     * Ability References
     * Each ability is containerized by a particular TPB_Ability ScriptableObject
     */
    [Header("Ability Data")]
    [SerializeField] private GameObject player;
    [SerializeField] private PhaseShift phaseShift;
    [SerializeField] private WallJump wallJump;

    private TPB_Ability_Controller abilities;
    
    protected override void Awake()
    {
        base.Awake();        
        base.rb2D = GetComponent<Rigidbody2D>();
        abilities = new TPB_Ability_Controller();
        InitializeCurrentAbilities();
    }

    protected virtual void Update()
    {
        base.anim = GetComponent<Animator>();

        MeleeAttack();
        RangedAttack();

        PhaseShift();
        WallJump();
    }

    /**
     * Attacking Definitions/Logic
     */
     void MeleeAttack()
     {
        // TODO: Implement Melee Attack Logic
        if (Input.GetButton("Melee"))
            Debug.Log("Melee Attack!");
     }

     void RangedAttack()
     {
        // TODO: Implement Ranged Attack Logic
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

    void PhaseShift()
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.PhaseShift)) {
            // Check if the character is mid-phaseshift, casting continues if shift is not complete
            if (Input.GetButton("Phase Shift") && !phaseShift.isPhaseShifting) {
                phaseShift.Cast();
                base.anim.SetBool("phaseShift", true);

            } else if (phaseShift.isPhaseShifting) {
                phaseShift.Cast();
                base.anim.SetBool("phaseShift", false);
            }
        }
    }

    void WallJump() 
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.WallJump)) {
            if (Input.GetButton("Jump") && wallJump.isWallSliding) {
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
