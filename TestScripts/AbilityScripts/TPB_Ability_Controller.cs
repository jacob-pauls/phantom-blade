using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Jake Pauls
 * TPB_Ability_Controller.cs
 * Handles the unlocking and initialization of abilities
 */

public class TPB_Ability_Controller
{
    /**
     *  Ability names are placed here
     *  This can be used to check whether an ability is unlocked or not
     */
    public enum AbilityTypes  {
        PhaseShift,
        WallJump,
    }

    private List<AbilityTypes> unlockedAbilities;
    
    public TPB_Ability_Controller()
    {
        unlockedAbilities = new List<AbilityTypes>();
        InitializeDefaultAbilities();
    }

    private void InitializeDefaultAbilities()
    {
        unlockedAbilities.Add(AbilityTypes.PhaseShift);
        unlockedAbilities.Add(AbilityTypes.WallJump);
    }

    public void UnlockAbility(AbilityTypes ability)
    {
        unlockedAbilities.Add(ability);
    }

    public bool IsAbilityUnlocked(AbilityTypes ability)
    {
        return unlockedAbilities.Contains(ability);
    }
}
