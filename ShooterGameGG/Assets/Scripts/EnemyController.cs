using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxhealth = 100f;
    public float currenthealth;
    public float baseSpeed = 5f;
    private Transform originalTarget;
    public Transform target;
    public bool canAttack = true;
    public float attackRate = 2f;
    public float PlayerDamage = 10;
    public bool isDead = false;
    private bool isConverted = false;

    private float currentSpeed;
    private float slowEffect = 0f;
    private float slowTimer = 1f;
    public Animator eAnim;

    private Coroutine resetSlowCoroutine;
    private NavMeshAgent agent;

    [Header("Score")]
    public GameController gm;
    public int Score = 10;
    
    private Coroutine revertToEnemyCoroutine;
    public bool isFrozen;

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


        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gm == null)
        {
            Debug.LogError("GameController not found");
        }
    }


    private void Update()
    {
        MoveToPlayer();
        LookatPlayer();
        
        if (currenthealth <= 0)
        {
            Die();
        }

    }

    private void LookatPlayer()
    {
        Vector3 PlayerPosition = target.position;
        Vector2 direction = (PlayerPosition - transform.position).normalized;
        transform.up = direction;

    }

    private void MoveToPlayer()
    {
        if (isConverted)
        {
            findAndMoveToEnemy();
        }


        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target != null && agent != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void findAndMoveToEnemy()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemy"));

        Transform nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        
        foreach (var collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy") && collider.transform != transform) 
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = collider.transform;
                }
            }
        }

        
        if (nearestEnemy != null)
        {
            agent.SetDestination(nearestEnemy.position);
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

        if(collision.CompareTag("Enemy") && isConverted)
        {
            collision.GetComponent<EnemyController>().TakeDamage(PlayerDamage);
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

    public void ApplyFreezeEffect(float freezeDuration)
    {
        if (!isFrozen)
        {
            StartCoroutine(FreezeEnemy(freezeDuration));
        }
        
    }

    private IEnumerator FreezeEnemy(float freezeDuration)
    {
        isFrozen = true;
        
        float originalSpeed = currentSpeed;
        currentSpeed = 0;

        yield return new WaitForSeconds(freezeDuration);

        isFrozen = false;
        currentSpeed = originalSpeed;
    }


    private IEnumerator ResetSlowEffect()
    {
        yield return new WaitForSeconds(slowTimer);
        slowEffect = 0;
        currentSpeed = baseSpeed;
    }

    public void ConvertToAlly(Transform newTarget, float duration)
    {
        originalTarget = target;
        target = newTarget;
        isConverted = true;
        StartCoroutine(RevertToEnemy(duration));


    }

    private IEnumerator RevertToEnemy(float duration)
    {
        yield return new WaitForSeconds(duration);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        isConverted = false;
    }


    public void Die() 
    {
        Destroy(gameObject);

        if (gm != null)
        {
            gm.AddScore(Score);
        }
        else
        {
            Debug.Log("shoot me in the dick");
        }
        
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
