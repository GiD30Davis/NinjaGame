using UnityEngine;

public class BulletGo : MonoBehaviour
{
    public float speed = 10f; // Speed of the bullet
    private Vector2 direction;

    void Start()
    {
        // Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Determine the direction based on the player's local scale
            direction = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        // Destroy the bullet after 5 seconds to prevent memory leaks
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Move the bullet in the determined direction
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
}
