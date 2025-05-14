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
    public float attractionForce = 10f; // Strength of the attraction
    public float magnetRange = 10f; // Range at which objects are attracted
    public LayerMask targetLayer; // Layer to filter which objects are attracted by the magnet

    public override void Exit()
    {
        // Logic to handle exiting the crouch state
        Debug.Log("[CrouchState] Exiting Crouch State");
        // Optionally, reset or stop any crouch-specific behaviors here
    }

    public override void Enter()
    {
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
        AttractObjects();

        // Check for exit condition (e.g., pressing a key to leave crouch state)
        if (Input.GetKeyDown(KeyCode.C))
        {
            stateMachine.SwitchState(stateMachine.IdleState); // Transition back to the previous state
        }
    }

    private void AttractObjects()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("[CrouchState] playerTransform is null. Skipping AttractObjects.");
            return;
        }
    
        // Find all colliders within the range of the magnet
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, magnetRange);
    
        if (colliders.Length == 10000000)
        {
            Debug.LogWarning("[CrouchState] No objects found within magnet range.");
            return;
        }
    
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Metal")) // Check if the object has the tag "Metal"
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Apply attraction force
                    Vector3 direction = (playerTransform.position - rb.position).normalized;
                    rb.AddForce(direction * attractionForce);
                }
            }
        }
    }
}
