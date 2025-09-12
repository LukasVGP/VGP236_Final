using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A Serializable class to hold enemy data that can be set in the Unity Inspector.
/// </summary>
[System.Serializable]
public class EnemyStats
{
    public string enemyTypeName;
    public float health;
    public float damage;
}

public class GameManager : MonoBehaviour
{
    // === Singleton Instance ===
    public static GameManager Instance { get; private set; }

    // === Public Variables for Inspector Setup ===
    public string[] levelScenes;
    public List<EnemyStats> enemyStats;
    public int playerLives = 3;
    public int standardAmmo = 50;
    public int explodingAmmo = 5;
    public int buckshotAmmo = 10;

    // === Private Variables ===
    private int currentLevelIndex = 0;
    private Vector3 currentSavePoint;
    private float gameTime = 0f;
    private Dictionary<string, EnemyStats> enemyStatDictionary = new Dictionary<string, EnemyStats>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var stats in enemyStats)
        {
            enemyStatDictionary[stats.enemyTypeName] = stats;
        }
    }

    private void Update()
    {
        gameTime += Time.deltaTime;
        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.UpdateTimer(gameTime);
        }
    }

    public void StartGame()
    {
        currentLevelIndex = 0;
        SceneManager.LoadScene(levelScenes[currentLevelIndex]);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelScenes.Length)
        {
            SceneManager.LoadScene(levelScenes[currentLevelIndex]);
        }
        else
        {
            SceneManager.LoadScene("GameWinScene");
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPlayerDeath()
    {
        playerLives--;
        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.UpdateLives(playerLives);
        }
        if (playerLives > 0)
        {
            if (currentSavePoint != Vector3.zero)
            {
                // Respawn player here
            }
            else
            {
                RestartLevel();
            }
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void SetSavePoint(Vector3 position)
    {
        currentSavePoint = position;
    }

    public EnemyStats GetEnemyStats(string enemyType)
    {
        if (enemyStatDictionary.ContainsKey(enemyType))
        {
            return enemyStatDictionary[enemyType];
        }
        return null;
    }

    public void HandlePickup(PickupType type, int amount)
    {
        switch (type)
        {
            case PickupType.StandardAmmo:
                standardAmmo += amount;
                break;
            case PickupType.ExplodingAmmo:
                explodingAmmo += amount;
                break;
            case PickupType.BuckshotAmmo:
                buckshotAmmo += amount;
                break;
            case PickupType.Life:
                playerLives += amount;
                if (UI_Manager.Instance != null)
                {
                    UI_Manager.Instance.UpdateLives(playerLives);
                }
                break;
        }

        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.UpdateAllAmmo(standardAmmo, explodingAmmo, buckshotAmmo);
        }
    }

    public bool HasAmmo(AmmunitionType type)
    {
        switch (type)
        {
            case AmmunitionType.Standard:
                return standardAmmo > 0;
            case AmmunitionType.Exploding:
                return explodingAmmo > 0;
            case AmmunitionType.Buckshot:
                return buckshotAmmo > 0;
            default:
                return false;
        }
    }

    public void DeductAmmo(AmmunitionType type, int amount)
    {
        switch (type)
        {
            case AmmunitionType.Standard:
                standardAmmo = Mathf.Max(0, standardAmmo - amount);
                break;
            case AmmunitionType.Exploding:
                explodingAmmo = Mathf.Max(0, explodingAmmo - amount);
                break;
            case AmmunitionType.Buckshot:
                buckshotAmmo = Mathf.Max(0, buckshotAmmo - amount);
                break;
        }

        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.UpdateAllAmmo(standardAmmo, explodingAmmo, buckshotAmmo);
        }
    }
}
