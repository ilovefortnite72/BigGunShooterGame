using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;
    private float damage;

    //add force once instantiated
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * speed;
        }
    }

    //get damage from the gun script and set it to the bullet
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }


    //if the bullet hits an enemy, deal damage to it and destroy the bullet

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

