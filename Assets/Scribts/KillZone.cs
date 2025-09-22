using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the OnPlayerDeath method in the GameManager.
            GameManager.Instance.OnPlayerDeath();
        }
    }
}