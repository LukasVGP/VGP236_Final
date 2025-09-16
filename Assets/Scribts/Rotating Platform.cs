using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public float rotationSpeed = 30f; // Speed in degrees per second.
    public bool rotateClockwise = true;

    // Private variables to track rotation
    private Vector2 lastPosition;
    private float lastRotationAngle;

    private void Start()
    {
        lastPosition = transform.position;
        lastRotationAngle = transform.eulerAngles.z;
    }

    private void Update()
    {
        // Determine the direction of rotation.
        float direction = rotateClockwise ? -1f : 1f;

        // Rotate the platform around the Z-axis.
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Calculate the platform's change in position and rotation
                Vector2 currentPosition = transform.position;
                float currentRotationAngle = transform.eulerAngles.z;

                Vector2 positionDelta = currentPosition - lastPosition;
                float rotationDelta = currentRotationAngle - lastRotationAngle;

                // Apply the platform's movement and rotation to the player's Rigidbody
                Vector2 rotatedDelta = Quaternion.Euler(0, 0, rotationDelta) * positionDelta;
                playerRb.linearVelocity += rotatedDelta / Time.deltaTime;

                // Update last position and rotation for the next frame
                lastPosition = currentPosition;
                lastRotationAngle = currentRotationAngle;

                // You can still choose to freeze player rotation on the Z-axis to keep them upright
                // playerRb.freezeRotation = true;
            }
        }
    }

    // OnCollisionExit2D is no longer needed with this approach.
}