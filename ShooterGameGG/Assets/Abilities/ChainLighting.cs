using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChainLightingAbility", menuName = "Abilities/ChainLighting")]
public class ChainLighting : SOAbilities
{
    public int remainingChains;
    public LayerMask whatIsEnemy;
    public GameObject chainLightingEffect;

    protected override void ActivateAbility(Transform player)
    {
        Debug.Log("Chain Lighting Ability Activated");
        ChainLightningAbility(player.position, remainingChains);
    }

    private void ChainLightningAbility(Vector2 startPosition, int remainingChains)
    {
        Vector2 currentPosition = startPosition;

        for (int i = 0; i < remainingChains; i++)
        {
            // Find the closest enemy within range
            Collider2D nearestEnemy = FindClosetEnemy(currentPosition);
            if (nearestEnemy == null)
            {
                Debug.Log("No more enemies in range.");
                break; // Stop chaining if no enemy is found
            }

            // Deal damage to the enemy
            var enemy = nearestEnemy.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Chain Lightning hit: {enemy.name}, Chain {i + 1}");
            }

            
            //if (lightningEffect != null)
            //{
            //    Instantiate(lightningEffect, nearestEnemy.transform.position, Quaternion.identity);
            //}

            
            currentPosition = nearestEnemy.transform.position;
        }
    }




    private Collider2D FindClosetEnemy(Vector2 currentPosition)
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
