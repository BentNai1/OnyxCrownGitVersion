using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject settingsMenuUI;
    public GameObject controlsMenuUI;
    public GameObject ResumeButton;
    public GameObject SettingsButton;
    public GameObject ControlsButton;
    [HideInInspector]
    public GameObject AbilityWheel;
    public GameObject MenuButton;
    public GameObject QuitButton;

    public GameObject defaultButton;

    private void Start()
    {
        AbilityWheel = GameObject.Find("AbilityWheel");
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
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
        controlsMenuUI.SetActive(false);
        AbilityWheel.SetActive(true);



         Time.timeScale = 1f;
         GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        ResumeButton.SetActive(true);
        SettingsButton.SetActive(true);
        ControlsButton.SetActive(true);
        MenuButton.SetActive(true);
        QuitButton.SetActive(true);
        AbilityWheel.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton);
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
