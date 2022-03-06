using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour
{
    public Text CurrentText;
    public Text NextText;
    public Text ObjectiveFade;
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CurrentText.gameObject.SetActive(false);
            NextText.gameObject.SetActive(true);
            ObjectiveFade.gameObject.SetActive(true);
            Destroy(ObjectiveFade.gameObject, 3f);
        }
    }
}
