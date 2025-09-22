using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public Transform startPoint; // The starting point GameObject.
    public Transform endPoint;   // The ending point GameObject.
    public float speed = 2f;
    public float waitTime = 1f; // How long to wait at each waypoint.

    // === Private Variables ===
    private Vector3 targetPosition;
    private float waitTimer;
    private bool movingToEnd = true;

    private void Start()
    {
        // Set the initial position and target based on the provided Transforms.
        transform.position = startPoint.position;
        targetPosition = endPoint.position;
        waitTimer = waitTime;
    }

    private void Update()
    {
        // Move towards the current target.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the target using a small threshold.
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Start the wait timer.
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                // Switch the target.
                movingToEnd = !movingToEnd;
                targetPosition = movingToEnd ? endPoint.position : startPoint.position;
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
            StartCoroutine(SetParentAfterDelay(collision.transform, null));
        }
    }

    // This coroutine waits for one frame before setting the parent to null.
    private IEnumerator SetParentAfterDelay(Transform child, Transform newParent)
    {
        yield return new WaitForEndOfFrame();
        child.SetParent(newParent);
    }
}