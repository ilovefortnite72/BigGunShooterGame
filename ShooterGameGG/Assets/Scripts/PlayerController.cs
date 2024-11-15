using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SOGuns equippedWeapon;
    private GameObject CurrentWeaponInstance;
    public Transform weaponOrigin;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 moveInput;
    public Transform LeftArm;
    public Transform RightArm;

    private void Start()
    {
        
    }

    void Update()
    {
        LookDirection();
        Move();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            equippedWeapon.ActivateWeapon(transform);
        }

    }

    private void Move()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.velocity = moveInput * moveSpeed;

    }

    private void LookDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector2 direction = (mousePosition - transform.position).normalized;
        transform.up = direction;
        Debug.Log(mousePosition);

        if (weaponOrigin != null)
        {
            
            weaponOrigin.up = direction;
            LeftArm.up = direction;
            RightArm.up = direction;

        }
    }

    public void EquipWeapon(SOGuns NewWeapon)
    {
        if(CurrentWeaponInstance != null)
        {
            Destroy(CurrentWeaponInstance);
        }
        equippedWeapon = NewWeapon;
        weaponOrigin.localRotation = Quaternion.Euler(Vector3.zero);

        if (NewWeapon.gunPrefab != null)
        {
            CurrentWeaponInstance = Instantiate(NewWeapon.gunPrefab, weaponOrigin.position, weaponOrigin.rotation, weaponOrigin);
        }

    }
}
