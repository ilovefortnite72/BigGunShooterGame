using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CryoBlast : MonoBehaviour
{
    public float freezeDuration = 2f;
    public float radius = 2f;  // The radius in which enemies will be frozen
    public float lifetime = 5f;  // Lifetime of the freeze orb
    private float timer;

    void Start()
    {
        timer = lifetime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);  // Destroy the orb after its lifetime expires
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply freeze effect to all enemies within the radius
            other.GetComponent<EnemyController>().ApplyFreezeEffect(freezeDuration);
        }
    }

    // Optional: Draw a gizmo in the editor for the radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
