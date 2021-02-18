using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character.cs
 * Class definition for characters in TPB
 */

public class TPB_Character
{
    [Header("Health and Essence Settings")]
    public int currentHealth = 100; 
    public int maxHealth = 100;
    public int currentEssence = 50;
    public int maxEssence = 50;

    public UnityEvent OnHealthChange;

    public void ChangeHealthAmount(int amount)
    {
        currentHealth = Mathf.Clamp(amount, 0, maxHealth);
        OnHealthChange?.Invoke();
    }
}
