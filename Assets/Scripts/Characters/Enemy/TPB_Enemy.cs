using UnityEngine;

/**
 * Jake Pauls
 * TPB_Enemy.cs
 * Basic, collision-based enemy definition in TPB
 */

public class TPB_Enemy : TPB_Character
{
    [Header ("Enemy")]
    [Space]
    public bool patrolWhenNotDetectingPlayer = false;
    public float aggroRange;
    public float stoppingDistance;
    [HideInInspector] public Transform target;

    [Header ("Enemy Stats")]
    [Space]
    [SerializeField] protected int attackDamage;
    private float delayBetweenCollisions = 0f;
    [SerializeField] float collisionDelay = 0.3f;
    
    [Header ("Enemy Collision Detection")]
    [SerializeField] Transform collisionHitBox;
    [SerializeField] float collisionHitBoxHeight;
    [SerializeField] float collisionHitBoxWidth;
    [SerializeField] Transform groundAheadCheck;
    [SerializeField] float groundAheadCheckLength;
    [SerializeField] Transform wallAheadCheck;
    [SerializeField] float wallAheadCheckLength;
    protected LayerMask playerLayer;
    
    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerLayer = LayerMask.GetMask("Player");
    }

    protected override void Update()
    {
        base.Update();
    }

    /*
     * Helper Functions
     */
    public bool CanMoveToTarget()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Move to the target if in range, and not "close enough"
        if (distanceToTarget < aggroRange && distanceToTarget > stoppingDistance) 
            return true;

        return false;
    }

    public void CheckForCollisionsWithPlayer() 
    {
        // Collide with the player, start a delay so that subsequent collisions are spaced out
        if (delayBetweenCollisions <= 0) {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(collisionHitBox.position, new Vector2(collisionHitBoxWidth, collisionHitBoxHeight), playerLayer);
            if (hitColliders.Length > 0) {
                for (int i = 0; i < hitColliders.Length; i++) {
                    TPB_Player player = hitColliders[i].GetComponent<TPB_Player>();
                    // Collide with player, apply damage, reset collision timer
                    if (player && !player.isPhaseShifting) {
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

    public bool GroundAheadCheckStatus()
    {
        bool shouldEnemyTurnAround = false;
        RaycastHit2D groundCast = Physics2D.Raycast(groundAheadCheck.position, Vector2.down, groundAheadCheckLength, base.environmentLayer);
        
        // If enemy ground ahead check doesn't return a collider, turn around
        if (groundCast.collider == null) {
            shouldEnemyTurnAround = true;
        }
        
        return shouldEnemyTurnAround;
    }

    public bool WallAheadCheckStatus()
    {
        bool shouldEnemyTurnAround = false;
        RaycastHit2D wallCast;
        
        // Determine cast direction
        if (isFacingRight) {
            wallCast = Physics2D.Raycast(wallAheadCheck.position, Vector2.right, wallAheadCheckLength, base.environmentLayer);
        } else {
            wallCast = Physics2D.Raycast(wallAheadCheck.position, Vector2.left, wallAheadCheckLength, base.environmentLayer);
        }

        // If enemy wall check returns a wall collider, turn around
        if (wallCast.collider != null) {
            shouldEnemyTurnAround = true;
        }

        return shouldEnemyTurnAround;
    }

    // When a skeleton dies, leave only their sprite on the ground
    public void TriggerSkeletonDeath()
    {
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour component in components) {
            if (component != GetComponent<SpriteRenderer>())
                component.enabled = false;
        }
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        
    }

    protected virtual void OnDrawGizmosSelected()
    {
        // Enemy Hit Box (Red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(collisionHitBox.position, new Vector3(collisionHitBoxWidth, collisionHitBoxHeight, 1));
        
        // Ground Ahead Raycast (Green)
        Gizmos.color = Color.green;
        Vector2 groundAheadDirection = transform.TransformDirection(Vector2.down) * groundAheadCheckLength;
        Gizmos.DrawRay(groundAheadCheck.position, groundAheadDirection);
        
        // Wall Ahead Raycast (Green)
        Gizmos.color = Color.blue;
        Vector2 wallAheadDirection;
        if (isFacingRight) {
            wallAheadDirection = transform.TransformDirection(Vector2.right) * wallAheadCheckLength;
        } else {
            wallAheadDirection = transform.TransformDirection(Vector2.left) * wallAheadCheckLength;
        }
        Gizmos.DrawRay(wallAheadCheck.position, wallAheadDirection);
    }
}