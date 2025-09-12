using UnityEngine;

public class SavePoint : MonoBehaviour
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
        // Check if the player has entered the save point's trigger zone.
        if (other.CompareTag("Player"))
        {
            // Tell the GameManager to save the player's position.
            gameManager.SetSavePoint(transform.position);

            // You might want to add a visual cue here to show the save point is activated.
            Debug.Log("Save point activated!");
        }
    }
}
