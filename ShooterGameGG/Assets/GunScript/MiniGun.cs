using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGun", menuName = "Guns/MiniGun")]
public class MiniGun : SOGuns
{
    private float overHeatLevel = 0f;
    public float overHeatThreshold = 100f;
    public float overHeatIncrease = 5f;
    public float overHeatCooldown = 2f;
    private bool isFiring = false;
    private float holdFireTime = 0f; // Timer to track hold time
    public float fireDelay = 2f; // Time to hold before firing
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float overHeatedTime = 3f;

    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true;
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        // Accumulate time while the fire button is held
        if (isFiring)
        {
            holdFireTime += Time.deltaTime;

            // Once the fire button is held for 2 seconds, start firing
            if (holdFireTime >= fireDelay && overHeatLevel < overHeatThreshold)
            {
                if (Time.time >= nextTimeToFire && currentAmmo > 0)
                {
                    ActivateWeapon(weaponOrigin, target);
                    overHeatLevel += overHeatIncrease * Time.deltaTime;
                    nextTimeToFire = Time.time + (1f / fireRate); // Maintain fire rate timing
                }
            }
        }
        else
        {
            // Reset fire delay timer when starting to hold fire
            holdFireTime = 0f;
        }

        // Stop firing if overheated
        if (overHeatLevel >= overHeatThreshold)
        {
            isFiring = false;
            CoroutineHelper.Instance.StartCoroutine(OverheatedCooldown());
        }

        // Handle cooldown when not firing
        Cooldown();
    }

    private IEnumerator FiringDelay(Transform weaponOrigin, Vector2 target)
    {
        // Add a small delay before starting to fire, mimicking the time it takes to start up the gun
        yield return new WaitForSeconds(0.2f); // Adjust the delay time as necessary

        while (isFiring && overHeatLevel < overHeatThreshold)
        {
            ActivateWeapon(weaponOrigin, target);
            overHeatLevel += overHeatIncrease * Time.deltaTime;
            yield return null;
        }

        // Once the overheat threshold is reached, stop firing and initiate cooldown
        if (overHeatLevel >= overHeatThreshold)
        {
            isFiring = false;
            yield return new WaitForSeconds(overHeatedTime); // Wait for the overheating cooldown to finish
            overHeatLevel = 0f; // Reset the overheat level
        }
    }

    public void Cooldown()
    {
        // Gradually decrease the overheat level when not firing
        if (!isFiring && overHeatLevel > 0)
        {
            overHeatLevel -= overHeatCooldown * Time.deltaTime;
        }
    }

    private IEnumerator OverheatedCooldown()
    {
        // Wait for a while (overheated time), then reset overheat level and allow firing again
        yield return new WaitForSeconds(overHeatedTime);
        overHeatLevel = 0f;
        isFiring = false;  // Ensure firing is stopped
    }

    public override void StopFiring(Transform weaponOrigin)
    {
        // Reset the firing and hold time when stopping fire
        isFiring = false;
        holdFireTime = 0f;
    }

    // Override ActivateWeapon to add the particle system effect when firing
    public override void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        base.ActivateWeapon(weaponOrigin, target);

        Vector2 direction = (target - (Vector2)weaponOrigin.position).normalized;
        GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, weaponOrigin.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        BulletBehaviour bulletBeh = bullet.GetComponent<BulletBehaviour>();
        if (bulletBeh != null)
        {
            bulletBeh.Initialize(bulletSpeed, direction);
        }

        // Consume ammo
        if (!usesFuel)
        {
            currentAmmo--;
            if (currentAmmo <= 0)
            {
                Reload();
            }
        }
    }
}