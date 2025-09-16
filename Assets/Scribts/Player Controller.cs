using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManagement namespace

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

    // --- Health Variables ---
    public int maxHealth = 100;
    private int currentHealth;
    // -----------------------------

    // References to other components.
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    // Tracks if the player is on the ground.
    private bool isGrounded;
    private bool isRocketJumping = false;
    private bool canRocketJump = true;

    // Use a static reference for the Singleton pattern
    public static PlayerController instance;

    void Awake()
    {
        // Enforce the Singleton pattern.
        if (instance == null)
        {
            instance = this;
            // This is the crucial line: it prevents the player from being destroyed.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another player already exists, destroy this one.
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event when the object is enabled.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the event when the object is disabled.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the spawn point in the new scene.
        SpawnPoint spawnPoint = FindObjectOfType<SpawnPoint>();
        if (spawnPoint != null)
        {
            // Move the player to the spawn point's position.
            transform.position = spawnPoint.transform.position;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (blunderbuss == null)
        {
            blunderbuss = GetComponentInChildren<Blunderbuss>();
        }
        audioSource = GetComponent<AudioSource>();

        // Set the player's current health to their max health at the start of the level.
        currentHealth = maxHealth;
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
            if (GameManager.Instance.GetAmmoCount(AmmoType.Rocket) > 0 && canRocketJump)
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

        GameManager.Instance.DeductAmmo(AmmoType.Rocket);

        if (rocketJumpMuzzleFlashPrefab != null && rocketJumpFirePoint != null)
        {
            Instantiate(rocketJumpMuzzleFlashPrefab, rocketJumpFirePoint.position, rocketJumpFirePoint.rotation, rocketJumpFirePoint);
        }
        if (audioSource != null && rocketJumpSound != null)
        {
            audioSource.clip = rocketJumpSound;
            audioSource.Play();
        }

        float timer = 0f;
        while (timer < rocketBurnTime)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            timer += Time.deltaTime;
            yield return null;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);

        isRocketJumping = false;
        yield return new WaitForSeconds(rocketCooldown);
        canRocketJump = true;
    }

    // Handles taking damage from enemies or hazards.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            GameManager.Instance.OnPlayerDeath();
        }
    }
}