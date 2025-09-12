using UnityEngine;
using UnityEngine.UI; // For older Text components
using TMPro; // For TextMeshPro

public class UI_Manager : MonoBehaviour
{
    // === Singleton Instance ===
    public static UI_Manager Instance { get; private set; }

    // === Public UI Elements ===
    public TMP_Text livesText;
    public TMP_Text timerText;
    public TMP_Text standardAmmoText;
    public TMP_Text explodingAmmoText;
    public TMP_Text buckshotAmmoText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    public void UpdateTimer(float time)
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + time.ToString("F2");
        }
    }

    public void UpdateAllAmmo(int standard, int exploding, int buckshot)
    {
        if (standardAmmoText != null) standardAmmoText.text = "Standard: " + standard;
        if (explodingAmmoText != null) explodingAmmoText.text = "Exploding: " + exploding;
        if (buckshotAmmoText != null) buckshotAmmoText.text = "Buckshot: " + buckshot;
    }
}
