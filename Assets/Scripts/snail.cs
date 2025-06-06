using UnityEngine;

public class Snail : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 currentDirection = Vector2.right; // Default direction is right

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = currentDirection * speed; // Set initial movement direction
    }

    void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            // Flip the snail's direction
            currentDirection = currentDirection == Vector2.right ? Vector2.left : Vector2.right;

            // Update the snail's velocity
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = currentDirection * speed;

            // Flip the snail's sprite horizontally
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        /*else
        {
            // Ensure the snail continues moving in the current direction
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = currentDirection * speed;
        }*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Make the snail jump
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.up * speed;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Ensure the snail continues moving in the current direction
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = currentDirection * speed;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the application
            Application.Quit();

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            // Toggle full screen mode
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                Destroy(collider.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
