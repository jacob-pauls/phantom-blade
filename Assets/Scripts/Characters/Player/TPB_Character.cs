using UnityEngine;
using UnityEngine.Events;

/**
 * Jake Pauls
 * TPB_Character.cs
 * Class definition for characters in TPB
 */

public class TPB_Character : MonoBehaviour
{
    [Header ("Health and Essence Settings")]
    public int maxHealth = 100;
    public int maxEssence = 50;
    public int currentHealth { get; private set; }
    public int currentEssence { get; private set; }

    public UnityEvent onHealthChange;
    public UnityEvent onEssenceChange;

    protected virtual void Awake() 
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealthAmount(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        onHealthChange?.Invoke();
    }

    public void ChangeEssenceAmount(int amount)
    {
        currentEssence = Mathf.Clamp(currentEssence + amount, 0, maxEssence);
        onEssenceChange?.Invoke();
    }
}
