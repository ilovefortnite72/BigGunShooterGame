using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public SOGuns equippedWeapon;
    private GameObject CurrentWeaponInstance;
    public Transform weaponOrigin;
    private Vector2 target;
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 moveInput;
    public Transform LeftArm;
    public Transform RightArm;
    public TextMeshProUGUI currentammoText;
    public TextMeshProUGUI maxammoText;

    private void Start()
    {
        UpdateAmmoUI();
    }

    

    void Update()
    {
        LookDirection();
        Move();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            equippedWeapon.ActivateWeapon(weaponOrigin, target);
            UpdateAmmoUI();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            equippedWeapon.Reload();
            UpdateAmmoUI();
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
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector2 direction = (target - transform.position).normalized;
        transform.up = direction;
        

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

    private void UpdateAmmoUI()
    {
        if(equippedWeapon != null)
        {
            currentammoText.text = equippedWeapon.currentAmmo.ToString();
            maxammoText.text = equippedWeapon.maxAmmo.ToString();
        }
        else
        {
            currentammoText.text = "-";
            maxammoText.text = "-";
        }
    }

    
}
