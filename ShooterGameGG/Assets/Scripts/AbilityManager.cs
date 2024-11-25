using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public SOAbilities[] abilities;

    private void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                abilities[i].ActivateAbility(transform);
            }
        }
    }
}
