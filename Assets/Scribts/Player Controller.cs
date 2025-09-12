using System.Collections;
using UnityEngine;

// PlayerController.cs: Manages all player input and movement.
public class PlayerController : MonoBehaviour
{
    // Public variables set in the Unity Inspector.
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float rocketBurnTime = 0.5f;
    public float rocketCooldown = 2.0f;

    // References to other components.
    private Rigidbody2D rb;
    private Animator animator;

    // Tracks if the player is on the ground.
    private bool isGrounded;
    private bool isRocketJumping = false;
    private bool canRocketJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is on the ground.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Handle horizontal movement.
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Handle jumping and rocket jump.
        if (Input.GetButtonDown("Jump") && isGrounded && !isRocketJumping)
        {
            // Check if the player has rocket ammo.
            if (GameManager.Instance.currentAmmoType == AmmoType.Rocket && canRocketJump)
            {
                StartCoroutine(RocketJumpRoutine());
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    // Coroutine for the rocket jump.
    private IEnumerator RocketJumpRoutine()
    {
        isRocketJumping = true;
        canRocketJump = false;

        float timer = 0f;
        while (timer < rocketBurnTime)
        {
            // Apply upward force without overriding horizontal velocity.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            timer += Time.deltaTime;
            yield return null;
        }

        // After the burn time, set vertical velocity to zero to allow gravity to take over.
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        isRocketJumping = false;
        yield return new WaitForSeconds(rocketCooldown);
        canRocketJump = true;
    }

    // Handles taking damage from enemies or hazards.
    public void TakeDamage(int damage)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDeath();
        }
    }
}