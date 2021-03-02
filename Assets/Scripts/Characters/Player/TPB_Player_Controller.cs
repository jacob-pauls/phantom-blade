using UnityEngine;

/**
 * Jake Pauls
 * TPB_Player_Controller.cs
 * Controls character inputs and input mapping for player movement mechanics
 */

public class TPB_Player_Controller : MonoBehaviour
{
    private TPB_Player player;

    void Awake()
    {
        player = GetComponent<TPB_Player>();
    }

    void Update() 
    {
        MovementController();
        JumpController();
        CrouchController();
        WallSlideController();
        MeleeAttackController();
        PhaseShiftController();
        WallJumpController();
    }

    void MovementController() 
    {
        float input = Input.GetAxisRaw("Horizontal");
        player.Move(input);
    }

    void JumpController() 
    {
        float input = Input.GetAxisRaw("Jump");
        player.Jump(input);
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
        player.MeleeAttack(isAttackKeyPressed);
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