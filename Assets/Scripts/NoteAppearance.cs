using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppearance : MonoBehaviour
{
    [SerializeField]
    private Image _noteImage;
    [SerializeField]
    private Text _noteText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement.playerBusy = true;
            _noteImage.enabled = true;
             _noteText.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement.playerBusy = false;
            _noteImage.enabled = false;
             _noteText.enabled = false;
        }
    }
}
