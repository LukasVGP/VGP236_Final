using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageRate = 1.0f; // Damage per second.

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Apply damage over time.
                player.TakeDamage((int)(damageAmount * Time.deltaTime));
            }
        }
    }
}