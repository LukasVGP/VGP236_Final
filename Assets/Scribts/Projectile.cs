using UnityEngine;

public class Projectile : MonoBehaviour
{
    // --- Public variables set in the Inspector ---
    public AmmoType ammoType;
    public float speed;
    public int damage;
    public float lifeTime = 2f;
    // --------------------------------------------------

    // --- Public variable for Inspector Setup (Rocket Explosion) ---
    public GameObject explosionFlashPrefab;
    // ----------------------------------------------------------------

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
            // Apply damage based on the public 'damage' variable set in the Inspector.
            enemy.TakeDamage(damage);

            // If the projectile is a rocket, instantiate an explosion.
            if (ammoType == AmmoType.Rocket && explosionFlashPrefab != null)
            {
                // Instantiate the explosion prefab at the collision point.
                GameObject explosion = Instantiate(explosionFlashPrefab, transform.position, Quaternion.identity);
                // Destroy the explosion after a short duration.
                Destroy(explosion, 0.5f);
            }

            // Destroy the projectile after hitting.
            Destroy(gameObject);
        }
    }
}