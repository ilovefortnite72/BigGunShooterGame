using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Weapon Stuff")]
    public SOGuns equippedWeapon;
    private GameObject CurrentWeaponInstance;
    public Transform weaponOrigin;
    private Vector2 target;
    private bool isShooting;
    


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
    public GameObject ammoInfoUI;
    public Slider fuelSlider;
    public GameObject deathScreen;
    


    [Header("Healh Stuff")]
    public float maxhealth = 100f;
    private float currenthealth;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 2f;
    private float damage;

    private void Start()
    {
        currenthealth = maxhealth;
        UpdateWeaponUI();
        UpdateUIVisibility();
    }

    void Update()
    {
        LookDirection();
        Move();
        UpdateWeaponUI();

        HandleShooting();
        

    }


    private void HandleShooting()
    {
        if (equippedWeapon != null)
        {
            // Handling fire modes
            if (equippedWeapon.canHoldTrigger) // Continuous fire while holding the mouse
            {
                if (Input.GetMouseButton(0)) // While holding mouse button
                {
                    if (!isShooting)
                    {
                        isShooting = true;
                        StartCoroutine(FireWeapon());
                    }
                }
                else if (Input.GetMouseButtonUp(0)) // Stop shooting when the mouse is released
                {
                    isShooting = false;
                }
            }
            else // Single-shot weapons (trigger fire)
            {
                if (Input.GetMouseButtonDown(0)) // Fire on button press
                {
                    equippedWeapon.ActivateWeapon(weaponOrigin, target);
                    UpdateWeaponUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) // Reload action
            {
                equippedWeapon.Reload();
                UpdateWeaponUI();
            }
        }
    }

    private IEnumerator FireWeapon()
    {
        if (equippedWeapon.currentAmmo <= 0)
        {
            yield break; // Don't fire if no ammo is left
        }

        // Fire continuously while holding the mouse button, respecting the fire rate
        while (isShooting && equippedWeapon.currentAmmo > 0)
        {
            equippedWeapon.ActivateWeapon(weaponOrigin, target); // Call ActivateWeapon from the ScriptableObject
            UpdateWeaponUI();

            // Wait for the time between shots based on the fire rate
            yield return new WaitForSeconds(1f / equippedWeapon.fireRate); // Fire rate logic
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
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // Ensure the z position is set correctly
        target = Camera.main.ScreenToWorldPoint(mousePosition);
        

        Vector2 direction = (target - (Vector2)transform.position).normalized;
        
        transform.up = direction;

        if (weaponOrigin != null)
        {
            
            weaponOrigin.up = direction;
            LeftArm.up = direction;
            RightArm.up = direction;

        }
    }

    private void StopFiring()
    {
        // Stop firing when swapping weapons
        isShooting = false;
        StopAllCoroutines(); // Stop any ongoing fire coroutines
    }

    public void EquipWeapon(SOGuns newWeapon) // Refresh weapon upon picking up a new weapon
    {
        StopFiring(); // Stop firing the previous weapon before equipping the new one

        if (CurrentWeaponInstance != null)
        {
            Destroy(CurrentWeaponInstance);
        }

        equippedWeapon = newWeapon;
        weaponOrigin.localRotation = Quaternion.identity;

        if (newWeapon.gunPrefab != null) // Instantiate new weapon based on weaponManager logic
        {
            CurrentWeaponInstance = Instantiate(newWeapon.gunPrefab, weaponOrigin.position, Quaternion.Euler(0, 0, 90), weaponOrigin);
            UpdateWeaponUI();
        }
    }
    //update ammo ui
    public void UpdateUIVisibility()
    {
        if (equippedWeapon != null)
        {
            bool usesFuel = equippedWeapon.usesFuel;

            fuelSlider.gameObject.SetActive(usesFuel);
            ammoInfoUI.gameObject.SetActive(!usesFuel);

            if (usesFuel)
            {
                fuelSlider.maxValue = equippedWeapon.maxAmmo;
                fuelSlider.value = equippedWeapon.currentAmmo;
            }

        }
    }


    private void UpdateWeaponUI()
    {
        if (equippedWeapon != null)
        {
            if (equippedWeapon.usesFuel)
            {
                fuelSlider.value = equippedWeapon.currentAmmo;
            }
            else
            {
                currentammoText.text = equippedWeapon.currentAmmo.ToString();
                maxammoText.text = equippedWeapon.maxAmmo.ToString();
            }
        }
        else
        {
            currentammoText.text = "-";
            maxammoText.text = "-";
        }
    }

    //logic to take damage from enemies when too close. referenable from enemycontroller script
    public void TakeDamage(float damage)
    {
        if(isInvulnerable)
        {
            return;
        }
        currenthealth -= damage;
        
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
    //invulnerability to damage after taking damage so you dont instantly die
    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
    private void Die()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0;
    }

}
