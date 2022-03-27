using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AreaFade : MonoBehaviour
{
     public Text ObjectiveFade;
     public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ObjectiveFade.gameObject.SetActive(true);
            Destroy(ObjectiveFade.gameObject, 3f);
        }
    }
}
