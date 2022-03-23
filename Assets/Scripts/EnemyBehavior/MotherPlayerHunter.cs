using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherPlayerHunter : MonoBehaviour
{
    private PlayerMovement playerMovementScript;
    private MotherBrain motherBrainScript;
    private MotherMover motherMoverScript;
    private bool playerInKillRange;

    public enum detectionEvent { KillVolume, closeVolume, detectionVolume}

    

    private bool playerDetected;
    // Start is called before the first frame update
    void Start()
    {
        //find the one player in the scene
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        motherBrainScript = GetComponent<MotherBrain>();
        motherMoverScript = GetComponent<MotherMover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void PlayerDetected(detectionEvent detectionType, bool enteringVolume)
    {
        if (detectionType == detectionEvent.KillVolume && enteringVolume)
        {
            playerInKillRange = true;
            motherMoverScript.ActivateLunge(playerMovementScript.transform.position);
            Debug.Log("Lunging at player");
        }
    }
}
