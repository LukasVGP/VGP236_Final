using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    // The player's transform to follow.
    public Transform player;

    void Awake()
    {
        // Find the persistent player object in the scene as soon as this script wakes up.
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        else
        {
            Debug.LogError("Player object not found. The background will not follow.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Only update the background's X position to follow the player.
            // The Y position (height) and Z position (depth) remain fixed.
            Vector3 newPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}