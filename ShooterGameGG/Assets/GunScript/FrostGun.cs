using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FrostGun", menuName = "Guns/FrostGun")]
public class FrostGun : SOGuns
{
    public ParticleSystem frostGunEffect;
    public Collider2D frostGunCollider;
    public float SlowIncrement = 0.1f;
    public float MaxSlow = 0.5f;
    public float tickRate = 0.5f;
    public float fuelConsumption = 5f;
    

    private float currentFuel;
    private bool isFiring;
    private bool isRecharing;

    public override void Initialize()
    {
        base.Initialize();
        currentFuel = maxAmmo;
        canHoldTrigger = true;
        usesFuel = true;
    }

    public override void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        if (isReloading || currentAmmo <= 0)
        {
            StopFire(weaponOrigin);
            return;
        }

        isFiring = true;
        frostGunCollider.enabled = true;
        frostGunEffect.Play();
        CoroutineHelper.Instance.StartCoroutine(ConsumeFuel());
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        if (canHoldTrigger)
        {
            ActivateWeapon(weaponOrigin, target);
        }
    }


    public override void StopFire(Transform weaponOrigin)
    {
        if (isFiring)
        {
            isFiring = false;
            frostGunCollider.enabled = false;
            frostGunEffect.Stop();
            CoroutineHelper.Instance.StopAllCoroutines();

            if (!isRecharing)
            {
                CoroutineHelper.Instance.StartCoroutine(RechargeFuel());
            }
        }
    }


    private IEnumerator ConsumeFuel()
    {
        while (isFiring && currentFuel > 0)
        {
            currentFuel -= fuelConsumption * Time.deltaTime;

            if (currentFuel <= 0)
            {
                StopFire(null);
            }
            yield return new WaitForSeconds(tickRate);

        }
    }


    private IEnumerator RechargeFuel()
    {
        isRecharing = true;
        while (!isFiring && currentFuel < maxAmmo)
        {
            currentFuel += fuelConsumption * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0, maxAmmo);
            yield return new WaitForSeconds(tickRate);
        }
        isRecharing = false;
    }
}