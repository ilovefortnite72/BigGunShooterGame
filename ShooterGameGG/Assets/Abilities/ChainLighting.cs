using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChainLightingAbility", menuName = "Abilities/ChainLighting")]
public class ChainLighting : SOAbilities
{
    [Header("Chain Lightning Properties")]
    public int maxChains = 5;
    public float damageReductionPerChain = 0.15f;
    public LayerMask whatIsEnemy;
    public GameObject chainLightningEffect;

    // Implementing the abstract method from SOAbilities
    protected override void UseAbility(Transform player)
    {
        Debug.Log("Chain Lightning Ability Activated");
        CoroutineHelper.Instance.StartCoroutine(ChainLightningEffect(player.position, maxChains, damage));
    }

    private IEnumerator ChainLightningEffect(Vector2 startPosition, int remainingChains, float currentDamage)
    {
        Vector2 currentPosition = startPosition;
        List<Vector3> positions = new List<Vector3> { currentPosition };

        GameObject effectInstance = null;
        LineRenderer lineRenderer = null;
        if(chainLightningEffect != null)
        {
            effectInstance = Instantiate(chainLightningEffect, startPosition, Quaternion.identity);
            lineRenderer = effectInstance.GetComponent<LineRenderer>();
        }

        for (int i = 0; i < remainingChains; i++)
        {
            // Find closest enemy
            Collider2D nearestEnemy = FindClosestEnemy(currentPosition);
            if (nearestEnemy == null)
            {
                Debug.Log("No more enemies in range.");
                break;
            }

            var enemy = nearestEnemy.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage);
                Debug.Log($"Chain Lightning hit: {enemy.name}, Chain {i + 1}, Damage: {currentDamage}");
            }

            currentPosition = nearestEnemy.transform.position;
            positions.Add(currentPosition);
            currentDamage *= 1 - damageReductionPerChain;

            yield return new WaitForSeconds(0.1f); // Small delay between chains for visual effect
        }

        // Instantiate chain lightning effect and set up LineRenderer
        if (chainLightningEffect != null && positions.Count > 1)
        {
            effectInstance = Instantiate(chainLightningEffect, positions[0], Quaternion.identity);
            lineRenderer = effectInstance.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = positions.Count;
                lineRenderer.SetPositions(positions.ToArray());
            }
            Destroy(effectInstance, 0.5f); // Destroy the effect after a short delay
        }
    }

    private Collider2D FindClosestEnemy(Vector2 currentPosition)
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(currentPosition, radius, whatIsEnemy);
        Collider2D nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (var enemy in enemiesInRange)
        {
            float distance = Vector2.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}
