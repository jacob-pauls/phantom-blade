using UnityEngine;
using Pathfinding;

/**
 * Jake Pauls
 * TPB_Flying_Enemy.cs
 * Flying and collision-based enemy definition in TPB
 */

public class TPB_Flying_Enemy : TPB_Enemy
{
    [Header ("Flying Enemy Pathfinding")]
    [SerializeField] private float nextWaypointDistance = 3f;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 2f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
           seeker.StartPath(base.rb2D.position, base.target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - base.rb2D.position).normalized;
        Vector2 force = direction * base.speed * Time.deltaTime;

        base.rb2D.AddForce(force);

        float distance = Vector2.Distance(base.rb2D.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    public void TriggerFlyingEnemyDeath()
    {   
        Destroy(gameObject);
    }
}
