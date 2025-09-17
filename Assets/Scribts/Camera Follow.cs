using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
        else
        {
            Debug.LogError("Player transform is not assigned to the CameraFollow script.");
        }
    }
}