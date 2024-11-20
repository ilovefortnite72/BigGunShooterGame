using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private float damage;
    private float radius;
    private AudioClip explosionSound;


    public void Initialize(float radius, float damage, AudioClip explosionSound)
    {
        this.damage = damage;
        this.radius = radius;
        this.explosionSound = explosionSound;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in colliders)
        {
            EnemyController enemy = hit.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if(explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }


        ScreenShakeEffect ScreenShake = Camera.main.GetComponent<ScreenShakeEffect>();
        if(ScreenShake != null)
        {
            ScreenShake.TriggerShake(0.5f, 0.3f);
        }


        //add vfx here

        Destroy(gameObject);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
