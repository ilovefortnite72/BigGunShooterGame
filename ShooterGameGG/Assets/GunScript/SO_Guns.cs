using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Ranged, Projectile }
public abstract class SOGuns : ScriptableObject
{
    public WeaponType weaponType;
    public Sprite gunSprite;
    public GameObject gunPrefab;
    public string gunName;
    public float damage;
    public float fireRate;
    public float range;
    public float reloadTime;
    public int currentAmmo;
    public int maxAmmo;
    public float spread;
    public bool isReloading;
    public AudioClip reloadSound;

    public virtual void Initialize()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public virtual void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        if (isReloading || currentAmmo <= 0)
        {
            Debug.Log("Reloading or out of ammo");
        }

        Fire(weaponOrigin, target);
        currentAmmo--;

        if (currentAmmo <= 0)
        {
            Reload();
        }

    }

    public abstract void Fire(Transform weaponOrigin, Vector2 target);
    

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

