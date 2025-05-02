using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Value of the coin

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player collects the coin
        {
            // Add coin value to player's score (assuming a GameManager handles the score)
            

            // Destroy the coin object
            Destroy(gameObject);
        }
    }
}