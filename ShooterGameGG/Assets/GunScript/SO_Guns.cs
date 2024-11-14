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
    public bool canShoot;
    public bool isAimed;
    public bool isReloading;

    public abstract void ActivateWeapon(Transform weaponOrigin);
}

