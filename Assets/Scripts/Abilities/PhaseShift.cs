using UnityEngine;

/**
 * Jake Pauls
 * PhaseShift.cs
 * Casting logic and implementation for PhaseShift
 */

[CreateAssetMenu (menuName = "Abilities/Phase Shift")]
public class PhaseShift : TPB_Ability
{   
    [Header("Phase Shift Ability Data")]    
    [SerializeField] private float phaseShiftSpeed = 0f;
    [SerializeField] private float phaseShiftDistance = 0f;
    private float phaseShiftTime;

    private GameObject player;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSpriteRenderer;    

    [HideInInspector] public bool isPhaseShifting = false;

    public override void Initialize(GameObject obj) 
    {
        player = obj;
        playerRigidBody = obj.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = obj.GetComponent<SpriteRenderer>();

        // Initialize phase shift time to be decremented as FixedUpdate() calls are made
        phaseShiftTime = phaseShiftDistance;
    }

    public override void Cast() 
    {
        if (phaseShiftTime <= 0) {
            isPhaseShifting = false;
            phaseShiftTime = phaseShiftDistance;
            playerRigidBody.velocity = Vector2.zero;
            
            player.layer = LayerMask.NameToLayer("Default");
            playerSpriteRenderer.color = new Color(1,1,1,1);
        } else {
            isPhaseShifting = true;
            phaseShiftTime -= Time.deltaTime;
                        
            // At 0, player idles
            if (playerRigidBody.velocity.x  > 0) {
                playerRigidBody.AddRelativeForce(Vector2.right * phaseShiftSpeed);
            } else if (playerRigidBody.velocity.x  < 0) {
                playerRigidBody.AddRelativeForce(Vector2.left * phaseShiftSpeed);
            }

            player.layer = LayerMask.NameToLayer("PlayerIgnoreWall");
            playerSpriteRenderer.color = new Color(1,1,1,.5f);
        }
    }
}
