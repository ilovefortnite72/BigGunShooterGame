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
        for (int i = 0; i <10; i++)
        {
            float bulletSpread = Random.Range(-BulletSpread, BulletSpread);
            Vector3 direction = weaponOrigin.right * Mathf.Cos(bulletSpread);
            Instantiate(ShotgunPelletPrefab, weaponOrigin.position, Quaternion.LookRotation(direction));
            ShotgunPelletPrefab.transform.right = direction;

            //set the damage and force of the bullet
            BulletBehaviour bulletBeh = ShotgunPelletPrefab.GetComponent<BulletBehaviour>();
            if (bulletBeh != null)
            {
                bulletBeh.SetDamage(damage);
            }

            Rigidbody2D rb = ShotgunPelletPrefab.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            }
        }
    }



}
