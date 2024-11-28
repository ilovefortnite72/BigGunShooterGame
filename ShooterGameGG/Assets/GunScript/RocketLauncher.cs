using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RocketLauncher", menuName = "Guns/RocketLauncher")]
public class RocketLauncher : SOGuns
{
    public GameObject rocketPrefab;
    public float rocketSpeed;
    public float explosionRadius;
    public float explosionDamage;
    public AudioClip explosionSound;
    public GameObject explosionVFX;

    private void Start()
    {
        Initialize();
    }


    public override void Fire(Transform weaponOrigin, Vector2 target)
    {
        GameObject rocket = Instantiate(rocketPrefab, weaponOrigin.position, weaponOrigin.rotation);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            rb.velocity = weaponOrigin.up * rocketSpeed;
        }


        Rocket rocketScript = rocket.GetComponent<Rocket>();

        if(rocketScript != null)
        {
            rocketScript.Initialize(explosionRadius, explosionDamage, explosionSound, explosionVFX);
        }

    }
}
