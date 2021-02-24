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
}