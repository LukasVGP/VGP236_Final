using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this line

public class BossAI : EnemyAI
{
    [Header("Boss Specifics")]
    public float meleeAttackDistance = 2f;
    public float shootingAttackDistance = 10f;
    public float speedUpHealthThreshold = 0.2f;
    public float speedUpMultiplier = 2f;
    public float meleeKnockbackForce = 500f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossBehavior());
    }

    private IEnumerator BossBehavior()
    {
        while (true)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            while (distanceToPlayer > meleeAttackDistance)
            {
                rb.linearVelocity = Vector2.zero;
                Shoot();
                yield return new WaitForSeconds(2.0f);
                distanceToPlayer = Vector2.Distance(transform.position, player.position);
            }

            while (distanceToPlayer <= shootingAttackDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

                distanceToPlayer = Vector2.Distance(transform.position, player.position);

                if (distanceToPlayer > shootingAttackDistance)
                {
                    rb.linearVelocity = Vector2.zero;
                    break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage((int)damage);

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 knockbackDirection = (player.position - transform.position).normalized;
                    playerRb.AddForce(knockbackDirection * meleeKnockbackForce);
                }
            }
        }
    }

    void Update()
    {
        if (health <= maxHealth * speedUpHealthThreshold)
        {
            moveSpeed *= speedUpMultiplier;
        }
    }

    private void Die()
    {
        if (SceneManager.GetActiveScene().name == GameManager.Instance.bossLevelName)
        {
            GameManager.Instance.BossDefeated();
        }
        Destroy(gameObject);
    }
}