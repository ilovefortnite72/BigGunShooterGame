using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SO_Guns equippedWeapon;
    public WeaponObject WeaponObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            equippedWeapon.ActivateWeapon(transform);
        }
    }


    public void EquipWeapon(SO_Guns NewWeapon)
    {
        if (NewWeapon is MachineGun)
        {
            equippedWeapon = NewWeapon;
           // WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
            //WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
            //WeaponObject.weaponType = NewWeapon.weaponType;
        }
        else if (NewWeapon is Shotgun)

        {
            equippedWeapon = NewWeapon;
            //WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
            //WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
            //WeaponObject.weaponType = NewWeapon.weaponType;
        }
        //else if (NewWeapon is Sniper)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
        //    WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
        //    WeaponObject.weaponType = NewWeapon.weaponType;
        //}
        //else if (NewWeapon is Pistol)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
         //   WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
        //    WeaponObject.weaponType = NewWeapon.weaponType;
        //}
        //else if (NewWeapon is RocketLauncher)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
        //    WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
        //    WeaponObject.weaponType = NewWeapon.weaponType;
        }
        //else if (NewWeapon is GrenadeLauncher)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
        //    WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
        //    WeaponObject.weaponType = NewWeapon.weaponType;
        //}
        //else if (NewWeapon is Flamethrower)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
        //    WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
        //    WeaponObject.weaponType = NewWeapon.weaponType;
        //}
        //else if (NewWeapon is LaserGun)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
        //    WeaponObject.WeaponCollider = NewWeapon.WeaponCollider;
        //    WeaponObject.weaponType = NewWeapon.weaponType;
        //}
        //else if (NewWeapon is RailGun)
        //{
        //    equippedWeapon = NewWeapon;
        //    WeaponObject.WeaponSprite = NewWeapon.WeaponSprite;
        //    WeaponObject.WeaponCollider = NewWeapon.We
       // }
    }
}
