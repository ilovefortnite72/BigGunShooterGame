using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currenthealth;
    public float baseSpeed = 5f;
    public Transform target;
    public bool canAttack = true;
    public float attackRate = 2f;
    public float PlayerDamage = 10;
    public bool isDead = false;


    private float currentSpeed;
    private float slowEffect = 0f;
    private float slowTimer = 1f;
    public Animator eAnim;

    private Coroutine resetSlowCoroutine;

    void Start()
    {
        currenthealth = maxhealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpeed = baseSpeed;
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
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
            transform.up = moveDirection;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canAttack)
        {
            collision.GetComponent<PlayerController>().TakeDamage(PlayerDamage);
            canAttack = false;
            StartCoroutine(ResetAttack());
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
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

    public void SlowEffect(float slowAmount, float MaxSlow)
    {
        slowEffect = Mathf.Clamp(slowEffect + slowAmount, 0, MaxSlow);
        currentSpeed = baseSpeed * (1 - slowEffect);
        

        if (resetSlowCoroutine != null)
        {
            StopCoroutine(resetSlowCoroutine);
        }

        resetSlowCoroutine = StartCoroutine(ResetSlowEffect());
    }


    private IEnumerator ResetSlowEffect()
    {
        yield return new WaitForSeconds(slowTimer);
        slowEffect = 0;
        currentSpeed = baseSpeed;
    }

    private void Die()
    {
        eAnim.SetBool("isDead", true);
        isDead = true;
        //Destroy(gameObject);
    }
}
