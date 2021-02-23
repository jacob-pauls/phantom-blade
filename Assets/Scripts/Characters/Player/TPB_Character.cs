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
    public UnityEvent onDeath;

    protected virtual void Awake() 
    {
        currentHealth = maxHealth;
        currentEssence = maxEssence;
    }

    protected virtual void Move(float input) 
    {
        //float movement = moveSpeed * input;
    }

    public void ChangeHealthAmount(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        onHealthChange?.Invoke();
        if (currentHealth <= 0)
            onDeath?.Invoke();
    }

    public void ChangeEssenceAmount(int amount)
    {
        currentEssence = Mathf.Clamp(currentEssence + amount, 0, maxEssence);
        onEssenceChange?.Invoke();
    }
}
