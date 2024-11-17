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
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        if (target != null)
        {
            Vector2 moveDirection = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            transform.up = moveDirection;
            Debug.Log(moveDirection);
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
