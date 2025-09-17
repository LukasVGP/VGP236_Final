using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    // The player's transform to follow.
    public Transform player;

    void Update()
    {
        // Only update the background's X position to follow the player.
        // The Y position (height) and Z position (depth) remain fixed.
        Vector3 newPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
}