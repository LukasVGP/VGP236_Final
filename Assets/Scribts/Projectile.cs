using UnityEngine;

public class Projectile : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 2f;

    private void Start()
    {
        // Destroy the projectile after a set time to prevent it from flying forever.
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Move the projectile forward.
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits an enemy.
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            // Destroy the projectile after hitting.
            Destroy(gameObject);
        }
    }
}
