using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherDetectionVolume : MonoBehaviour
{
    public MotherPlayerHunter.detectionEvent typeOfColliderThisIs;
    private MotherPlayerHunter motherPlayerHunterScript;


    void Start()
    {
        motherPlayerHunterScript = GetComponentInParent<MotherPlayerHunter>();
        if(motherPlayerHunterScript == null)
        {
            Debug.Log("Couldn't find mother script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            motherPlayerHunterScript.PlayerDetected(typeOfColliderThisIs, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            motherPlayerHunterScript.PlayerDetected(typeOfColliderThisIs, false);
        }
    }
}
