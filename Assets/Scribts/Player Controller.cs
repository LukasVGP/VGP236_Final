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
    public Blunderbuss blunderbuss;

    // --- Variables for the rocket jump effect ---
    public GameObject rocketJumpMuzzleFlashPrefab;
    public Transform rocketJumpFirePoint;
    public AudioClip rocketJumpSound;
    // --------------------------------------------------

    // References to other components.
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    // Tracks if the player is on the ground.
    private bool isGrounded;
    private bool isRocketJumping = false;
    private bool canRocketJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (blunderbuss == null)
        {
            blunderbuss = GetComponentInChildren<Blunderbuss>();
        }
        audioSource = GetComponent<AudioSource>();
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
            if (GameManager.Instance.currentAmmoType == AmmoType.Rocket && canRocketJump)
            {
                StartCoroutine(RocketJumpRoutine());
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        // Handle shooting input
        if (Input.GetButtonDown("Fire1"))
        {
            if (blunderbuss != null)
            {
                blunderbuss.StartShooting();
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (blunderbuss != null)
            {
                blunderbuss.StopShooting();
            }
        }

        // Handle ammo switching input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.currentAmmoType = AmmoType.Default;
            Debug.Log("Switched to Default Ammo");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.currentAmmoType = AmmoType.Buckshot;
            Debug.Log("Switched to Buckshot Ammo");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.currentAmmoType = AmmoType.Rocket;
            Debug.Log("Switched to Rocket Ammo");
        }
    }

    // Coroutine for the rocket jump.
    private IEnumerator RocketJumpRoutine()
    {
        isRocketJumping = true;
        canRocketJump = false;

        // Play sound and spawn muzzle flash on rocket jump.
        if (rocketJumpMuzzleFlashPrefab != null && rocketJumpFirePoint != null)
        {
            // Instantiate the muzzle flash.
            Instantiate(rocketJumpMuzzleFlashPrefab, rocketJumpFirePoint.position, rocketJumpFirePoint.rotation);
        }
        if (audioSource != null && rocketJumpSound != null)
        {
            audioSource.clip = rocketJumpSound;
            audioSource.Play();
        }

        float timer = 0f;
        while (timer < rocketBurnTime)
        {
            // Apply upward force without overriding horizontal velocity.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop the sound when the burn time is over.
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
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