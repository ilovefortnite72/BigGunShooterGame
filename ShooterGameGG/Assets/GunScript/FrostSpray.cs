using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostSpray : MonoBehaviour
{
    public FrostGun frostGun;
    private float tickRate = 0.5f;


    private void OnTriggerStay2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyController>();
        if(enemy != null)
        {
            enemy.TakeDamage(frostGun.damage * (tickRate * Time.deltaTime));
            enemy.SlowEffect(frostGun.SlowIncrement, frostGun.MaxSlow);
        }
    }
}
