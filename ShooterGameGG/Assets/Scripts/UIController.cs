using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider cooldownSlider;
    public Image abilityIcon;

    public SOAbilities ability;

    private float coolDownTime;
    private float CooldownTimer;

    private void Start()
    {
        if(ability == null || cooldownSlider == null || abilityIcon == null)
        {
            Debug.LogError("UIController is missing a reference");
            return;
        }

        coolDownTime = ability.cooldown;
        cooldownSlider.maxValue = coolDownTime;
        cooldownSlider.value = coolDownTime;

        abilityIcon.sprite = ability.abilitySprite;

        CooldownTimer = 0;

    }

    private void Update()
    {
        if(CooldownTimer > 0)
        {
            CooldownTimer -= Time.deltaTime;
            cooldownSlider.value = CooldownTimer;


            if(CooldownTimer <= 0)
            {
                cooldownSlider.value = 0;
                cooldownSlider.value = coolDownTime;
            }
        }
    }


    public void StartCooldown()
    {
        CooldownTimer = coolDownTime;
        cooldownSlider.value = CooldownTimer;
    }
}
