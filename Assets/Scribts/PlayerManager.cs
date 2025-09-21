using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    void Awake()
    {
        // Check if an instance of the player already exists.
        if (PlayerController.instance == null)
        {
            // If not, instantiate the player from the prefab and make it persistent.
            Instantiate(playerPrefab);
        }
        // If an instance already exists, do nothing, as it will be carried over.
    }
}