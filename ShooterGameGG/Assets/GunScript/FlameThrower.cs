using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrower : SOGuns
{
    public float fuelConsumptionRate = 10f; // Fuel consumed per second
    public float damagePerTick = 5f;       // Damage dealt per tick
    public float fuelRechargeRate = 5f;    // Fuel recharged per second
    public float tickRate = 0.5f;          // Interval between damage/slow applications

    private ParticleSystem flameParticles;
    private BoxCollider2D flameCollider;
    private bool isRecharging = true;
    private float tickTimer; // Timer to track ticks

    public override void Initialize()
    {
        base.Initialize();
        isRecharging = true; // Enable recharge on start
        tickTimer = 0f;      // Initialize tick timer
    }

    public override void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        if (currentAmmo <= 0 || isReloading)
        {
            StopFiring(weaponOrigin);
            return;
        }

        // Stop recharging when firing starts
        isRecharging = false;

        // Find or create the flame components (Particles and Collider)
        if (flameParticles == null || flameCollider == null)
        {
            flameParticles = weaponOrigin.GetComponentInChildren<ParticleSystem>();
            flameCollider = weaponOrigin.GetComponentInChildren<BoxCollider2D>();

            if (flameParticles == null || flameCollider == null)
            {
                Debug.LogError("Flamethrower is missing its particle system or box collider.");
                return;
            }
        }

        flameParticles.Play(); // Start particle effects
        flameCollider.enabled = true; // Enable damage detection

        // Continuously reduce fuel while firing
        CoroutineHelper.Instance.StartCoroutine(ConsumeFuel(weaponOrigin));
    }

    public override void StopFiring(Transform weaponOrigin)
    {
        // Stop particles and disable the collider
        if (flameParticles != null)
            flameParticles.Stop();
        if (flameCollider != null)
            flameCollider.enabled = false;

        // Start recharging fuel
        isRecharging = true;
        CoroutineHelper.Instance.StartCoroutine(RechargeFuel());
    }

    private IEnumerator ConsumeFuel(Transform weaponOrigin)
    {
        while (currentAmmo > 0)
        {
            currentAmmo -= Mathf.RoundToInt(fuelConsumptionRate * Time.deltaTime);
            if (currentAmmo <= 0)
            {
                StopFiring(weaponOrigin);
                yield break;
            }

            // Apply damage and slow at fixed intervals
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickRate)
            {
                tickTimer = 0f; // Reset timer
                ApplyEffects(flameCollider);
            }

            yield return null; // Wait for the next frame
        }
    }

    private void ApplyEffects(BoxCollider2D collider)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0, whatIsEnemy);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerTick);              // Apply damage
                
            }
        }
    }

    private IEnumerator RechargeFuel()
    {
        while (isRecharging && currentAmmo < maxAmmo)
        {
            currentAmmo += Mathf.RoundToInt(fuelRechargeRate * Time.deltaTime);

            if (currentAmmo >= maxAmmo)
            {
                currentAmmo = maxAmmo; // Clamp to max ammo
                isRecharging = false;  // Stop recharging if full
            }

            yield return null; // Wait for the next frame
        }
    }
}
