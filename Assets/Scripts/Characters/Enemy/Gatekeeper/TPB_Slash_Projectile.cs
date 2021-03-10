using UnityEngine;

/**
 * Jake Pauls
 * TPB_Slash_Projectile.cs
 * Slash projectile collision detection
 */

public class TPB_Slash_Projectile : MonoBehaviour
{
    [Header ("Projectile Stats")]
    [Space]
    [SerializeField] private int projectileDamage;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        TPB_Player player = collision.GetComponent<TPB_Player>();
        if (player != null) {
            player.ChangeHealthAmount(-projectileDamage);
        }
    }
}
