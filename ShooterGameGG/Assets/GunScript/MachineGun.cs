using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = System.Random;

[CreateAssetMenu(fileName = "MachineGun", menuName = "Guns/MachineGun")]
public class MachineGun : SOGuns
{
    public LayerMask whatIsEnemy;
    public LineRenderer lineRenderer;
    private float firetime;


    private void Awake()
    {
        fireRate = 0.1f;
        range = 100f;
        damage = 1f;
    }

    public override void ActivateWeapon(Transform WeaponOrigin)
    {
        if (canShoot && !isReloading)
        {
            WeaponOrigin.GetComponent<MonoBehaviour>().StartCoroutine(Fire(WeaponOrigin));
        }
    }


    IEnumerator Fire(Transform WeaponOrigin)
    {
        while (Input.GetKey("0"))
        {
            FireRaycasts(WeaponOrigin);
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void FireRaycasts(Transform weaponOrigin)
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(weaponOrigin.position, weaponOrigin.forward, out hit, range, whatIsEnemy))
            {
                // Deal damage to the enemy
                var enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }

                // Create visual effect
                ShowBulletEffect(weaponOrigin.position, hit.point);
            }
        }
    }



    IEnumerator ShowBulletEffect(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.05f);

        lineRenderer.enabled = false;
    }
}
