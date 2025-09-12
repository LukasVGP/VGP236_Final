using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public float rotationSpeed = 30f; // Speed in degrees per second.
    public bool rotateClockwise = true;

    private void Update()
    {
        // Determine the direction of rotation.
        float direction = rotateClockwise ? -1f : 1f;

        // Rotate the platform around the Z-axis.
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }

    // This is crucial for the player to move with the platform.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
