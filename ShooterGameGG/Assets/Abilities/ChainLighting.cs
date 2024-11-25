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
        ChainLightningEffect(player.position, maxChains, damage);
    }

    
    private void ChainLightningEffect(Vector2 startPosition, int remainingChains, float currentDamage)
    {
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < remainingChains; i++)
        {
            //find closest enemy
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

            
            if (chainLightningEffect != null)
            {
                Instantiate(chainLightningEffect, nearestEnemy.transform.position, Quaternion.identity);
            }

            currentPosition = nearestEnemy.transform.position;
            currentDamage *= 1 - damageReductionPerChain;
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
