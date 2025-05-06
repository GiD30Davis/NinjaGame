public void SetColliderCrouching()
{
    if (playerCollider == null) return;

    // Set the collider size and offset for crouching
    playerCollider.size = crouchingColliderSize;
    playerCollider.offset = crouchingColliderOffset;

    Debug.Log("[PlayerStateMachine] Collider set to crouching size and offset.");
}

public void SetColliderStanding()
{
    if (playerCollider == null) return;

    // Restore the collider size and offset for standing
    playerCollider.size = standingColliderSize;
    playerCollider.offset = standingColliderOffset;

    Debug.Log("[PlayerStateMachine] Collider restored to standing size and offset.");
}using UnityEngine;
using System.Collections.Generic;

// Base class for all player states
public abstract class PlayerBaseState
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
}

// The main state machine component
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerBaseState IdleState { get; private set; }
    public PlayerBaseState ShootState { get; private set; }

    private void Awake()
    {
        IdleState = new IdleState(this);
        ShootState = new ShootState(this);
    }

    private void Start()
    {
        SwitchState(IdleState);
    }

    private void Update()
    {
        // Check for mouse click to switch to ShootState
        if (Input.GetMouseButtonDown(0) && !(CurrentState is ShootState))
        {
            SwitchState(ShootState);
        }

        CurrentState.Tick(Time.deltaTime);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}