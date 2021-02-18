using UnityEngine;

/**
 * Jake Pauls
 * TPB_Ability_Behaviour.cs
 * Controls monobehaviour for ability casting
 */

public class TPB_Ability_Behaviour : MonoBehaviour
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
    
    void Awake()
    {        
        abilities = new TPB_Ability_Controller();

        // TODO: Clean up ability initialization logic
        phaseShift.Initialize(player);
        wallJump.Initialize(player);
    }

    void Update()
    {
        PhaseShift();
        WallJump();
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
            } else if (phaseShift.isPhaseShifting) {
                phaseShift.Cast();
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
}
