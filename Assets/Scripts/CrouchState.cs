using UnityEngine;

public class CrouchState : PlayerBaseState 
{
    private Transform playerTransform; // Reference to the player's transform
    public float attractionForce = 10f; // Strength of the attraction
    public float magnetRange = 10f; // Range at which objects are attracted
    public LayerMask targetLayer; // Layer to filter which objects are attracted by the magnet

    
    public override void Enter()
    { GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
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
    }

    private void AttractObjects()
    {
        // Find all colliders within the range of the magnet
        if (playerTransform == null)
        {
            Debug.LogWarning("[CrouchState] playerTransform is null. Skipping AttractObjects.");
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, magnetRange, targetLayer);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate direction towards the magnet
                Vector3 direction = (playerTransform.position - col.transform.position).normalized;

                // Apply force towards the magnet
                rb.AddForce(direction * attractionForce * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }

    public override void Exit()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("[CrouchState] playerTransform is null. Cannot draw Gizmos.");
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, magnetRange);
        // Visualize the magnet range in the editor
        if (playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerTransform.position, magnetRange);
        }
    }
}
