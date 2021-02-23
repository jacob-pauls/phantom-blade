using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
        MovementCheck();
        Jump();
        Crouch();
        WallSlide();
    }

    void MovementCheck() 
    {
        float input = Input.GetAxisRaw("Horizontal");
        base.Move(input);
    }

    void Jump() 
    {
        bool input = Input.GetKeyDown(KeyCode.Space);
        Debug.Log("Sending Jump Condition -> " + input);
        base.Jump(input);
    }

    void Crouch() 
    {
        bool input = Input.GetKey(KeyCode.S);
        base.Crouch(input);
    }

    void WallSlide()
    {
        bool onPressKey1 = Input.GetKey(KeyCode.A);
        bool onPressKey2 = Input.GetKey(KeyCode.D);
        base.WallSlide(onPressKey1, onPressKey2);
    }
}