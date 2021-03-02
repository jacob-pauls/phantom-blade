using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy.cs
 * Enemy definition in TPB
 */

public class TPB_Enemy : TPB_Character
{
    [Header ("Enemy")]
    [Space]
    public bool isFlyingUnit = false;
    public float aggroRange;
    public float stoppingDistance;
    public Transform target;

    [Header ("Enemy Stats")]
    [Space]
    [SerializeField] private float damage;
    
    private LayerMask layerEnemyInterest;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        layerEnemyInterest = LayerMask.GetMask("EnemyInterest");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TPB_Player player = collision.gameObject.GetComponent<TPB_Player>();
        if (player)
            Debug.Log("Collided with the player");
    }

    /*
     * Helper Functions
     */
    public bool CanMoveToTarget()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget < aggroRange && distanceToTarget > stoppingDistance) 
            return true;
        return false;
    }

    // void CheckForCollisionsWithPlayer() 
    // {
    //     Collider2D hitCollider = Physics2D.OverlapBox(transform.position, transform.localScale / 2, 0f, layerEnemyInterest);
    //     if (hitCollider != null) {
    //         Debug.Log("Collider: " + hitCollider.name);
    //     }
    // }
}