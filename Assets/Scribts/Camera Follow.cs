using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player's transform to follow.
    private Transform player;
    // The offset from the player's position.
    public Vector3 offset;

    // The BackgroundFollow script to update.
    public BackgroundFollow backgroundFollow;

    // Public method to set the player target.
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;

        // Also set the player on the background follow script.
        if (backgroundFollow != null)
        {
            backgroundFollow.player = newTarget;
        }
    }

    void LateUpdate()
    {
        // If the player has been found, follow it.
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}