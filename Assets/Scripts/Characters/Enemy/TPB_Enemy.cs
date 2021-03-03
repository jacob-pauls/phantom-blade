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
    [HideInInspector] public Transform target;

    [Header ("Enemy Stats")]
    [Space]
    [SerializeField] private int attackDamage;
    private float delayBetweenCollisions = 0f;
    [SerializeField] float collisionDelay = 0.3f;
    
    [Header ("Enemy Collision Detection")]
    [SerializeField] float collisionHitBoxHeight;
    [SerializeField] float collisionHitBoxWidth;
    private LayerMask playerLayer;

    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerLayer = LayerMask.GetMask("Player");
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

    public void CheckForCollisionsWithPlayer() 
    {
        // Collide with the player, start a delay so that subsequent collisions are spaced out
        if (delayBetweenCollisions <= 0) {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(collisionHitBoxWidth, collisionHitBoxHeight), playerLayer);
            if (hitColliders.Length > 0) {
                for (int i = 0; i < hitColliders.Length; i++) {
                    TPB_Player player = hitColliders[i].GetComponent<TPB_Player>();
                    // Collide with player, apply damage, reset collision timer
                    if (player) {
                        player.ChangeHealthAmount(-attackDamage);
                        delayBetweenCollisions = collisionDelay;
                        break;
                    }
                }
            }
        }
        if (delayBetweenCollisions > 0)
            delayBetweenCollisions -= Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(collisionHitBoxWidth, collisionHitBoxHeight, 1));
    }
}