using System.Threading.Tasks;
using System.Collections.Generic;

public class TPB_Ability_Cooldown
{
    public List<TPB_Ability> abilitiesOnCooldown;

    public TPB_Ability_Cooldown()
    {
        abilitiesOnCooldown = new List<TPB_Ability>();
    }

    public void StartCooldown(TPB_Ability ability)
    {
        abilitiesOnCooldown.Add(ability);
        int secondsDelay = (int) ability.cooldown * 1000;
        Task.Delay(secondsDelay).ContinueWith(task => EndCooldown(ability));
    }

    private void EndCooldown(TPB_Ability ability)
    {
        abilitiesOnCooldown.Remove(ability);
    }

    public bool isAbilityOnCooldown(TPB_Ability ability)
    {
        return abilitiesOnCooldown.Contains(ability);
    }

}
