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
    [Header("Weapon Stuff")]
    public SOGuns equippedWeapon;
    private GameObject CurrentWeaponInstance;
    public Transform weaponOrigin;
    private Vector2 target;
    

    [Header("Movement Stuff")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 moveInput;
    public Transform LeftArm;
    public Transform RightArm;

    [Header("UI Stuff")]
    public TextMeshProUGUI currentammoText;
    public TextMeshProUGUI maxammoText;
    public Slider HealthSlider;


    [Header("Healh Stuff")]
    public float maxhealth = 100f;
    private float currenthealth;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 2f;
    private float damage;

    private void Start()
    {
        currenthealth = maxhealth;
        UpdateAmmoUI();
    }

    

    void Update()
    {
        LookDirection();
        Move();
        UpdateAmmoUI();

        if (Input.GetMouseButtonDown(0))
        {
            if(equippedWeapon != null)
            {
                Debug.Log("Firing");
                equippedWeapon.ActivateWeapon(weaponOrigin, target);
                UpdateAmmoUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(equippedWeapon != null)
            {
                equippedWeapon.Reload();
                UpdateAmmoUI();
            }
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
        weaponOrigin.localRotation = Quaternion.identity;

        if (NewWeapon.gunPrefab != null)
        {
            CurrentWeaponInstance = Instantiate(NewWeapon.gunPrefab, weaponOrigin.position, Quaternion.Euler(0, 0, 90), weaponOrigin);
            UpdateAmmoUI();
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


    public void TakeDamage(float damage)
    {
        if(isInvulnerable)
        {
            return;
        }
        currenthealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currenthealth}");
        HealthSlider.value = currenthealth;

        if (currenthealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invulnerability());
        }

    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
    private void Die()
    {
        throw new NotImplementedException();
    }

   
}
