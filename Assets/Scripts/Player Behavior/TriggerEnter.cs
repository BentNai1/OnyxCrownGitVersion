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
        /*
        if (other.tag != "Player")
        {
            if (other.tag != "enemy")
            {
                print(gameObject.name + " collided with " + other.name);
                colliding = true;
            }
        }
        */
        if (other.tag == "Cover")
        {
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        if (other.name != "Player")
        {
            if (other.tag != "enemy")
            {
                colliding = false;
            }
        }
        */
        if (other.tag == "Cover")
        {
            colliding = false;
        }
    }
}
