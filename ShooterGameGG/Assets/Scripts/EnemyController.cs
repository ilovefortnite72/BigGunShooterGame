using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
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
    private NavMeshAgent agent;

    [Header("Score")]
    public GameManager gm;
    public int scoreValue = 10;

    void Start()
    {
        currenthealth = maxhealth;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpeed = baseSpeed;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found");
        }
        else
        {
            agent.speed = currentSpeed;
        }
    }


    private void Update()
    {
        MoveToPlayer();
        LookatPlayer();
        

    }

    private void LookatPlayer()
    {
        Vector3 PlayerPosition = target.position;
        Vector2 direction = (PlayerPosition - transform.position).normalized;
        transform.up = direction;

    }

    private void MoveToPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target != null && agent != null)
        {
            agent.SetDestination(target.position);
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
        gm.AddScore(scoreValue);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
