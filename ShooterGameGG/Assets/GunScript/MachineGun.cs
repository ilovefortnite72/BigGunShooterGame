using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineGun", menuName = "Guns/MachineGun")]
public class MachineGun : SO_Guns
{

    private void Awake()
    {
        fireRate = 0.1f;
    }

    public override void ActivateWeapon(Transform transform)
    {
        
    }


    IEnumerator Fire(Transform transform)
    {
        while (Input.GetKey("0"))
        {
            //Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(fireRate);
        }
    }
}
