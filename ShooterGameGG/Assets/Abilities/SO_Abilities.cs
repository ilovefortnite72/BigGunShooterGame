using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/SO_Abilities")]
public abstract class SOAbilities : ScriptableObject
{
    public string abilityName;
    public Sprite abilitySprite;
    public float cooldown;
    public float range;
    public float damage;
    public float duration;
    public float radius;
    public float speed;
    public float knockback;
    public float damageOverTime;
    public float slowAmount;
    public float stunDuration;
    public Transform player;
    public GameObject orbPrefab;

    private float lastActivatedTime;

    public UIController AbilityUI;

    public enum ProjectileDamageType { Ice, Fire }
    public ProjectileDamageType DamageType;

    // Checks if the ability is on cooldown
    public bool IsOnCooldown()
    {
        return Time.time < lastActivatedTime + cooldown; 
    }


    public virtual void ActivateAbility(Transform player)
    {
        if (!IsOnCooldown())
        {
            UseAbility(player);  
            lastActivatedTime = Time.time;

            if (AbilityUI != null)
            {
                AbilityUI.StartCooldown();
            }
        }
    }

    protected abstract void UseAbility(Transform player);


    
}



