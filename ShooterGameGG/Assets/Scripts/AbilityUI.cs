using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public Image[] abilityIcons;
    public Image[] cooldownOverlays;

    public SOAbilities[] abilities;

    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            float cooldownRemaining = Mathf.Clamp(abilities[i].cooldown - (Time.time - abilities[i].cooldown), 0, abilities[i].cooldown);
            float fillAmount = cooldownRemaining / abilities[i].cooldown;

            cooldownOverlays[i].fillAmount = fillAmount; // Update overlay fill
        }
    }
}
