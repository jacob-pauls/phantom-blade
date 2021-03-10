using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy_Controller.cs
 * Controls character inputs and input mapping for enemy movement mechanics
 */

public class TPB_Enemy_Controller : MonoBehaviour
{
    private TPB_Enemy enemy;
    private TPB_Melee_Enemy mEnemy;
    private TPB_Gatekeeper gEnemy;
    private bool isMelee = false;

    void Awake()
    {
        if (GetComponent<TPB_Enemy>()) 
            enemy = GetComponent<TPB_Enemy>();

        if (GetComponent<TPB_Melee_Enemy>()) 
            mEnemy = GetComponent<TPB_Melee_Enemy>();

        if (GetComponent<TPB_Gatekeeper>()) 
            gEnemy = GetComponent<TPB_Gatekeeper>();
    }

    void Update()
    {
        MoveToTarget();   
        Patrol();
        enemy.CheckForCollisionsWithPlayer();   
        if (mEnemy) 
            MeleeAttack();
        if (gEnemy)
            GatekeeperCharge();
    }

    void Patrol()
    {  
        if (enemy.patrolWhenNotDetectingPlayer) {
            if (!enemy.CanMoveToTarget()) {
                if (enemy.GroundAheadCheckStatus() || enemy.WallAheadCheckStatus()) {
                    // Enemy needs to flip direction
                    if (enemy.isFacingRight) {
                        enemy.Move(-1);
                    } else {
                        enemy.Move(1);
                    }
                } else {
                    if (enemy.isFacingRight) {
                        enemy.Move(1);
                    } else {
                        enemy.Move(-1);
                    }
                }
            }
        }
    }

    void MoveToTarget()
    {
        if (enemy.CanMoveToTarget()) {
            if (enemy.transform.position.x < enemy.target.position.x) {
                enemy.Move(1);
            } else {
                enemy.Move(-1);
            }        
        } else {
            enemy.Move(0);
        }
    }

    /**
     *  Melee Controls
     */
    void MeleeAttack()
    {
        if (mEnemy.CanAttackPlayer()) {
            mEnemy.MeleeAttack(true);
        }
    }

    /**
     * Gatekeeper Controls
     * TODO: Abstract this logic into seperate 'Gatekeeper Controller' in order to extend logic
     */
    void GatekeeperCharge()
    {
        gEnemy.Charge(Input.GetButtonDown("Jump"));
    }
}
