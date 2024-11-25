using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float Bspeed;
    private Vector2 direction;
    private float destroyTimer = 2f;

    private Coroutine returnToPoolTimer;

    private void OnEnable()
    {
        returnToPoolTimer = StartCoroutine(returnToAfterTime());
    }

    //add force once instantiated
    public void Initialize(float speed, float bulletSpeed, Vector2 direction)
    {
        this.Bspeed = speed;
        this.direction = direction;
        
    }

    private void Update()
    {
        transform.Translate(direction * Bspeed * Time.deltaTime);
    }



    //if the bullet hits an enemy, deal damage to it and destroy the bullet

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(Bspeed);
            StopCoroutine(returnToPoolTimer);
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

    }


    private IEnumerator returnToAfterTime()
    {
        float elapsedTime = 0;
        while(elapsedTime < destroyTimer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}

