using System.Collections;
using UnityEngine;

// Blunderbuss.cs: Manages the player's weapon and ammunition types.
public class Blunderbuss : MonoBehaviour
{
    // Public variables set in the Unity Inspector.
    // --- Projectile Prefabs ---
    public GameObject singleShotPrefab;
    public GameObject buckshotPrefab;
    public GameObject rocketPrefab;

    // --- Firing Properties ---
    public Transform firePoint;
    public float fireRate = 0.5f;

    // --- Variables for recoil and sound ---
    [Header("Recoil & Audio")]
    public Transform gunBarrelTransform;
    public float recoilDistance = 0.2f;
    public float recoilSpeed = 20f;
    private Vector3 initialBarrelPosition;
    private AudioSource audioSource;
    // ------------------------------------------

    // --- Variables for different muzzle flashes ---
    [Header("Muzzle Flash")]
    public GameObject defaultMuzzleFlashPrefab;
    public GameObject buckshotMuzzleFlashPrefab;
    public GameObject rocketMuzzleFlashPrefab;
    public float muzzleFlashDuration = 0.1f;
    // ------------------------------------------

    // References to other components.
    private Coroutine fireCoroutine;

    void Start()
    {
        if (gunBarrelTransform != null)
        {
            initialBarrelPosition = gunBarrelTransform.localPosition;
        }

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

            GameObject projectilePrefabToUse = null;
            GameObject muzzleFlashPrefabToUse = null;

            switch (currentAmmoType)
            {
                case AmmoType.Buckshot:
                    projectilePrefabToUse = buckshotPrefab;
                    muzzleFlashPrefabToUse = buckshotMuzzleFlashPrefab;
                    break;
                case AmmoType.Rocket:
                    projectilePrefabToUse = rocketPrefab;
                    muzzleFlashPrefabToUse = rocketMuzzleFlashPrefab;
                    break;
                case AmmoType.Default:
                    projectilePrefabToUse = singleShotPrefab;
                    muzzleFlashPrefabToUse = defaultMuzzleFlashPrefab;
                    break;
            }

            if (projectilePrefabToUse != null)
            {
                Instantiate(projectilePrefabToUse, firePoint.position, firePoint.rotation);
            }

            if (muzzleFlashPrefabToUse != null)
            {
                StartCoroutine(MuzzleFlashRoutine(muzzleFlashPrefabToUse));
            }

            GameManager.Instance.DeductAmmo(currentAmmoType);
        }
    }

    // Coroutine for Recoil.
    private IEnumerator RecoilRoutine()
    {
        if (gunBarrelTransform == null) yield break;

        Vector3 recoilPosition = initialBarrelPosition - new Vector3(recoilDistance, 0, 0);

        while (Vector3.Distance(gunBarrelTransform.localPosition, recoilPosition) > 0.01f)
        {
            gunBarrelTransform.localPosition = Vector3.MoveTowards(gunBarrelTransform.localPosition, recoilPosition, recoilSpeed * Time.deltaTime);
            yield return null;
        }

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
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefabToUse, firePoint.position, firePoint.rotation, firePoint);
            yield return new WaitForSeconds(muzzleFlashDuration);
            Destroy(muzzleFlash);
        }
    }
}