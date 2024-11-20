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

    public bool AbilityAvailable()
    {
        return Time.time >= cooldown;
    }

    public void Use(Transform player)
    {
        if (AbilityAvailable())
        {
            ActivateAbility(player);
            cooldown = Time.time + cooldown;
        }
        else
        {
            Debug.Log("Ability is on cooldown");
        }
    }
    
    protected abstract void ActivateAbility(Transform player);




}

