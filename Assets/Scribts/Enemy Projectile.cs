using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public float speed = 5f;
    public int damage = 10;
    public float lifeTime = 3f;

    private void Start()
    {
        // Destroy the projectile after a set time to prevent it from flying forever.
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the projectile forward in the direction it was fired.
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the player.
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
            // Destroy the projectile after hitting the player.
            Destroy(gameObject);
        }
    }
}
