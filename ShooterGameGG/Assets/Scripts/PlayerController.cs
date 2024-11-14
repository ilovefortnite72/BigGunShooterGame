using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SOGuns equippedWeapon;
    public WeaponObject WeaponObject;
    private GameObject CurrentWeaponInstance;
    public Transform weaponOrigin;
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        LookDirection();
        Move();

        if (Input.GetKeyDown("0"))
        {
            equippedWeapon.ActivateWeapon(transform);
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    private void LookDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void EquipWeapon(SOGuns NewWeapon)
    {
        equippedWeapon = NewWeapon;
        if (CurrentWeaponInstance != null)
        {
            Destroy(CurrentWeaponInstance);
        }
        CurrentWeaponInstance = Instantiate(NewWeapon.gunPrefab, weaponOrigin.position, weaponOrigin.rotation, weaponOrigin);
        WeaponObject = CurrentWeaponInstance.GetComponent<WeaponObject>();


    }
}
