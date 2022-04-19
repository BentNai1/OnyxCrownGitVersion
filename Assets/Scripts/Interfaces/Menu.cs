using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject settingsMenu, controlsMenu, loreMenu, lore2Menu, lore3Menu, lore4Menu, creditsMenu;

    public GameObject settingsButton, controlsButton, loreButton, creditsButton;

    private GameObject lastSelectedButton;

    private GameObject currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        settingsButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
        controlsButton.GetComponent<Button>().onClick.AddListener(OpenControls);
        loreButton.GetComponent<Button>().onClick.AddListener(OpenLore);
        creditsButton.GetComponent<Button>().onClick.AddListener(OpenCredits);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMenu != null)
        {
            if (!currentMenu.activeInHierarchy && !lore2Menu.activeInHierarchy && !lore3Menu.activeInHierarchy && !lore4Menu.activeInHierarchy)
            {
                currentMenu = null;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(lastSelectedButton);
            }
        }
    }

    void OpenSettings()
    {
        lastSelectedButton = settingsButton;
        currentMenu = settingsMenu;
    }

    void OpenControls()
    {
        lastSelectedButton = controlsButton;
        currentMenu = controlsMenu;
    }

    void OpenLore()
    {
        lastSelectedButton = loreButton;
        currentMenu = loreMenu;
    }

    void OpenCredits()
    {
        lastSelectedButton = creditsButton;
        currentMenu = creditsMenu;
    }
}
