using UnityEngine;

/**
 * Jake Pauls
 * EnvironmentHazard.cs
 * Behaviour for environmental hazards
 */

public class EnvironmentHazard : MonoBehaviour
{
    [Header ("Hazard Damage")]
    [SerializeField] private bool killOnCollision;
    [SerializeField] private int collisionDamage;

    void Awake() {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
        if (colliders.Length == 0) {
            Debug.LogError("Warning: " + gameObject.name + " has been declared an EnvironmentalHazard without a collider");
        }
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        TPB_Player player = collision.gameObject.GetComponent<TPB_Player>();
        if (player != null) {
            
            // Override collision damage to be player max health
            if (killOnCollision)
                collisionDamage = player.maxHealth;
    
            player.ChangeHealthAmount(-collisionDamage);
        }
    }
}
