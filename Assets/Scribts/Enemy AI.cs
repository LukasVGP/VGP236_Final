using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public EnemyType enemyType;
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 3f;
    public float stoppingDistance = 1f;

    // === Private Variables ===
    private int currentHealth;
    private int damage;

    private void Start()
    {
        // Get enemy stats from the GameManager based on the type.
        EnemyStats stats = GameManager.Instance.GetEnemyStats(enemyType);
        currentHealth = stats.health;
        damage = stats.damage;

        // Find the player object in the scene.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        // Behavior for all enemy types.
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        // Specific behavior for different enemy types can be added here.
        switch (enemyType)
        {
            case EnemyType.Melee:
                // Melee specific logic (e.g., attacking when close).
                break;
            case EnemyType.Shooting:
                // Shooting specific logic (e.g., firing projectiles).
                break;
            case EnemyType.Ramming:
                // Ramming specific logic (e.g., charging at player).
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Called on contact with the player.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
            }
        }
    }
}

public enum EnemyType
{
    Melee,
    Shooting,
    Ramming
}
