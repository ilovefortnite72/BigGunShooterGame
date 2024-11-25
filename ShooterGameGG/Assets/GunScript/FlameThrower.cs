using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrower : SOGuns
{
    //half cooked script good luck


    public float tickRate;
    public ParticleSystem ParticleSystem;
    public float lastDamageTime;
    private Collider2D col;
    public float fuelConsumptionRate;

    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true;
        usesFuel = true;
    }

    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        if(currentAmmo <= 0)
        {
            StopFire(weaponOrigin);
            return;
        }

        if (col == null)
            col = weaponOrigin.GetComponentInChildren<Collider2D>();
        if (ParticleSystem == null)
            ParticleSystem = weaponOrigin.GetComponentInChildren<ParticleSystem>();

        if(!ParticleSystem.isPlaying)
        {
            ParticleSystem.Play();
        }

        col.enabled = true;
    }




    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        if (currentAmmo > 0)
        {

            Fire(weaponOrigin, target);

            if (Time.time >= lastDamageTime)
            {
                DealFireDamage();
                lastDamageTime = Time.time + tickRate;

            }


            //currentAmmo -= fuelConsumptionRate * Time.deltaTime;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        }
        else
        {
            StopFire(weaponOrigin);
        }
    }


    public override void StopFire(Transform weaponOrigin)
    {
        if(ParticleSystem != null && ParticleSystem.isPlaying)
        {
            ParticleSystem.Stop();
        }

        if (col != null)
        {
            col.enabled = false;
        }
    }


    private void DealFireDamage()
    {
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
