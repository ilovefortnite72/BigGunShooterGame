using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "Guns/Shotgun")]
public class Shotgun : SOGuns
{
    public float BulletSpread = 0.45f;
    public float bulletForce = 100f;
    public GameObject ShotgunPelletPrefab;


    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        for (int i = 0; i < 10; i++)
        {
            float bulletSpread = Random.Range(-BulletSpread, BulletSpread);
            Vector2 direction = ((Vector2)weaponOrigin.right + new Vector2(bulletSpread, bulletSpread)).normalized;

            GameObject pellet = Instantiate(ShotgunPelletPrefab, weaponOrigin.position, Quaternion.identity);
            BulletBehaviour bulletBeh = pellet.GetComponent<BulletBehaviour>();
            

            if (bulletBeh != null)
            {
                bulletBeh.Initialize(damage, bulletForce, direction);
            }
        }
    }



}
