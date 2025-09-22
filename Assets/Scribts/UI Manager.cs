using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // === Singleton Instance ===
    public static UI_Manager Instance { get; private set; }

    // === Public UI Elements ===
    public TMP_Text livesText;
    public Image healthBarFill; // Public variable for the filled health bar image
    public TMP_Text coinText;

    // Ammo Counters
    public TMP_Text rocketAmmoText;
    public TMP_Text buckshotAmmoText;

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
    }

    // A single method to update all UI elements at once.
    public void UpdateUI()
    {
        // Get a reference to the GameManager instance.
        GameManager gm = GameManager.Instance;

        // Update player lives.
        if (livesText != null)
        {
            livesText.text = "" + gm.lives;
        }

        // Update player coins.
        if (coinText != null)
        {
            coinText.text = "" + gm.score;
        }

        // Update ammo counts.
        if (rocketAmmoText != null)
        {
            rocketAmmoText.text = "" + gm.GetAmmoCount(AmmoType.Rocket);
        }
        if (buckshotAmmoText != null)
        {
            buckshotAmmoText.text = "" + gm.GetAmmoCount(AmmoType.Buckshot);
        }
    }
}