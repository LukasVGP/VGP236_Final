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

    // --- Variables for recoil and sound ---
    public Transform gunBarrelTransform;
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 20f;
    private Vector3 initialBarrelPosition;
    private AudioSource audioSource;
    // ------------------------------------------

    // --- Variables for different muzzle flashes ---
    public GameObject defaultMuzzleFlashPrefab;
    public GameObject buckshotMuzzleFlashPrefab;
    public GameObject rocketMuzzleFlashPrefab;
    public float muzzleFlashDuration = 0.1f;
    // ------------------------------------------

    // References to other components.
    private Coroutine fireCoroutine;

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
        AmmoType currentAmmoType = GameManager.Instance.currentAmmoType;

        if (GameManager.Instance.GetAmmoCount(currentAmmoType) > 0)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            StartCoroutine(RecoilRoutine());

            // Instantiate the correct projectile and muzzle flash based on ammo type.
            switch (currentAmmoType)
            {
                case AmmoType.Buckshot:
                    if (buckshotPrefab != null)
                    {
                        Instantiate(buckshotPrefab, firePoint.position, firePoint.rotation);
                        if (buckshotMuzzleFlashPrefab != null)
                        {
                            StartCoroutine(MuzzleFlashRoutine(buckshotMuzzleFlashPrefab));
                        }
                    }
                    break;
                case AmmoType.Rocket:
                    if (rocketPrefab != null)
                    {
                        Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
                        if (rocketMuzzleFlashPrefab != null)
                        {
                            StartCoroutine(MuzzleFlashRoutine(rocketMuzzleFlashPrefab));
                        }
                    }
                    break;
                case AmmoType.Default:
                    if (singleShotPrefab != null)
                    {
                        Instantiate(singleShotPrefab, firePoint.position, firePoint.rotation);
                        if (defaultMuzzleFlashPrefab != null)
                        {
                            StartCoroutine(MuzzleFlashRoutine(defaultMuzzleFlashPrefab));
                        }
                    }
                    break;
            }

            GameManager.Instance.DeductAmmo(currentAmmoType);
        }
    }

    // Coroutine for Recoil.
    private IEnumerator RecoilRoutine()
    {
        if (gunBarrelTransform == null) yield break;

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

    // Coroutine for Muzzle Flash.
    private IEnumerator MuzzleFlashRoutine(GameObject muzzleFlashPrefabToUse)
    {
        if (muzzleFlashPrefabToUse != null && firePoint != null)
        {
            // Instantiate the muzzle flash as a child of the fire point.
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefabToUse, firePoint.position, firePoint.rotation, firePoint);
            // Wait for the specified duration.
            yield return new WaitForSeconds(muzzleFlashDuration);
            // Destroy the muzzle flash GameObject.
            Destroy(muzzleFlash);
        }
    }
}