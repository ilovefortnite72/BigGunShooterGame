using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorBladeController : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;
    private int remainingBounces;
    private LayerMask whatIsEnemy;
    private Rigidbody2D rb;

    public float spinSpeed = 360f;


    public void Initialize(Vector2 direction, float bladeSpeed, float damage, int maxBounces, LayerMask whatIsEnemy)
    {
        this.direction = direction;
        this.speed = bladeSpeed;
        this.damage = damage;
        this.remainingBounces = maxBounces;
        this.whatIsEnemy = whatIsEnemy;
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null");
        }
    }



    private void FixedUpdate()
    {
        if (rb == null)
        {
            rb.velocity = direction * speed;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    { //check is enemy is hit
        if(((1 << collision.gameObject.layer) & whatIsEnemy) != 0)
        { //if hit enemy, deal damage and destroy blade
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }
        //bouncing off walls logic
        if(remainingBounces > 0)
        {
            remainingBounces--;
            direction = Vector2.Reflect(direction, collision.contacts[0].normal);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
