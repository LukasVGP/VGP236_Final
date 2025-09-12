using UnityEngine;

// PlayerController.cs: Manages all player input and movement.
public class PlayerController : MonoBehaviour
{
    // Public variables set in the Unity Inspector.
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // References to other components.
    private Rigidbody2D rb;
    private Animator animator;

    // Tracks if the player is on the ground.
    private bool isGrounded;

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

        // Handle jumping.
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Handles taking damage from enemies or hazards.
    public void TakeDamage(int damage)
    {
        GameManager.Instance.OnPlayerDeath();
    }
}