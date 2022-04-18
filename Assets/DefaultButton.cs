using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultButton : MonoBehaviour
{
    public GameObject defaultButton;
    public GameObject backButton;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && backButton != null)
        {
            backButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton);
    }
}
