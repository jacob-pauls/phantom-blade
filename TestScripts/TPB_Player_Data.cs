using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Jake Pauls
 * TPB_Ability_Controller.cs
 * Controls the player daya, including health/soul totals, and ability progression
 */

public class TPB_Player_Data : MonoBehaviour
{   
    [Header("Health and Soul Settings")]
    public int currentHealth = 50; // WIP
    public int maxHealth = 100;
    public int currentSouls = 50;
    public int maxSouls = 50;

    [Header("Reference Data")]
    [SerializeField] private GameObject player;

    /**
     * Ability References
     * Each ability is containerized by a particular TPB_Ability ScriptableObject
     */
    [Header("Ability Definitions")]
    [SerializeField] private TPB_Ability phaseShift;

    private TPB_Ability_Controller abilities;
    
    void Awake()
    {        
        abilities = new TPB_Ability_Controller();

        // TODO: Clean up ability initialization logic
        phaseShift.Initialize(player);
    }

    void Update()
    {
        CheckIfAbilitiesAreCasted();
    }

    void CheckIfAbilitiesAreCasted() 
    {
        PhaseShift();
    }

    /**
     *  Ability Definitions/Casting Logic
     */
    void PhaseShift()
    {
        if (abilities.IsAbilityUnlocked(TPB_Ability_Controller.AbilityTypes.PhaseShift)) {
            if (Input.GetKeyDown(phaseShift.buttonAssignment)) {
                phaseShift.Cast();
            }
            if (Input.GetKeyUp(phaseShift.buttonAssignment)) {
                phaseShift.Cast();
            }
        }
    }
}
