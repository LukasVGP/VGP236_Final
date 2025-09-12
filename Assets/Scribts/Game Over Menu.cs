using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        // Load the main menu scene.
        SceneManager.LoadScene("MainMenu");
    }
}
