using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Chainsaw", menuName = "Guns/Chainsaw")]
public class Chainsaw : SOGuns
{
    public GameObject chainsawEffectPrefab; // Prefab for chainsaw effect
    private GameObject activeEffect; // Instance of the effect
    private bool isAttacking = false; // Is the chainsaw currently attacking
    private Collider2D col; // Collider for detecting enemies in range

    public float DamageTickRate = 0.5f; // How often the chainsaw deals damage
    public float damageDuration = 2f; // How long the chainsaw can attack before stopping

    public override void Initialize()
    {
        base.Initialize();
        canHoldTrigger = true; // Allow continuous fire while holding the trigger
    }

    public override void HoldFire(Transform weaponOrigin, Vector2 target)
    {
        if (!isAttacking)
        {
            isAttacking = true;

            // Find the collider attached to the chainsaw (child object)
            col = weaponOrigin.GetComponentInChildren<Collider2D>();
            if (col != null)
            {
                col.enabled = true; // Enable the collider to detect enemies
            }

            // Instantiate the chainsaw effect (e.g., sparks or saw rotation)
            if (chainsawEffectPrefab != null && activeEffect == null)
            {
                activeEffect = Instantiate(chainsawEffectPrefab, weaponOrigin.position, Quaternion.identity, weaponOrigin);
            }

            // Start the damage-over-time coroutine
            CoroutineHelper.Instance.StartCoroutine(DamageOverTime());
        }
    }

    public void StopFire(Transform weaponOrigin)
    {
        // Stop the attack
        isAttacking = false;

        // Disable the collider when stopping the attack
        if (col != null)
        {
            col.enabled = false;
        }

        // Destroy the chainsaw effect if it's active
        if (activeEffect != null)
        {
            Destroy(activeEffect);
            activeEffect = null;
        }
    }

    private IEnumerator DamageOverTime()
    {
        float attackTime = 0f;

        // Continuously apply damage while the chainsaw is attacking
        while (isAttacking && attackTime < damageDuration)
        {
            DamageEnemiesInRange();
            attackTime += DamageTickRate;
            yield return new WaitForSeconds(DamageTickRate); // Wait before applying damage again
        }

        // Once the attack duration is over, stop firing
        StopFire(null);
    }

    private void DamageEnemiesInRange()
    {
        // Use OverlapBox to detect enemies within the collider's bounds
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(
            col.bounds.center,
            col.bounds.size,
            0f, // No rotation
            whatIsEnemy
        );

        foreach (Collider2D enemy in hitColliders)
        {
            // Apply damage to each enemy in range
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage);

                // Play death effect if the enemy is dead (can be customized)
                if (enemyController.isDead)
                {
                    PlayDeathEffect(enemyController.transform.position);
                }
            }
        }
    }

    private void PlayDeathEffect(Vector3 position)
    {
        // Implement a visual effect or sound here when an enemy dies (optional)
        Debug.Log("Playing death effect at: " + position);
    }
}