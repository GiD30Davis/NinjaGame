using UnityEngine;

public class JumpState : PlayerBaseState
{
    private float jumpForce = 10f; // Adjust this value for desired jump height
    private bool isWallJump;

    public JumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        isWallJump = stateMachine.IsTouchingWall(); // Check if this is a wall jump

        if (isWallJump)
        {
            PerformWallJump();
        }
        else if (stateMachine.JumpsRemaining > 0)
        {
            PerformNormalJump();
        }
        else
        {
            // If no jumps are remaining and not wall jumping, transition to FallState
            stateMachine.SwitchState(stateMachine.FallState);
        }
    }

    public override void Tick(float deltaTime)
    {
        // Check if grounded to reset jumps
        if (stateMachine.IsGrounded())
        {
            stateMachine.JumpsRemaining = stateMachine.MaxJumps;
            stateMachine.SwitchState(stateMachine.IdleState);
            return;
        }

        // Allow transitioning to WallClingState if touching a wall
        if (stateMachine.IsTouchingWall() && stateMachine.RB.velocity.y <= 0)
        {
            stateMachine.SwitchState(stateMachine.WallClingState);
            return;
        }

        // Optional: Handle additional jump logic here (e.g., holding jump for higher jumps)
    }

    public override void Exit()
    {
        // Optional: Add logic for exiting the jump state if needed
    }

    private void PerformNormalJump()
    {
        // Apply upward force for a normal jump
        stateMachine.RB.velocity = new Vector2(stateMachine.RB.velocity.x, jumpForce);
        stateMachine.JumpsRemaining--; // Decrease jump count
        Debug.Log($"[JumpState] Performed normal jump. Jumps remaining: {stateMachine.JumpsRemaining}");
    }

    private void PerformWallJump()
    {
        // Determine the direction of the wall jump
        float wallDirection = stateMachine.transform.localScale.x > 0 ? -1 : 1; // Assuming positive X is right
        Vector2 wallJumpForce = new Vector2(wallDirection * stateMachine.WallJumpForce, jumpForce);

        // Apply the wall jump force
        stateMachine.RB.velocity = Vector2.zero; // Reset velocity for consistent jumps
        stateMachine.RB.AddForce(wallJumpForce, ForceMode2D.Impulse);

        // Reset jumps for infinite wall jumps
        stateMachine.JumpsRemaining = stateMachine.MaxJumps;

        Debug.Log($"[JumpState] Performed wall jump. Jumps reset to: {stateMachine.JumpsRemaining}");
    }
}