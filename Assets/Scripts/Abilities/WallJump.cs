﻿using System.Threading.Tasks;
using UnityEngine;

/**
 * Jake Pauls
 * WallJump.cs
 * Casting logic and implementation for WallJump
 */

[CreateAssetMenu (menuName = "Abilities/Wall Jump")]
public class WallJump : TPB_Ability
{
    [Header ("Wall Movement")]
    [SerializeField] private float horizontalWallForce = 0f;
    [SerializeField] private float verticalWallForce = 0f;
    [SerializeField] private int wallJumpDurationInMilliseconds = 0;

    private Rigidbody2D playerRigidBody;

    private bool isWallJumping;

    [HideInInspector] public bool isWallSliding;

    public override void Initialize(GameObject obj) 
    {
        playerRigidBody = obj.GetComponent<Rigidbody2D>();
    }

    public override void Cast() 
    {
        isWallJumping = true;
        Task.Delay(wallJumpDurationInMilliseconds).ContinueWith(task => SetWallJumpToFalse());
    }
    private void SetWallJumpToFalse() 
    {
        isWallJumping = false;
    }

    public void AddForceIfWallJumping() {
        if (isWallJumping) {
            float movement = Input.GetAxisRaw("Horizontal");
            playerRigidBody.AddForce(new Vector2(horizontalWallForce * -movement, verticalWallForce));
        }
    }

    public void CatchOnWallEvent() { isWallSliding = true; }
    public void CatchOffWallEvent() { isWallSliding = false; }
}
