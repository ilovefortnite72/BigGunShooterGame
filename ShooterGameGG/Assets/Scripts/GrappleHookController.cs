using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookController : MonoBehaviour
{
    private Transform playerTransform;
    private LayerMask wallLayer;
    private LayerMask enemyLayer;
    private float range;
    private float pullSpeed;
    private GrappleHook grapplingHookAbility;

    private Rigidbody2D rb;

    public void Initialize(Transform player, LayerMask walls, LayerMask enemies, float maxRange, float speed, GrappleHook ability)
    {
        playerTransform = player;
        wallLayer = walls;
        enemyLayer = enemies;
        range = maxRange;
        pullSpeed = speed;
        grapplingHookAbility = ability;

        rb = GetComponent<Rigidbody2D>();

        // Launch the grapple hook
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position).normalized;
        rb.velocity = direction * grapplingHookAbility.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            return;
        }
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            // Grapple hit a wall
            StopGrapple();
            PullPlayerToPoint(collision.transform.position);
        }
        else if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            // Grapple hit an enemy
            StopGrapple();
            PullEnemyToPlayer(collision.transform);
        }
    }

    private void StopGrapple()
    {
        // Stop grapple hook movement
        rb.velocity = Vector2.zero;
        Destroy(gameObject); // Destroy the grapple hook
    }

    private void PullPlayerToPoint(Vector2 point)
    {
        // Pull the player towards the point
        playerTransform.position = Vector2.MoveTowards(playerTransform.position, point, pullSpeed * Time.deltaTime);
    }

    private void PullEnemyToPlayer(Transform enemyTransform)
    {
        // Pull the enemy towards the player
        enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, playerTransform.position, pullSpeed * Time.deltaTime);
    }
}
