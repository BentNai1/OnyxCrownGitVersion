using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotator : MonoBehaviour
{
    public GameObject door;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Mother"))
        {
            door.SetActive(false);
        }
        // else
        // {
        //     door.SetActive(true);
        // }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Mother"))
        {
            door.SetActive(true);
        }
    }

}
