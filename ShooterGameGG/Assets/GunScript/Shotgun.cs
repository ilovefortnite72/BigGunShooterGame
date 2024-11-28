using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Guns/Shotgun")]
public class Shotgun : SOGuns
{
    public float BulletSpread = 0.45f;
    public GameObject ShotgunPelletPrefab;
    public float bulletSpeed;


    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        for (int i = 0; i < 10; i++)
        {
            float bulletSpread = Random.Range(-BulletSpread, BulletSpread);
            Vector2 direction = ((Vector2)weaponOrigin.up + new Vector2(bulletSpread, bulletSpread)).normalized;

            GameObject pellet = ObjectPoolManager.SpawnObject(ShotgunPelletPrefab, weaponOrigin.position, Quaternion.identity, ObjectPoolManager.PoolType.Bullet);
            ShotGunBullet bulletBeh = pellet.GetComponent<ShotGunBullet>();
            bulletBeh.Initialize(bulletSpeed, direction);
        }
    }

}
