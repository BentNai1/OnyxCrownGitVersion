using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotator : MonoBehaviour
{
    public GameObject door;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetActive(false);
        }
    }
}
