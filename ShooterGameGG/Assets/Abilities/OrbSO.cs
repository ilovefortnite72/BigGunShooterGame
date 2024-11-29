using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Orb", menuName = "Abilities/Orb")]
public class OrbSO : SOAbilities
{
    public GameObject fireOrbPrefab;  // Reference to Fire Orb prefab
    public GameObject freezeOrbPrefab;  // Reference to Freeze Orb prefab
    private float lastActivatedTime;

    // This determines which orb to spawn, based on the ability's type
    public ProjectileDamageType orbType;

    public override void ActivateAbility(Transform player)
    {
        if (!IsOnCooldown())
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (player == null) 
            {
                return;
            }

            Transform playerTransform = playerObj.transform;

            GameObject orbPrefabToSpawn = (orbType == ProjectileDamageType.Fire) ? fireOrbPrefab : freezeOrbPrefab;
            SpawnOrb(playerTransform, orbPrefabToSpawn);
            lastActivatedTime = Time.time;

            if (AbilityUI != null)
            {
                AbilityUI.StartCooldown();
            }
        }
    }

    private void SpawnOrb(Transform player, GameObject orbPrefab)
    {
        GameObject orb = Instantiate(orbPrefab, player.position, Quaternion.identity);
        OrbController orbMovement = orb.GetComponent<OrbController>();
        orbMovement.playerTransform = player;
    }

    protected override void UseAbility(Transform player)
    {
        
    }
}
