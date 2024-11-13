using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SO_Guns : ScriptableObject
{
    public string gunName;
    public float damage;
    public float fireRate;
    public float range;
    public float reloadTime;
    public int maxAmmo;
    public int clipSize;
    public int bulletsPerTap;
    public float spread;
    public float recoil;
    public float recoilRecoverSpeed;
    public float aimSpeed;
    public bool allowButtonHold;
    public bool allowAutoFire;
    public bool canShoot;
    public bool isAimed;
    public bool isReloading;
    

    public abstract void ActivateWeapon(Transform weaponOrigin);
}

