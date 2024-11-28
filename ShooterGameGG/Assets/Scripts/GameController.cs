using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SOGuns[] weaponProgression;
    private int currentWeaponIndex = 0;
    public int playerScore = 0;
    public int ScoreForUpgrade = 100;
    public TextMeshProUGUI scoreText;

    public PlayerController playerController;

    private void Start()
    {
        if(weaponProgression.Length > 0)
        {
            playerController.EquipWeapon(weaponProgression[currentWeaponIndex]);
        }
        UpdateScoreUI();
    }

    public void AddScore(int Score)
    {
        playerScore += Score;
        UpdateScoreUI();


        if (playerScore >= ScoreForUpgrade)
        {
            UpgradeWeapon();
            playerScore = 0;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null) 
        { 
            scoreText.text = "Score:" + playerScore.ToString();
        }
        else
        {
            Debug.LogError("Score Text not found");
        }
    }

    private void UpgradeWeapon()
    {
        currentWeaponIndex = Mathf.Clamp(currentWeaponIndex + 1, 0, weaponProgression.Length - 1);
        playerController.EquipWeapon(weaponProgression[currentWeaponIndex]);
    }
}
