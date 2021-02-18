using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character.cs
 * Class definition for characters in TPB
 */

public class TPB_Character
{
    [Header ("Health and Essence Settings")]
    public int maxHealth = 100;
    public int maxEssence = 50;
    public int currentHealth { get; set; }
    public int currentEssence { get; set; }

    public UnityEvent OnHealthChange;
    public UnityEvent OnEssenceChange;

    public void ChangeHealthAmount(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChange?.Invoke();
    }

    public void ChangeEssenceAmount(int amount)
    {
        currentEssence = Mathf.Clamp(currentEssence + amount, 0, maxEssence);
        OnEssenceChange?.Invoke();
    }
}
