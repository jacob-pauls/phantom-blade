using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Ability_Unlock_Manager.cs
 * Handles the unlocking of abilities off of particular events
 */
public class TPB_Ability_Unlock_Manager : MonoBehaviour
{    
    private TPB_Player player;

    void Awake()
    {
        player = FindObjectOfType<TPB_Player>();
    }

    /**
     *  Non-Default Ability Unlocks
     */
    public void UnlockDoubleJump()
    {
        Debug.Log("Double Jump unlock event invoked");
        player.onDoubleJumpUnlocked?.Invoke();
        player.abilities.UnlockAbility(TPB_Ability_Controller.AbilityTypes.DoubleJump);
    }
}
