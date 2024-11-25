using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType { Melee, Ranged, Projectile }
public abstract class SOGuns : ScriptableObject
{
    public WeaponType weaponType;
    public Sprite gunSprite;
    public GameObject gunPrefab;
    public LayerMask whatIsEnemy;
    public string gunName;
    public float damage;
    public float fireRate;
    public float range;
    public float reloadTime;
    public int currentAmmo;
    public int maxAmmo;
    public float spread;
    public bool isReloading;
    public float nextTimeToFire;
    public bool canHoldTrigger;
    public bool usesFuel;
    public AudioClip reloadSound;
    public AudioClip shootSound;
    public AudioClip emptyMagClip;

    

    public virtual void Initialize()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        nextTimeToFire = 0f;
        
        
    }

    public virtual void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        if (isReloading || currentAmmo <= 0)
        {
            Debug.Log("Reloading or out of ammo");
            if (emptyMagClip != null)
            {
                AudioSource.PlayClipAtPoint(emptyMagClip, Camera.main.transform.position);
            }
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            Fire(weaponOrigin, target);
            Debug.Log("Firing");

            if (!usesFuel)
            {
                currentAmmo--;
                

                if (currentAmmo <= 0)
                {
                    Reload();
                }
            }
            nextTimeToFire = Time.time + (1f / fireRate);
        }
                
    }

    public virtual void HoldFire(Transform weaponOrigin, Vector2 target)
    {

        if (canHoldTrigger)
        {
            ActivateWeapon(weaponOrigin, target);
        }
    }

    public virtual void StopFire(Transform weaponOrigin)
    {
        
    }

    public virtual void Fire(Transform weaponOrigin, Vector2 target)
    {
        FireRaycasts(weaponOrigin, target, whatIsEnemy, range, damage);
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
        }
    }


    protected void FireRaycasts(Transform weaponOrigin, Vector2 target, LayerMask whatIsEnemy, float range, float damage)
    {
        

        RaycastHit2D hit;
        Vector2 direction = (target - (Vector2)weaponOrigin.position).normalized;
        hit = Physics2D.Raycast(weaponOrigin.position, direction, range, whatIsEnemy);

        Debug.DrawRay(weaponOrigin.position, direction * range, Color.green, 0.5f);
        if (hit.collider != null)
        {
            // Deal damage to the enemy
            Debug.Log("Hit: " + hit.collider.name);
            var enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

        }
    }


    

    public virtual void Reload()
    {
        Debug.Log("Reloading");
        isReloading = true;
        CoroutineHelper.Instance.StartCoroutine(ReloadCoroutine());
        AudioSource.PlayClipAtPoint(reloadSound, Camera.main.transform.position);
    }


    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded");
    }


}

