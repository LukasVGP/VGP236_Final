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
                // Instantiate a buckshot projectile.
                // You might want to spawn multiple projectiles in a cone for a real buckshot effect.
                Instantiate(buckshotPrefab, firePoint.position, firePoint.rotation);
                break;
            case AmmoType.Rocket:
                // Instantiate a rocket projectile.
                Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
                break;
            case AmmoType.Default:
                // Instantiate a single shot projectile.
                Instantiate(singleShotPrefab, firePoint.position, firePoint.rotation);
                break;
        }

        // Deduct the ammo after firing.
        GameManager.Instance.DeductAmmo(currentAmmoType);
    }
}