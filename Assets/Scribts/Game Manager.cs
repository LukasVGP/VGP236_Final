using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance for easy access from other scripts.
    public static GameManager Instance;

    // Player Stats
    public int playerLives = 3;
    private int currentAmmo = 10;
    private int buckshotAmmo = 0;
    private int rocketAmmo = 0;
    public AmmoType currentAmmoType;

    // Enemy Stats
    [System.Serializable]
    public class EnemyStats
    {
        public int health;
        public int damage;
        // Add moveSpeed to the EnemyStats class.
        public float moveSpeed;
    }

    public List<EnemyStats> enemyStats;
    public List<GameObject> enemyPrefabs;

    // Level Management
    public string[] levelOrder; // Assign scene names in the Inspector.
    private int currentLevelIndex = 0;
    private Vector2 lastSavePoint;

    // UI Manager reference.
    public UI_Manager uiManager;

    private void Awake()
    {
        // Singleton pattern implementation.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the last save point to the start of the first level.
        lastSavePoint = Vector2.zero;
        currentAmmoType = AmmoType.Default;
        // Update the UI at the start of the game.
        uiManager.UpdateLives(playerLives);
        uiManager.UpdateAllAmmo(currentAmmo, buckshotAmmo, rocketAmmo);
    }

    // --- Player and Ammo Management ---
    public void DeductAmmo(AmmoType type)
    {
        switch (type)
        {
            case AmmoType.Default:
                currentAmmo--;
                break;
            case AmmoType.Buckshot:
                buckshotAmmo--;
                break;
            case AmmoType.Rocket:
                rocketAmmo--;
                break;
        }
        uiManager.UpdateAllAmmo(currentAmmo, buckshotAmmo, rocketAmmo);
    }

    public int GetAmmoCount(AmmoType type)
    {
        switch (type)
        {
            case AmmoType.Default:
                return currentAmmo;
            case AmmoType.Buckshot:
                return buckshotAmmo;
            case AmmoType.Rocket:
                return rocketAmmo;
            default:
                return 0;
        }
    }

    public void HandlePickup(PickupType type, int amount)
    {
        switch (type)
        {
            case PickupType.DefaultAmmo:
                currentAmmo += amount;
                uiManager.UpdateAllAmmo(currentAmmo, buckshotAmmo, rocketAmmo);
                break;
            case PickupType.BuckshotAmmo:
                buckshotAmmo += amount;
                uiManager.UpdateAllAmmo(currentAmmo, buckshotAmmo, rocketAmmo);
                break;
            case PickupType.RocketAmmo:
                rocketAmmo += amount;
                uiManager.UpdateAllAmmo(currentAmmo, buckshotAmmo, rocketAmmo);
                break;
            case PickupType.Life:
                playerLives += amount;
                uiManager.UpdateLives(playerLives);
                break;
        }
    }

    // --- Level and Game State Management ---
    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelOrder.Length)
        {
            SceneManager.LoadScene(levelOrder[currentLevelIndex]);
            // Reset the save point for the new level
            lastSavePoint = Vector2.zero;
        }
        else
        {
            // Game Win Condition
            SceneManager.LoadScene("GameWinMenu");
        }
    }

    public void OnPlayerDeath()
    {
        playerLives--;
        uiManager.UpdateLives(playerLives);

        if (playerLives <= 0)
        {
            // Game Over Condition
            SceneManager.LoadScene("GameOverMenu");
        }
        else
        {
            // Respawn the player
            // Find the player object in the scene and move it to the last save point.
            // This assumes the player object has the tag "Player".
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = lastSavePoint;
            }
        }
    }

    public void SetSavePoint(Vector2 position)
    {
        lastSavePoint = position;
    }

    // --- Enemy and Boss Management ---
    public EnemyStats GetEnemyStats(int index)
    {
        if (index >= 0 && index < enemyStats.Count)
        {
            return enemyStats[index];
        }
        else
        {
            Debug.LogError("Enemy stats index out of range!");
            return new EnemyStats();
        }
    }

    public void OnBossDefeated()
    {
        // This method will be called by the boss AI script upon its defeat.
        LoadNextLevel();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelOrder[0]);
    }
}