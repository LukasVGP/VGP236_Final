using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the first level of the game.
        // Make sure to add the levels to the build settings in Unity.
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        // Quits the application.
        // Note: This only works in a built game, not in the Unity editor.
        Application.Quit();
    }
}
