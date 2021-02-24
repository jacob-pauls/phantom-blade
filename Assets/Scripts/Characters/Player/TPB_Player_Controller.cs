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
        bool input = Input.GetKey(KeyCode.Space);
        base.Jump(input);
    }

    void CrouchController() 
    {
        bool input = Input.GetKey(KeyCode.S);
        base.Crouch(input);
    }

    void WallSlideController()
    {
        bool onPressKey1 = Input.GetKey(KeyCode.A);
        bool onPressKey2 = Input.GetKey(KeyCode.D);
        base.WallSlide(onPressKey1, onPressKey2);
    }
}