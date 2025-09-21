using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI
{
    [Header("Boss Specifics")]
    public float meleeAttackDistance = 2f;
    public float shootingAttackDistance = 10f;
    public float speedUpHealthThreshold = 0.2f;
    public float speedUpMultiplier = 2f;
    public float meleeKnockbackForce = 500f; // New variable for knockback force.

    // Override the base Start method.
    protected override void Start()
    {
        // Call the base class's Start method to initialize common properties.
        base.Start();

        // Start the unique boss behavior.
        StartCoroutine(BossBehavior());
    }

    // The coroutine that handles the boss's attack pattern.
    private IEnumerator BossBehavior()
    {
        while (true)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Phase 1: Shoot at the player if they are far away.
            while (distanceToPlayer > meleeAttackDistance)
            {
                rb.linearVelocity = Vector2.zero; // Stop to shoot.
                Shoot();
                yield return new WaitForSeconds(2.0f);
                distanceToPlayer = Vector2.Distance(transform.position, player.position);
            }

            // Phase 2: Charge the player for a melee attack.
            while (distanceToPlayer <= shootingAttackDistance)
            {
                // Move towards the player for the melee attack.
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

                distanceToPlayer = Vector2.Distance(transform.position, player.position);

                // If the player moves out of range, break the loop and go back to shooting.
                if (distanceToPlayer > shootingAttackDistance)
                {
                    rb.linearVelocity = Vector2.zero;
                    break;
                }
                yield return null;
            }

            // Reset the behavior after the melee or a failed charge.
            yield return new WaitForSeconds(1.0f); // A short pause before the next attack.
        }
    }

    // Override the base OnCollisionEnter2D to handle the boss's melee damage and knockback.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                // Apply damage to the player.
                playerScript.TakeDamage((int)damage);

                // --- Knock the player back after a successful melee attack. ---
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDirection = (player.position - transform.position).normalized;
                    playerRb.AddForce(knockbackDirection * meleeKnockbackForce);
                }
            }
        }
    }

    // The boss's speed-up logic.
    void Update()
    {
        // Check for the boss's health threshold to speed up.
        if (health <= maxHealth * speedUpHealthThreshold)
        {
            moveSpeed *= speedUpMultiplier;
        }
    }
}