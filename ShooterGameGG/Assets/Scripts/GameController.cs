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

    // Flag to prevent multiple upgrades at once
    private bool isUpgrading = false;

    private void Start()
    {
        if (weaponProgression.Length > 0)
        {
            playerController.EquipWeapon(weaponProgression[currentWeaponIndex]);
        }
        UpdateScoreUI();
    }

    public void AddScore(int Score)
    {
        playerScore += Score;
        UpdateScoreUI();

        // Only upgrade if the score threshold is reached and we aren't already upgrading
        if (playerScore >= ScoreForUpgrade && !isUpgrading)
        {
            StartCoroutine(UpgradeWeapon());
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + playerScore.ToString();
        }
        else
        {
            Debug.LogError("Score Text not found");
        }
    }

    // Coroutine to handle weapon upgrade to ensure one upgrade happens after another
    private IEnumerator UpgradeWeapon()
    {
        isUpgrading = true;  // Lock the upgrade process

        Debug.Log("Weapon upgraded: " + playerScore);

        // Upgrade the weapon
        currentWeaponIndex = Mathf.Clamp(currentWeaponIndex + 1, 0, weaponProgression.Length - 1);
        playerController.EquipWeapon(weaponProgression[currentWeaponIndex]);
        playerScore = 0;  // Reset score after upgrade

        yield return new WaitForSeconds(1f);  

        Debug.Log($"Current score after upgrade: {playerScore}");

        isUpgrading = false;  // Allow the next upgrade to happen
    }
}
