using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrbAbility : MonoBehaviour
{
    public float damagePerSecond = 5f;
    public float radius = 2f;  // The radius in which enemies will be affected
    public float lifetime = 5f;  // Lifetime of the fire orb
    private float timer;

    void Start()
    {
        timer = lifetime;
        StartCoroutine(ApplyDamageOverTime());
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);  // Destroy the orb after its lifetime expires
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (timer > 0)
        {
            // Find all enemies in range using OverlapCircleAll
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            foreach (var enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<EnemyController>().TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }
            yield return null;  // Continue applying damage every frame
        }
    }

    // Optional: Draw a gizmo in the editor for the radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
