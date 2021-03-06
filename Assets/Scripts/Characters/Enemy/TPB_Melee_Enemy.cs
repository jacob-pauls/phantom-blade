using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy.cs
 * Melee and collision-based enemy definition in TPB
 */

public class TPB_Melee_Enemy : TPB_Enemy
{    
    [Header ("Attack Detection")]
    [Space]
    [SerializeField] float attackHitBoxHeight;
    [SerializeField] float attackHitBoxWidth;
    [SerializeField] Transform attackCollider;

    [Header ("Attack Timing")]
    [Space]
    private float delayBetweenAttacks = 0f;
    [SerializeField] private float attackDelay = 0.3f;

    public bool CanAttackPlayer() 
    {
        Collider2D[] playerColliders = Physics2D.OverlapBoxAll(attackCollider.position, new Vector2(attackHitBoxWidth, attackHitBoxHeight), base.playerLayer);
        for (int i = 0; i < playerColliders.Length; i++) {
            TPB_Player player = playerColliders[i].GetComponent<TPB_Player>();
            if (player) {
                return true;
            }
        }    
        return false;
    }

    public void MeleeAttack(bool canAttack)
    {
        if (delayBetweenAttacks <= 0) {
            if (canAttack) {
                anim.SetBool("firstAttack", true);
                Collider2D[] playerColliders = Physics2D.OverlapBoxAll(attackCollider.position, new Vector2(attackHitBoxWidth, attackHitBoxHeight), base.playerLayer);
                for (int i = 0; i < playerColliders.Length; i++) {
                    TPB_Player player = playerColliders[i].GetComponent<TPB_Player>();
                    if (player && !player.isPhaseShifting) {
                        player.ChangeHealthAmount(-base.attackDamage);
                        break;
                    }
                }
                delayBetweenAttacks = attackDelay;
            }
        } else {
            delayBetweenAttacks -= Time.deltaTime;
        }
    }

    public void ResetMeleeAttack() 
    {
        anim.SetBool("firstAttack", false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackCollider.position, new Vector3(attackHitBoxWidth, attackHitBoxHeight, 1));
    }
}
