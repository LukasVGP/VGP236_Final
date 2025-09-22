using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager.cs: The central hub for all game state and progression.
public class GameManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance exists.
    public static GameManager Instance { get; private set; }

    [Header("Player Stats")]
    public int lives = 3;
    public int score = 0;
    public AmmoType currentAmmoType;
    public Dictionary<AmmoType, int> ammoCount = new Dictionary<AmmoType, int>();

    [Header("Ammo Damage")]
    public AmmoDamagePair[] ammoDamageList;
    public Dictionary<AmmoType, int> ammoDamage = new Dictionary<AmmoType, int>();

    [Header("Level Progression")]
    public string[] levelOrder;
    private int currentLevelIndex = 0;
    private Vector3 lastSavePointPosition;

    [Header("Enemy Stats")]
    public EnemyStats[] enemyStats;

    // --- New: Boss and Win Condition variables ---
    public string bossLevelName;
    private bool bossIsDead = false;
    // ---------------------------------------------

    // Define EnemyStats as a nested class here.
    [System.Serializable]
    public class AmmoDamagePair
    {
        public AmmoType ammoType;
        public int damage;
    }

    // Define EnemyStats as a nested class here.
    [System.Serializable]
    public class EnemyStats
    {
        public EnemyType enemyType;
        public float health;
        public float damage;
        public float moveSpeed;
    }

    void Awake()
    {
        // Singleton enforcement.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensures the GameManager persists across scenes.
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize ammo counts.
        ammoCount.Add(AmmoType.Default, 999);
        ammoCount.Add(AmmoType.Buckshot, 0);
        ammoCount.Add(AmmoType.Rocket, 0);

        // Populate the ammoDamage dictionary from the serializable list.
        foreach (var pair in ammoDamageList)
        {
            if (!ammoDamage.ContainsKey(pair.ammoType))
            {
                ammoDamage.Add(pair.ammoType, pair.damage);
            }
        }
    }

    // You will need a public method to get the ammo damage.
    public int GetAmmoDamage(AmmoType type)
    {
        if (ammoDamage.ContainsKey(type))
        {
            return ammoDamage[type];
        }
        return 0;
    }

    // Handles player death and respawn.
    public void OnPlayerDeath()
    {
        lives--;
        if (lives <= 0)
        {
            // Load Game Over screen if all lives are lost.
            SceneManager.LoadScene("GameOverMenu");
        }
        else
        {
            // Respawn at the last save point.
            if (PlayerController.instance != null)
            {
                PlayerController.instance.transform.position = lastSavePointPosition;
            }
        }
    }

    // --- New: Handles the boss defeat win condition ---
    public void BossDefeated()
    {
        bossIsDead = true;
        Debug.Log("Boss defeated! Game won.");
        SceneManager.LoadScene("GameWinMenu");
    }
    // ----------------------------------------------------

    // Sets the player's last save point.
    public void SetSavePoint(Vector3 position)
    {
        lastSavePointPosition = position;
        Debug.Log("Save point set at: " + position);
    }

    // Loads the next level in the sequence.
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelOrder.Length)
        {
            SceneManager.LoadScene(levelOrder[currentLevelIndex]);
        }
        else
        {
            // All levels complete, load win screen.
            SceneManager.LoadScene("GameWinMenu");
        }
    }

    // Adds coins to the score.
    public void AddCoins(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    // Adds ammo of a specific type.
    public void AddAmmo(AmmoType type, int amount)
    {
        if (ammoCount.ContainsKey(type))
        {
            ammoCount[type] += amount;
        }
    }

    // Deducts one unit of ammo.
    public void DeductAmmo(AmmoType type)
    {
        if (ammoCount.ContainsKey(type) && ammoCount[type] > 0)
        {
            ammoCount[type]--;
        }
    }

    // Returns the current ammo count for a given type.
    public int GetAmmoCount(AmmoType type)
    {
        if (ammoCount.ContainsKey(type))
        {
            return ammoCount[type];
        }
        return 0;
    }

    // Handles pickup events.
    public void HandlePickup(PickupType type, int amount)
    {
        switch (type)
        {
            case PickupType.BuckshotAmmo:
                AddAmmo(AmmoType.Buckshot, amount);
                Debug.Log("Picked up " + amount + " Buckshot Ammo.");
                break;
            case PickupType.RocketAmmo:
                AddAmmo(AmmoType.Rocket, amount);
                Debug.Log("Picked up " + amount + " Rocket Ammo.");
                break;
            case PickupType.Life:
                lives += amount;
                Debug.Log("Picked up a Life.");
                break;
            default:
                break;
        }
    }
}