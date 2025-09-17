using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player's transform to follow.
    public Transform player;
    // The offset from the player's position.
    public Vector3 offset;

    private void Start()
    {
        // Find the persistent player object in the scene.
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        else
        {
            // Optional: Log an error if the player is not found.
            Debug.LogError("Player object not found. Make sure you start the game from the main scene.");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Set the camera's position to the player's position plus the offset.
            transform.position = player.position + offset;
        }
    }
}