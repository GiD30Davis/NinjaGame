using UnityEngine;

public interface ICrouchState
{
    void Enter();
    void Exit();
    void Tick(global::System.Single deltaTime);
// Removed duplicate Exit method
}

public class CrouchState : PlayerBaseState, ICrouchState
{
    public CrouchState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    private Transform playerTransform; // Reference to the player's transform
    public float attractionForce = 50f; // Strength of the attraction
    public float magnetRange = 10f; // Range at which objects are attracted
    public LayerMask targetLayer = ~0; // Default to all layers being included
    public float attractionTimer;
    

    public override void Exit()
    {
        // Logic to handle exiting the crouch state
        Debug.Log("[CrouchState] Exiting Crouch State");
        // Optionally, reset or stop any crouch-specific behaviors here
    }

    /// <summary>
    public override void Enter()
    {
        // Simulate being grounded when entering crouch state
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Debug.Log($"[CrouchState] Player Object: {playerObject}");
        if (playerObject != null)
        {
            this.playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("[CrouchState] No GameObject with the 'Player' tag found.");
        }

        // Play crouch animation
        if (playerTransform != null)
        {
            // Assuming you have an Animator component on the player
            Animator animator = playerTransform.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Crouch");
            }
        }

        Debug.Log("[CrouchState] Entering Crouch State");
    }
    public override void Tick(float deltaTime)
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("[CrouchState] playerTransform is null. Skipping AttractObjects.");
            return;
        }

        
        AttractObjects();

        attractionTimer -= deltaTime;

        // Check for exit condition (e.g., pressing a key to leave crouch state)
        if (Input.GetKeyDown(KeyCode.E))
        {
            stateMachine.SwitchState(stateMachine.IdleState); // Transition back to the previous state
        }
    }

    private void AttractObjects()
    {
        // Find all colliders within the range of the magnet
        Collider2D[] colliders = Physics2D.OverlapCircleAll(stateMachine.transform.position, magnetRange, targetLayer);
    
        if (colliders.Length == 0)
        {
            Debug.LogWarning("[CrouchState] No objects found within magnet range.");
            return;
        }
    
        foreach (Collider2D col in colliders)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate direction
                Vector2 playerPosition = new Vector2(stateMachine.transform.position.x, stateMachine.transform.position.y);
                Vector2 objectPosition = rb.position;
                Vector2 direction = (playerPosition - objectPosition).normalized;

                // Adjust velocity
                Vector2 velocityAdjustment = direction * attractionForce * Time.deltaTime;
                rb.linearVelocity += velocityAdjustment;
            }
        }
    }
 }
