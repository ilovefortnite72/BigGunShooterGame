using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = System.Random;

[CreateAssetMenu(fileName = "MachineGun", menuName = "Guns/MachineGun")]
public class MachineGun : SOGuns
{
    public LineRenderer lineRenderer;
    private float firetime;
    public GameObject bulletPrefab;
    public float bulletSpeed;


    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        
        firetime = Time.time;

        Vector2 direction = (target - (Vector2)weaponOrigin.position).normalized;


        FireRaycasts(weaponOrigin, target, whatIsEnemy, range, damage);

        GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, weaponOrigin.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObject);
        BulletBehaviour bulletBeh = bullet.GetComponent<BulletBehaviour>();

        

        if (bulletBeh != null)
        {
            bulletBeh.Initialize(damage, bulletSpeed, direction);
        }

        if(shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, weaponOrigin.position);
        }
    }
}
