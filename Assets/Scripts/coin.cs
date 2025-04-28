using UnityEngine;

public class coin : MonoBehaviour
{
    public int coins = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = 0;
    }

    
    
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            coins += 1; // Correct increment operation
            // Destroy the coin object
            Destroy(gameObject);
        }
    }
    
}
