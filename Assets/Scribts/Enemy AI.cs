using System.Collections;
using UnityEngine;

// EnemyAI.cs: Manages all enemy behaviors (Melee, Shooting, Ramming).
public class EnemyAI : MonoBehaviour
{
    // Public variables set in the Unity Inspector.
    public EnemyType enemyType;
    protected float health;
    protected float damage;
    protected float moveSpeed;
    protected float maxHealth;

    // References to other components.
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    protected Rigidbody2D rb;
    protected bool isActive = false; // New variable to track if the enemy is active.

    // --- New variables for flipping ---
    public GameObject front;
    public GameObject back;
    private bool facingRight = true;
    // ----------------------------------

    // Change the Start method to just get references, and use a new public method to start behavior.
    protected virtual void Start()
    {
        // Get the enemy stats from the GameManager.
        GameManager.EnemyStats stats = GameManager.Instance.enemyStats[(int)enemyType];
        health = stats.health;
        damage = stats.damage;
        moveSpeed = stats.moveSpeed;

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // New: Activates the enemy's behavior.
    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            // Start the behavior coroutine based on the enemy type.
            switch (enemyType)
            {
                case EnemyType.Melee:
                    StartCoroutine(MeleeBehavior());
                    break;
                case EnemyType.Shooting:
                    StartCoroutine(ShootingBehavior());
                    break;
                case EnemyType.Ramming:
                    StartCoroutine(RammingBehavior());
                    break;
            }
        }
    }

    // New: This method is called when another object enters the trigger zone.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }

    // Handles movement for all enemy types.
    void Update()
    {
        if (isActive && player != null) // Only run if the enemy is active.
        {
            // --- New: Flip the enemy based on player position ---
            Flip(player.position.x);
            // ---------------------------------------------------

            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    // --- New Flip() method ---
    private void Flip(float playerXPosition)
    {
        // If the player is to the right and the enemy is not facing right...
        if (playerXPosition > transform.position.x && !facingRight)
        {
            facingRight = true;
            front.transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
            back.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        // If the player is to the left and the enemy is facing right...
        else if (playerXPosition < transform.position.x && facingRight)
        {
            facingRight = false;
            front.transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
            back.transform.rotation = Quaternion.Euler(0, 180, 180);
        }
    }
    // ---------------------------------------

    // Coroutine for the Melee enemy's behavior.
    private IEnumerator MeleeBehavior()
    {
        while (true)
        {
            // The enemy simply moves towards the player to make contact.
            yield return null;
        }
    }

    // Coroutine for the Shooting enemy's behavior.
    private IEnumerator ShootingBehavior()
    {
        while (true)
        {
            // Enemy stops, shoots, and waits.
            rb.linearVelocity = Vector2.zero;
            Shoot();
            yield return new WaitForSeconds(2.0f);
        }
    }

    // Coroutine for the Ramming enemy's behavior.
    private IEnumerator RammingBehavior()
    {
        while (true)
        {
            // Enemy waits, then charges at the player.
            yield return new WaitForSeconds(2.0f);
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed * 2, rb.linearVelocity.y);
        }
    }

    // Handles the shooting logic for the enemy.
    protected void Shoot()
    {
        if (projectilePrefab != null && firePoint != null && player != null)
        {
            // Calculate the direction from the fire point to the player.
            Vector2 direction = (player.position - firePoint.position).normalized;

            // Calculate the rotation to face the player.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // Instantiate the projectile with the new rotation.
            Instantiate(projectilePrefab, firePoint.position, rotation);
        }
    }

    // Handles taking damage.
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Handles the enemy's death.
    private void Die()
    {
        Destroy(gameObject);
    }

    // Handles melee contact damage and player knockback.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemyType != EnemyType.Shooting)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage((int)damage); // Cast to int
            }

            if (enemyType == EnemyType.Ramming)
            {
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                    playerRb.AddForce(knockbackDirection * 500f);
                }
            }
        }
    }
}