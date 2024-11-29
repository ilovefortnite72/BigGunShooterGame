using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MindBlast", menuName = "Abilities/MindBlast")]
public class MindBlastAbility : SOAbilities
{
    public float angle = 45f; 
    public float effectDuration = 10f; 

    protected override void UseAbility(Transform player)
    {
        Vector2 direction = player.up; // The direction the player is facing.
        Collider2D[] enemiesInRange = GetEnemiesInCone(player.position, direction, range, angle);

        foreach (Collider2D enemy in enemiesInRange)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                // Apply the conversion to ally effect
                enemyController.ConvertToAlly(player, effectDuration);

                // Optionally change their color to indicate conversion
                CoroutineHelper.Instance.StartCoroutine(FlashEnemyColor(enemyController, Color.green)); // Flash green as an indicator
            }
        }
    }

    private Collider2D[] GetEnemiesInCone(Vector2 origin, Vector2 direction, float range, float angle)
    {
        // Create a cone of detection based on player direction, angle, and range
        List<Collider2D> enemiesInCone = new List<Collider2D>();
        Collider2D[] allEnemies = Physics2D.OverlapCircleAll(origin, range);

        foreach (var enemy in allEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Vector2 directionToEnemy = ((Vector2)enemy.transform.position - origin).normalized;
                float angleToEnemy = Vector2.Angle(direction, directionToEnemy);

                if (angleToEnemy <= angle / 2)
                {
                    enemiesInCone.Add(enemy);
                }
            }
        }

        return enemiesInCone.ToArray();
    }

    private IEnumerator FlashEnemyColor(EnemyController enemy, Color color)
    {
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.color = originalColor;
        }
    }
}