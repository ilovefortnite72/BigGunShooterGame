using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RazorBlade", menuName = "Guns/RazorBlade")]
public class RazorBlade : SOGuns
{
    public GameObject razorBladePrefab;
    public int maxBounces;
    public float bladeSpeed;

    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        if (razorBladePrefab == null)
        {
            Debug.LogError("Razor Blade Prefab is null");
            return;
        }


        Vector2 direction = (target - (Vector2)weaponOrigin.position).normalized;

        GameObject blade = Instantiate(razorBladePrefab, weaponOrigin.position, Quaternion.identity);
        RazorBladeController bladeController = blade.GetComponent<RazorBladeController>();

        if (bladeController != null)
        {
            bladeController.Initialize(direction, bladeSpeed, damage, maxBounces, whatIsEnemy);
        }
        else
        {
            Debug.LogError("Razor Blade Controller is null");
        }
    }
}
