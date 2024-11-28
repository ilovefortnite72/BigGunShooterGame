using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrower : SOGuns
{
    public float tickRate;
    public ParticleSystem particleSystem;  // Note: Variable renamed to match proper casing
    public float lastDamageTime;
    public Collider2D col;
    public float fuelConsumptionRate;

    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true;
        usesFuel = true;
        if (particleSystem != null)
        {
            particleSystem.Stop(); // Ensure the particle system is stopped when the weapon is initialized
        }
        
    }

    

    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        if (currentAmmo <= 0)
        {
            StopFire(weaponOrigin); // Stop fire if out of ammo
            return;
        }

        // Ensure we have a valid collider and particle system references
        if (col == null)
        {
            col = weaponOrigin.GetComponent<Collider2D>();
        }

        if (particleSystem == null)
        {
            particleSystem = weaponOrigin.GetComponent<ParticleSystem>();  // Get the particle system from a child if it's not directly attached
        }

        // Start the particle system if it's not already playing
        if (particleSystem != null && !particleSystem.isPlaying)
        {
            particleSystem.Play();
        }

        col.enabled = true;
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        if (currentAmmo > 0)
        {
            Fire(weaponOrigin, target);

            // If the time is right, apply fire damage
            if (Time.time >= lastDamageTime)
            {
                DealFireDamage();
                lastDamageTime = Time.time + tickRate;
            }

            // Reduce fuel over time (if needed, adjust accordingly)
            currentAmmo -= Mathf.FloorToInt(fuelConsumptionRate * Time.deltaTime);
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        }
        else
        {
            StopFire(weaponOrigin); // Stop the fire if out of ammo
        }
    }

    public void StopFire(Transform weaponOrigin)
    {
        // Stop the particle system if it's playing
        if (particleSystem != null && particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }

        // Disable the collider when not firing
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void DealFireDamage()
    {
        if (col == null) return;

        // Get enemies in the flame's area and apply damage
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.size,
            0f,
            whatIsEnemy
        );

        foreach (var enemy in hitEnemies)
        {
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);
            }
        }
    }
}