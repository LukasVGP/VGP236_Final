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

    // --- New variables for recoil and sound ---
    public Transform gunBarrelTransform;
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 20f;
    private Vector3 initialBarrelPosition;
    private AudioSource audioSource;
    // ------------------------------------------

    // References to other components.
    private Coroutine fireCoroutine;

    // --- New Start method to initialize variables ---
    void Start()
    {
        // Cache the initial position of the barrel for recoil reset.
        if (gunBarrelTransform != null)
        {
            initialBarrelPosition = gunBarrelTransform.localPosition;
        }

        // Get the AudioSource component on the same GameObject.
        audioSource = GetComponent<AudioSource>();
    }
    // --------------------------------------------------

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
            // --- Play sound and start recoil before firing ---
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            StartCoroutine(RecoilRoutine());
            // ----------------------------------------------------

            // Instantiate the correct projectile prefab.
            switch (currentAmmoType)
            {
                case AmmoType.Buckshot:
                    // Code for firing buckshot.
                    if (buckshotPrefab != null)
                    {
                        Instantiate(buckshotPrefab, firePoint.position, firePoint.rotation);
                    }
                    break;
                case AmmoType.Rocket:
                    // Code for firing a rocket.
                    if (rocketPrefab != null)
                    {
                        Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
                    }
                    break;
                case AmmoType.Default:
                    // Instantiate a single shot projectile.
                    if (singleShotPrefab != null)
                    {
                        Instantiate(singleShotPrefab, firePoint.position, firePoint.rotation);
                    }
                    break;
            }

            // Deduct the ammo after firing.
            GameManager.Instance.DeductAmmo(currentAmmoType);
        }
    }

    // --- New Coroutine for Recoil ---
    private IEnumerator RecoilRoutine()
    {
        Vector3 recoilPosition = initialBarrelPosition - new Vector3(recoilDistance, 0, 0);

        // Move the barrel back.
        while (Vector3.Distance(gunBarrelTransform.localPosition, recoilPosition) > 0.01f)
        {
            gunBarrelTransform.localPosition = Vector3.MoveTowards(gunBarrelTransform.localPosition, recoilPosition, recoilSpeed * Time.deltaTime);
            yield return null;
        }

        // Move the barrel forward.
        while (Vector3.Distance(gunBarrelTransform.localPosition, initialBarrelPosition) > 0.01f)
        {
            gunBarrelTransform.localPosition = Vector3.MoveTowards(gunBarrelTransform.localPosition, initialBarrelPosition, recoilSpeed * Time.deltaTime);
            yield return null;
        }

        gunBarrelTransform.localPosition = initialBarrelPosition;
    }
    // ------------------------------------
}