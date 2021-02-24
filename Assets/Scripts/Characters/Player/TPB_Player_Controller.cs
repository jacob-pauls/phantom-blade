using UnityEngine;

/**
 * Jake Pauls
 * TPB_Player_Controller.cs
 * Controls character inputs and input mapping for player movement mechanics
 */

public class TPB_Player_Controller : TPB_Player
{
    protected override void Update() 
    {
        base.Update();
        base.GroundCheck();
        
        MovementController();
        JumpController();
        CrouchController();
        WallSlideController();
    }

    void MovementController() 
    {
        float input = Input.GetAxisRaw("Horizontal");
        base.Move(input);
    }

    void JumpController() 
    {
        float input = Input.GetAxisRaw("Jump");
        base.Jump(input);
    }

    void CrouchController() 
    {
        float input = Input.GetAxisRaw("Vertical");
        Debug.Log("Crouch Input -> " + input);
        base.Crouch(input);
    }

    void WallSlideController()
    {
        float input = Input.GetAxisRaw("Horizontal");
        base.WallSlide(input);
    }
}