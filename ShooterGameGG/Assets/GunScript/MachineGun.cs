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
        damage = 10f;
    }

    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        if (Time.time - firetime > fireRate)
        {
            firetime = Time.time;
            FireRaycasts(weaponOrigin);
            Debug.Log("Fired");
        }
    }



    private void FireRaycasts(Transform weaponOrigin)
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            Debug.DrawRay(weaponOrigin.position, weaponOrigin.forward * range, Color.red, 0.1f);
            if (Physics.Raycast(weaponOrigin.position, weaponOrigin.forward, out hit, range, whatIsEnemy))
            {
                // Deal damage to the enemy
                Debug.Log("Hit: " + hit.collider.name);

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
