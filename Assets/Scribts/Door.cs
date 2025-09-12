using UnityEngine;

public class Door : MonoBehaviour
{
    // A reference to the GameManager.
    private GameManager gameManager;

    private void Start()
    {
        // Find the GameManager in the scene.
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the door's trigger zone.
        if (other.CompareTag("Player"))
        {
            // You might want to add a prompt here for the player to press 'E'.
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the player is in the trigger zone and pressing the "E" key.
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Tell the GameManager to load the next level.
            gameManager.LoadNextLevel();
        }
    }
}
