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

    private TPB_Player player;
    private GameObject gameObject;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSpriteRenderer;
    private Animator anim;

    [HideInInspector] public bool isPhaseShifting = false;

    public override void Initialize(GameObject obj) 
    {
        gameObject = obj;
        playerRigidBody = obj.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = obj.GetComponent<SpriteRenderer>();
        player = obj.GetComponent<TPB_Player>();
        anim = obj.GetComponent<Animator>();

        // Initialize phase shift time to be decremented as FixedUpdate() calls are made
        phaseShiftTime = phaseShiftDistance;
    }

    public override void Cast() 
    {
        if (phaseShiftTime <= 0) {
            isPhaseShifting = false;
            phaseShiftTime = phaseShiftDistance;
            playerRigidBody.velocity = Vector2.zero;
            
            gameObject.layer = LayerMask.NameToLayer("Default");
            playerSpriteRenderer.color = new Color(1,1,1,1);
        } else {
            isPhaseShifting = true;
            phaseShiftTime -= Time.deltaTime;
            
            /*
            * If the player is holding 'up', then vertical phase shift
            * At 0, the last faced direction is phase shifted
            */
            if (Input.GetAxisRaw("Vertical") == 1) {
                playerRigidBody.AddRelativeForce(Vector2.up * phaseShiftSpeed/5);
                anim.SetBool("phaseShiftUp",true);
            } else if (playerRigidBody.velocity.x  > 0) {
                playerRigidBody.AddRelativeForce(Vector2.right * phaseShiftSpeed);
            
            } else if (playerRigidBody.velocity.x  < 0) {
                playerRigidBody.AddRelativeForce(Vector2.left * phaseShiftSpeed);
                
            } else if (playerRigidBody.velocity.x == 0) {
                if(player.isFacingRight) {
                    playerRigidBody.AddRelativeForce(Vector2.right * phaseShiftSpeed);
                } else {
                    playerRigidBody.AddRelativeForce(Vector2.left * phaseShiftSpeed);
                }
            }

            gameObject.layer = LayerMask.NameToLayer("PlayerIgnoreWall");
            playerSpriteRenderer.color = new Color(1,1,1,.5f);
        }
    }
}
