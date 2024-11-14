using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SOGuns[] weaponProgression;
    private int currentWeaponIndex = 0;
    private int playerScore = 0;
    public int ScoreForUpgrade = 100;


    public PlayerController playerController;

    private void Start()
    {
        if(weaponProgression.Length > 0)
        {
            playerController.EquipWeapon(weaponProgression[currentWeaponIndex]);
        }
    }

    public void AddScore(int Score)
    {
        playerScore += Score;
        if(playerScore >= ScoreForUpgrade)
        {
            UpgradeWeapon();
            playerScore = 0;
        }
    }

    private void UpgradeWeapon()
    {
        currentWeaponIndex = Mathf.Clamp(currentWeaponIndex + 1, 0, weaponProgression.Length - 1);
        playerController.EquipWeapon(weaponProgression[currentWeaponIndex]);
    }
}
