using UnityEngine;

/**
 * Jake Pauls
 * TPB_Player_Controller.cs
 * Controls character inputs and input mapping for player movement mechanics
 */

public class TPB_Player_Controller : MonoBehaviour
{
    private TPB_Player player;
    private TPB_Player_Attack_Manager attackManager;
   

    void Awake()
    {
        player = GetComponent<TPB_Player>();
        attackManager = GetComponent<TPB_Player_Attack_Manager>();
    }

    void Update() 
    {
        if (!player.isDead) {
            MovementController();
            JumpController();
            CrouchController();
            WallSlideController();
            MeleeAttackController();
            RangedAttackController();
            WallJumpController();
            PhaseShiftController();
        }
    }

    void MovementController() 
    {
        float input = Input.GetAxisRaw("Horizontal");
        player.Move(input);
        
    }

    void JumpController() 
    {
        float input = Input.GetAxisRaw("Jump");
        bool buttonInput = Input.GetButtonDown("Jump");

        if (buttonInput && player.canDoubleJump) {
            player.DoubleJump(buttonInput);
        } else {
            player.Jump(input);
        }
    }

    void CrouchController() 
    {
        float input = Input.GetAxisRaw("Vertical");
        player.Crouch(input);
    }

    void WallSlideController()
    {
        float input = Input.GetAxisRaw("Horizontal");
        player.WallSlide(input);
    }

    /*
     * Attacks
     */
    void MeleeAttackController()
    {
        bool isAttackKeyPressed = Input.GetButton("Melee");
        attackManager.MeleeAttack(isAttackKeyPressed);
       
    }

    void RangedAttackController()
    {
        bool isAttackKeyPressed = Input.GetButton("Range");
        attackManager.RangedAttack(isAttackKeyPressed);
    }

    /*
     * Abilities
     */
    void PhaseShiftController()
    {
        bool isPhaseShiftKeyPressed = Input.GetButton("Phase Shift");
        player.PhaseShift(isPhaseShiftKeyPressed);   
    }

    void WallJumpController()
    {
        bool isWallJumpKeyPressed = Input.GetButton("Jump");
        player.WallJump(isWallJumpKeyPressed);
    }
}