using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Chainsaw", menuName = "Guns/Chainsaw")]
public class Chainsaw : SOGuns
{
    private BoxCollider2D chainsawCollider;

    public override void ActivateWeapon(Transform weaponOrigin, Vector2 target)
    {
        if (isReloading) return;

        if(chainsawCollider == null)
        {
            chainsawCollider = weaponOrigin.GetComponentInChildren<BoxCollider2D>();
            if (chainsawCollider == null)
            {
                Debug.LogError("Chainsaw is missing its box collider.");
                return;
            }
        }


        chainsawCollider.enabled = true;
    }

    public override void StopFiring(Transform weaponOrigin)
    {
        if (chainsawCollider != null)
            chainsawCollider.enabled = false;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0, whatIsEnemy);
        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log("Hit: " + hit.name);
                enemy.TakeDamage(damage);              // Apply damage

            }
        }
    }

}