using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy_Controller.cs
 * Controls character inputs and input mapping for enemy movement mechanics
 */

public class TPB_Enemy_Controller : MonoBehaviour
{
    private TPB_Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<TPB_Enemy>();
    }

    void Update()
    {
        // Patrol();
        MoveToTarget();
        enemy.CheckForCollisionsWithPlayer();
    }

    void Patrol()
    {
        if (!enemy.CanMoveToTarget()) {
            enemy.Move(-1);
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
        }
    }
}
