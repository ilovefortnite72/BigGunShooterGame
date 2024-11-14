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

    public abstract void ActivateAbility(Transform abilityOrigin);

}
