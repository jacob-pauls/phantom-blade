using System.Collections.Generic;
using UnityEngine;

/**
 * Jake Pauls
 * TPB_Gatekeeper_Controller.cs
 * Controls inputs and input mapping for the Gatekeeper
 */

public class TPB_Gatekeeper_Controller : TPB_Enemy_Controller
{
    private TPB_Gatekeeper gatekeeper;

    private enum GatekeeperPhases {
        Idle,      // During cutscene
        Phase1,    // Basic Attack 
        Phase2,    // Charge + Basic Atack
        Phase3     // Projectile + Charge + Basic Attack
    }

    private enum GatekeeperAbilityOptions {
        Charge,
        Projectile
    }

    [HideInInspector] private GatekeeperPhases phase = GatekeeperPhases.Idle;
    [SerializeField] private float controllerAbilityDelay;
    private float timeBetweenControllerAbilityCasts;
    private double phaseTwoHealthThreshold;
    private double phaseThreeHealthThreshold;
    private List<GatekeeperAbilityOptions> abilityOptions;
    private bool completedPhaseTwo = false;

    protected override void Awake()
    {
        base.Awake();

        if (GetComponent<TPB_Gatekeeper>()) {
            gatekeeper = GetComponent<TPB_Gatekeeper>();
            abilityOptions = new List<GatekeeperAbilityOptions>();
            phaseTwoHealthThreshold = gatekeeper.maxHealth - (gatekeeper.maxHealth * 0.2);
            phaseThreeHealthThreshold = gatekeeper.maxHealth - (gatekeeper.maxHealth * 0.5);
        }
    }

    protected override void Update()
    {
        base.Update();
        CheckGatekeeperAbilityOptions();
        DetermineGatekeeperAbility();

        // TODO: Find way to make this work without persistent Update call
        if (gatekeeper.isCharging)
            gatekeeper.Charge(true);
    }

    /**
     * Gatekeeper Abilities
     */
    void ChargeCast()
    {
        gatekeeper.Charge(true);
    }

    void SlashProjectileCast()
    {
        gatekeeper.SlashProjectile(true);
    }

    /**
     *  Gatekeeper Phases
     */
    public void StartGatekeeperBattle()
    {
        // Takes gatekeeper out of 'Idle' phase
        BeginNextPhase();
    }

    public void EndGatekeeperBattle() 
    {
        Debug.Log("Gatekeeper is dead");
    }

    public void GatekeeperPhaseManager()
    {
        if (gatekeeper.currentHealth <= phaseTwoHealthThreshold && !completedPhaseTwo) {
            BeginNextPhase();
        } else if (gatekeeper.currentHealth <= phaseThreeHealthThreshold) {
            BeginNextPhase();
        }
    }

    private void BeginNextPhase() {
        switch(phase) {
            case GatekeeperPhases.Idle:
                phase = GatekeeperPhases.Phase1;
                Debug.Log("Next phase has begun: " + phase);
                break;
            case GatekeeperPhases.Phase1:
                phase = GatekeeperPhases.Phase2;
                completedPhaseTwo = true;
                Debug.Log("Next phase has begun: " + phase);
                break;
            case GatekeeperPhases.Phase2:
                phase = GatekeeperPhases.Phase3;
                Debug.Log("Next phase has begun: " + phase);
                break;
            default:
                break;
        }
    }

    public void CheckGatekeeperAbilityOptions()
    {
        // If Idling, disable attacking and abilities (during cutscene)
        if (phase == GatekeeperPhases.Idle)  {
            gatekeeper.isAttackingDisabled = true;
            StartGatekeeperBattle();
        } else if (phase == GatekeeperPhases.Phase1) {
            gatekeeper.isAttackingDisabled = false;            
        } else if (phase == GatekeeperPhases.Phase2) {
            AddAbilityOption(GatekeeperAbilityOptions.Charge);
        } else if (phase == GatekeeperPhases.Phase3) {
            AddAbilityOption(GatekeeperAbilityOptions.Projectile);
        }
    }

    public void DetermineGatekeeperAbility()
    {
        int abilityRoll;
        // Roll attack option, and execute attack from available options
        if (abilityOptions.Count >= 1) { 
            if (timeBetweenControllerAbilityCasts <= 0) {
                abilityRoll = Random.Range(0, abilityOptions.Count);
                GatekeeperAbilityOptions abilityThisTurn = abilityOptions[abilityRoll];
                CastAbility(abilityThisTurn);
                timeBetweenControllerAbilityCasts = controllerAbilityDelay;
            } else {
                timeBetweenControllerAbilityCasts -= Time.deltaTime;
            }
        }
    }

    /**
     * Gatekeeper Utility
     */
    private void AddAbilityOption(GatekeeperAbilityOptions abilityOption) 
    {
        if (!abilityOptions.Contains(abilityOption))
            abilityOptions.Add(abilityOption);
    }

    private void CastAbility(GatekeeperAbilityOptions abilityOption)
    {
        Debug.Log("Gatekeeper rolled to cast: " + abilityOption);
        switch(abilityOption) {
            case GatekeeperAbilityOptions.Charge:
                ChargeCast();
                break;
            case GatekeeperAbilityOptions.Projectile:
                SlashProjectileCast();
                break;
        }
    }
}
