using UnityEngine;

public class CrouchState : PlayerBaseState
{
    private float enterTime;
    private float crouchMoveSpeed;

    public CrouchState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        // Calculate actual crouch speed based on multipliers
        crouchMoveSpeed = stateMachine.MoveSpeed * stateMachine.CrouchSpeedMultiplier;
    }

    public override void Enter()
    {
        enterTime = Time.time;
        stateMachine.SetColliderCrouching();
        // Play crouch animation
        if (stateMachine.Animator != null)
            stateMachine.Animator.Play("Crouch"); // Replace with actual animation name
        Debug.Log($"[CrouchState] Entering Crouch State at {enterTime:F2}s");
    }

    public override void Tick(float deltaTime)
    {
        // --- NEW: Check for loss of ground or wall contact ---
        if (!stateMachine.IsGrounded())
        {
            if (stateMachine.IsTouchingWall() && stateMachine.RB.linearVelocity.y <= 0)
            {
                stateMachine.SwitchState(stateMachine.WallClingState);
            }
            else
            {
                stateMachine.SwitchState(stateMachine.FallState);
            }
            return;
        }

        // Check for Shoot input first
        if (stateMachine.InputReader.IsShootPressed())
        {
            stateMachine.SwitchState(stateMachine.ShootState);
            return;
        }

        // --- Check for Exit Conditions ---

        // 1. Crouch key released?
        if (!stateMachine.InputReader.IsCrouchHeld())
        {
            // Determine next state based on input
            Vector2 moveInputCheck = stateMachine.InputReader.GetMovementInput();
            if (moveInputCheck == Vector2.zero)
            {
                stateMachine.SwitchState(stateMachine.IdleState);
            }
            else
            {
                if (stateMachine.InputReader.IsRunPressed())
                    stateMachine.SwitchState(stateMachine.RunState);
                else
                    stateMachine.SwitchState(stateMachine.WalkState);
            }
            return;
        }

        // --- Apply Crouch Movement ---
        Vector2 moveInput = stateMachine.InputReader.GetMovementInput();
        stateMachine.RB.linearVelocity = moveInput * crouchMoveSpeed;

        // Update animation blend tree if needed
        if (stateMachine.Animator != null && moveInput != Vector2.zero)
        {
            stateMachine.Animator.SetFloat("Horizontal", moveInput.x);
        }
    }

    public override void Exit()
    {
        // Restore collider to standing position unconditionally
        stateMachine.SetColliderStanding();

        // Stop crouch animation if needed
        Debug.Log($"[CrouchState] Exiting Crouch State after {Time.time - enterTime:F2}s");
    }
}
