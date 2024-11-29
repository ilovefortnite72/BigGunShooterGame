using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Teleport Ability", menuName = "Abilities/TeleportAbility")]
public class TeleportAbility : SOAbilities
{
    // This method contains the logic for teleportation
    protected override void UseAbility(Transform player)
    {
        // Get the player's current position
        Vector3 currentPosition = player.position;

        // Calculate the teleport destination based on the player's forward direction and range
        Vector3 teleportDirection = player.up.normalized; // Assuming 'up' is the forward direction in 2D
        Vector3 targetPosition = currentPosition + teleportDirection * range;

        // Teleport the player to the target position
        player.position = targetPosition;

        Debug.Log($"{abilityName} activated. Teleported to {targetPosition}");
    }
}
