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

    // We use a private dictionary and a public serializable list.
    private Dictionary<AmmoType, int> ammoDamage = new Dictionary<AmmoType, int>();

    // --- New: A serializable class to hold the damage pairs ---
    [System.Serializable]
    public class AmmoDamagePair
    {
        public AmmoType ammoType;
        public int damage;
    }
    // -----------------------------------------------------------

    [Header("Ammo Damage")]
    public AmmoDamagePair[] ammoDamageList;

    [Header("Level Progression")]
    public string[] levelOrder;
    private int currentLevelIndex = 0;
    private Vector3 lastSavePointPosition;

    [Header("Enemy Stats")]
    public EnemyStats[] enemyStats;

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
            DontDestroyOnLoad(gameObject);
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

    // Public method to get the ammo damage.
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