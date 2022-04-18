using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvisibleWall : MonoBehaviour
{
	
	public GameObject Wall;
	

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Wall.SetActive(false);
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Wall.SetActive(true);
        }
    }
}
