using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Guns/Shotgun")]
public class Shotgun : SOGuns
{
    public float BulletSpread = 0.45f;
    public GameObject ShotgunPelletPrefab;


    public override void ActivateWeapon(Transform transform)
    {
        for (int i = 0; i <10; i++)
        {
            float bulletSpread = Random.Range(-BulletSpread, BulletSpread);
            //Vector3 direction = weaponOrigin.right * Mathf.Cos(bulletSpread);
            //Instantiate(ShotgunPelletPrefab, weaponOrigin.position, Quaternion.LookRotation(direction));
        }
    }



}
