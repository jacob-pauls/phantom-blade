using UnityEngine;

/**
 * Jake Pauls
 * TPB_Ability.cs
 * Template class for all character abilities in TPB
 */

public abstract class TPB_Ability : ScriptableObject
{
    protected enum Targeting {
        Self,
        CurrentTarget
    };

    [Header("Default Ability Data")]
    [SerializeField] protected string abilityName;
    [SerializeField] protected int damage;
    [SerializeField] public int essenceCost = 10; 
    [SerializeField] public float cooldown = 0f;

    public abstract void Initialize(GameObject obj);
    public abstract void Cast();
}
