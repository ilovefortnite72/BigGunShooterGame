using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "Chainsaw", menuName = "Guns/Chainsaw")]
public class Chainsaw : SOGuns
{
    public GameObject chainsawEffectPrefab;
    private GameObject activeEffect;

    public float DamageTickRate = 0.5f;
    private bool isAttacking = false;
    private Collider2D Col;

    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true;
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        if (!isAttacking)
        {
            isAttacking = true;

            Col = weaponOrigin.GetComponentInChildren<Collider2D>();
            if (Col != null)
            {
                Col.enabled = true;
            }

            if (chainsawEffectPrefab != null && activeEffect != null)
            {
                activeEffect = Instantiate(chainsawEffectPrefab, weaponOrigin.position, Quaternion.identity, weaponOrigin);
            }

            CoroutineHelper.Instance.StartCoroutine(DamageOverTime());
        }
    }

    public override void StopFire(Transform weaponOrigin)
    {
        isAttacking = false;
        if (Col != null)
        {
            Col.enabled = false;
        }

        if (activeEffect != null)
        {
            Destroy(activeEffect);
            activeEffect = null;
        }
    }


    private IEnumerator DamageOverTime()
    {
        while (isAttacking)
        {
            DamageEnemiesInRange();
            yield return new WaitForSeconds(DamageTickRate);
        }
    }

    private void DamageEnemiesInRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(
            Col.bounds.center,
            Col.bounds.size,
            0,
            whatIsEnemy
            );

        foreach (Collider2D enemy in hitColliders)
        {
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);


                if (enemyController.isDead == true)
                {
                    playDeathEffect(enemyController.transform.position);
                }
            }
        }
    }

    private void playDeathEffect(Vector3 position)
    {
        throw new NotImplementedException();
    }
}
