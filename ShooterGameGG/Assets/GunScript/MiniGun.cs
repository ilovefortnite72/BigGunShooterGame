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
    public float overHeatedTime = 2f;

    public ParticleSystem firingParticleSystem;  // Reference to the particle system

    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true;
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        // Start firing if it's not already firing, not reloading, and we have ammo
        if (!isFiring && !isReloading && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            isFiring = true;
            CoroutineHelper.Instance.StartCoroutine(FiringDelay(weaponOrigin, target));
        }
        // If already firing, continue firing and apply overheating
        else if (isFiring && overHeatLevel < overHeatThreshold)
        {
            // Continuously fire while holding the trigger and handle overheating
            if (Time.time >= nextTimeToFire && currentAmmo > 0)
            {
                ActivateWeapon(weaponOrigin, target);
                overHeatLevel += overHeatIncrease * Time.deltaTime;  // Gradual increase in overheat
                nextTimeToFire = Time.time + (1f / fireRate); // Maintain fire rate timing
            }
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

    public void StopFire(Transform weaponOrigin)
    {
        // Stop the particle system when the fire stops
        if (firingParticleSystem != null && firingParticleSystem.isPlaying)
        {
            firingParticleSystem.Stop();
        }
    }

    // Override ActivateWeapon to add the particle system effect when firing
    public override void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        base.ActivateWeapon(weaponOrigin, target);

        // If the particle system is set up, play it when firing
        if (firingParticleSystem != null && !firingParticleSystem.isPlaying)
        {
            firingParticleSystem.Play();
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