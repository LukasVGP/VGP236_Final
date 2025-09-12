using System.Collections;
using UnityEngine;

// Blunderbuss.cs: Manages the player's weapon and ammunition types.
public class Blunderbuss : MonoBehaviour
{
    // Public variables set in the Unity Inspector.
    public GameObject singleShotPrefab;
    public GameObject buckshotPrefab;
    public GameObject rocketPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    // References to other components.
    private Coroutine fireCoroutine;

    // Handles the shooting input.
    public void StartShooting()
    {
        if (fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireRoutine());
        }
    }

    public void StopShooting()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    // The coroutine that handles the firing logic.
    private IEnumerator FireRoutine()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }

    // Handles the actual firing of a projectile.
    private void Fire()
    {
        // Get the current ammo type from the GameManager.
        AmmoType currentAmmoType = GameManager.Instance.currentAmmoType;

        // Check if the player has ammo for the selected type.
        if (GameManager.Instance.GetAmmoCount(currentAmmoType) > 0)
        {
            // Instantiate the correct projectile prefab.
            switch (currentAmmoType)
            {
                case AmmoType.Buckshot:
                    // Code for firing buckshot.
                    break;
                case AmmoType.Rocket:
                    // Code for firing a rocket.
                    break;
                case AmmoType.Default:
                    Instantiate(singleShotPrefab, firePoint.position, firePoint.rotation);
                    break;
            }

            // Deduct the ammo after firing.
            GameManager.Instance.DeductAmmo(currentAmmoType);
        }
    }
}