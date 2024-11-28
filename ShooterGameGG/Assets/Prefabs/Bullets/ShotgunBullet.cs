using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBullet : MonoBehaviour
{
    public float Bspeed;
    private Vector2 direction;
    public int damage;
    private float destroyTimer = 2f;

    private Coroutine returnToPoolTimer;

    private void OnEnable()
    {
        returnToPoolTimer = StartCoroutine(returnToAfterTime());
    }

    //add force once instantiated
    public void Initialize(float speed, Vector2 direction)
    {
        this.Bspeed = speed;
        this.direction = direction;

    }


    private void FixedUpdate()
    {
        transform.Translate(direction * Bspeed * Time.deltaTime);
    }



    //if the bullet hits an enemy, deal damage to it and destroy the bullet

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("EnemyController not found on enemy");
            }
            StopCoroutine(returnToPoolTimer);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

    }




    private IEnumerator returnToAfterTime()
    {
        float elapsedTime = 0;
        while (elapsedTime < destroyTimer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

