using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherDetectionVolume : MonoBehaviour
{
    public MotherPlayerHunter.detectionEvent typeOfColliderThisIs;
    private MotherPlayerHunter motherPlayerHunterScript;

    private float timer;


    void Start()
    {
        motherPlayerHunterScript = GetComponentInParent<MotherPlayerHunter>();
        if(motherPlayerHunterScript == null)
        {
            Debug.Log("Couldn't find mother script");
        }
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (typeOfColliderThisIs == MotherPlayerHunter.detectionEvent.detectionVolume && timer <= 0 && other.tag == "Player")
        {
            motherPlayerHunterScript.PlayerDetected(typeOfColliderThisIs, true);

            timer = 1;
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
