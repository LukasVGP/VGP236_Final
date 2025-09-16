using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1; // You can set this in the Inspector.

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            // Call the GameManager to add coins to the score.
            GameManager.Instance.AddCoins(coinValue);

            // Destroy the coin object.
            Destroy(gameObject);
        }
    }
}