using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public Vector3[] waypoints; // Array of points the platform will move between.
    public float speed = 2f;
    public float waitTime = 1f; // How long to wait at each waypoint.

    // === Private Variables ===
    private int currentWaypointIndex = 0;
    private float waitTimer;

    private void Start()
    {
        waitTimer = waitTime;
    }

    private void Update()
    {
        // Move towards the current waypoint.
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex], speed * Time.deltaTime);

        // Check if the platform has reached the waypoint.
        if (transform.position == waypoints[currentWaypointIndex])
        {
            // Start the wait timer.
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                // Move to the next waypoint.
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                waitTimer = waitTime;
            }
        }
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
