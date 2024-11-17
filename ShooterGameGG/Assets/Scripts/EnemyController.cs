using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currenthealth;
    public float moveSpeed = 5f;
    public Transform target;
    public float attackRange = 2f;

    void Start()
    {
        currenthealth = maxhealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance < attackRange)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.up = direction * moveSpeed * Time.deltaTime;
        }


    }


    public void TakeDamage(float damage)
    {
        currenthealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currenthealth}");

        if (currenthealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
