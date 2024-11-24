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


    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true;
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        if (!isFiring && !isReloading && Time.time >= nextTimeToFire)
        {
            isFiring = true;
            CoroutineHelper.Instance.StartCoroutine(FiringDelay(weaponOrigin, target));

        }
    }

    public override void StopFire(Transform weaponOrigin)
    {
        isFiring = false;
    }

    private IEnumerator FiringDelay(Transform weaponOrigin, Vector2 target)
    {
        yield return new WaitForSeconds(2f);

        while (isFiring && overHeatLevel < overHeatThreshold)
        {
            ActivateWeapon(weaponOrigin, target);
            overHeatLevel += overHeatIncrease;
            yield return null;
        }

        if (overHeatLevel >= overHeatThreshold)
        {
            isFiring = false;
            yield return new WaitForSeconds(overHeatedTime);
            overHeatLevel = 0f;
        }
    }

    public void Cooldown()
    {
        if (!isFiring && overHeatLevel > 0)
        {
            overHeatLevel -= overHeatCooldown * Time.deltaTime;
        }
    }

    
}
