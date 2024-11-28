using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject optionsMenuPanel;
    public GameObject mainMenuPanel;

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnOptionsMenuClicked()
    {
        optionsMenuPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void OnCloseOptionsMenuClicked()
    {
        optionsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }


    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
