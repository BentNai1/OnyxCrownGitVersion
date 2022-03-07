using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    private string identity;
    [HideInInspector]
    public bool colliding;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cover")
        {
            colliding = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Cover")
        {
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cover")
        {
            print("Stopped colliding");
            colliding = false;
        }
    }
}
