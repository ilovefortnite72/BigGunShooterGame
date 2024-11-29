using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FrostGun", menuName = "Guns/FrostGun")]
public class FrostGun: SOGuns
{
    public float fuelConsumptionRate = 10f; 
    public float damagePerTick = 5f;       
    public float fuelRechargeRate = 5f;    
    public float slowAmount = 0.3f;        
    public float slowDuration = 2f;        
    public float maxSlowEffect = 0.5f;     
    public float tickRate = 0.5f;          

    private ParticleSystem iceParticles;
    private BoxCollider2D iceCollider;
    private bool isRecharging = true;
    private float tickTimer; 

    public override void Initialize()
    {
        base.Initialize();
        isRecharging = true; 
        tickTimer = 0f; 
    }

    public override void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        if (currentAmmo <= 0 || isReloading)
        {
            StopFiring(weaponOrigin);
            return;
        }

        
        isRecharging = false;

        if (iceParticles == null || iceCollider == null)
        {
            iceParticles = weaponOrigin.GetComponentInChildren<ParticleSystem>();
            iceCollider = weaponOrigin.GetComponentInChildren<BoxCollider2D>();

            if (iceParticles == null || iceCollider == null)
            { 
                return;
            }
        }

        iceParticles.Play(); 
        iceCollider.enabled = true; 


        CoroutineHelper.Instance.StartCoroutine(ConsumeFuel(weaponOrigin));
    }

    public override void StopFiring(Transform weaponOrigin)
    {

        if (iceParticles != null)
            iceParticles.Stop();
        if (iceCollider != null)
            iceCollider.enabled = false;


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
                ApplyEffects(iceCollider);
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
                enemy.SlowEffect(slowAmount, maxSlowEffect);  // Apply slow effect
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