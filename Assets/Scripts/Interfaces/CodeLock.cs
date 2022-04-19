using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeLock : MonoBehaviour
{
    [SerializeField]
    Text code;
    public GameObject codePanel, gate, note;
    string codeInput;


    public string correctCode;
    void Update()
    {
        //(Added null check to prevent being flooded with nullrefexceptions -Mike)
        if (codeInput != null)
        {
            code.text = codeInput;

            if (codeInput == correctCode)
            {
                Debug.Log("Correct");
                gate.SetActive(false);
                note.SetActive(false);
                codePanel.SetActive(false);
            }

            if (codeInput.Length >= 4)
            {
                codeInput = "";
            }
        }
    }

    public void AddDigit (string digit)
    {
        codeInput += digit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            codePanel.SetActive (true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            codePanel.SetActive (false);
        }
    }
}
