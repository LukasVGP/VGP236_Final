using UnityEngine;

public class Blunderbuss : MonoBehaviour
{
    // === Public Variables for Inspector Setup ===
    public Transform firePoint;
    public GameObject standardProjectilePrefab;
    public GameObject buckshotProjectilePrefab;
    public GameObject explodingProjectilePrefab;
    public float fireRate = 0.5f;

    // === Private Variables ===
    private float nextFireTime = 0f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // Check which ammo type to fire based on player input or a selected state.
        AmmunitionType ammoType = AmmunitionType.Standard;
        if (GameManager.Instance.HasAmmo(ammoType))
        {
            // Instantiate the correct projectile prefab and deduct ammo.
            GameObject projectilePrefab = GetProjectilePrefab(ammoType);
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            GameManager.Instance.DeductAmmo(ammoType, 1);

            // Set the next fire time.
            nextFireTime = Time.time + fireRate;
        }
    }

    private GameObject GetProjectilePrefab(AmmunitionType type)
    {
        // Return the correct prefab based on the ammo type.
        switch (type)
        {
            case AmmunitionType.Standard:
                return standardProjectilePrefab;
            case AmmunitionType.Buckshot:
                return buckshotProjectilePrefab;
            case AmmunitionType.Exploding:
                return explodingProjectilePrefab;
            default:
                return standardProjectilePrefab;
        }
    }
}

public enum AmmunitionType
{
    Standard,
    Buckshot,
    Exploding
}
