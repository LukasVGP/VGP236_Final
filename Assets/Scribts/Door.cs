using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Public variable to set the target scene in the Inspector.
    // You can type the name of the scene here.
    public string targetSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider entering the trigger belongs to the player.
        // It's important that your player GameObject has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            // Load the specified target scene.
            SceneManager.LoadScene(targetSceneName);
        }
    }
}