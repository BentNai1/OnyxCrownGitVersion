using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject settingsMenuUI;
    public GameObject ResumeButton;
    public GameObject SettingsButton;
    public GameObject MenuButton;
    public GameObject QuitButton;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);

         Time.timeScale = 1f;
         GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        ResumeButton.SetActive(true);
        SettingsButton.SetActive(true);
        MenuButton.SetActive(true);
        QuitButton.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log ("Quitting game...");
        Application.Quit();
    }
}
