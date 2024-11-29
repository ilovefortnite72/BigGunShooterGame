using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrappleHookAbility", menuName = "Abilities/GrappleHook", order = 1)]
public class GrappleHook : SOAbilities
{
    public GameObject grapplePrefab; // Grapple hook prefab
    public LayerMask wallLayer; // Layer for walls
    public LayerMask enemyLayer; // Layer for enemies
    public float pullSpeed = 10f; // Speed at which to pull objects

    private GameObject grappleInstance;

    protected override void UseAbility(Transform playerTransform)
    {
        // Spawn the grapple hook
        grappleInstance = Instantiate(grapplePrefab, playerTransform.position, Quaternion.identity);

        GrappleHookController grappleBehavior = grappleInstance.GetComponent<GrappleHookController>();
        if (grappleBehavior != null)
        {
            grappleBehavior.Initialize(playerTransform, wallLayer, enemyLayer, range, pullSpeed, this);
        }
    }
}