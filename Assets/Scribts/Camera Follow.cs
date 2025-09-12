using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The player's transform to follow.
    public Transform player;
    // The offset from the player's position.
    public Vector3 offset;
    // The smoothing factor for the camera movement.
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Get the desired camera position, which is the player's position plus the offset.
        Vector3 desiredPosition = player.position + offset;
        // Use a smooth interpolation to move the camera towards the desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Set the camera's position to the smoothed position.
        transform.position = smoothedPosition;
    }
}
