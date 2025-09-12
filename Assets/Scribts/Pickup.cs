using UnityEngine;

public class Pickup : MonoBehaviour
{
    // The type of item this is.
    public PickupType pickupType;
    // The amount to give to the player.
    public int amount = 1;

    // Detect collision with the player.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            // Call the GameManager to handle the pickup.
            GameManager.Instance.HandlePickup(pickupType, amount);
            // Destroy the pickup object.
            Destroy(gameObject);
        }
    }
}
