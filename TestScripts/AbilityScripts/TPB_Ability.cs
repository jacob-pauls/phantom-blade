using UnityEngine;
using System;

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

    [SerializeField] protected string abilityName;
    [SerializeField] protected int damage;
    [SerializeField] protected int soulCost = 10; 
    [SerializeField] protected float cooldown = 0f;
    [SerializeField] protected Targeting targetingType;
    [SerializeField] public KeyCode buttonAssignment;

    public abstract void Initialize(GameObject obj);
    public abstract void Cast();
}
