using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherPlayerHunter : MonoBehaviour
{
    private PlayerMovement playerMovementScript;
    private CrouchToHide_Script crouchScript;

    private bool playerActivelyDetected;

    private MotherBrain motherBrainScript;
    private MotherMover motherMoverScript;
    private bool playerInKillRange;

    public enum detectionEvent { KillVolume, tooCloseToHideVolume, detectionVolume}

    

    private bool playerDetected;
    // Start is called before the first frame update
    void Start()
    {
        //find the one player in the scene
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        crouchScript = FindObjectOfType<CrouchToHide_Script>();

        motherBrainScript = GetComponent<MotherBrain>();
        motherMoverScript = GetComponent<MotherMover>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void PlayerDetected(detectionEvent detectionType, bool enteringVolume)
    {
        //if player gets RIGHT up to ai, tell movement to lunge at the player
        if (detectionType == detectionEvent.KillVolume && enteringVolume)
        {
            playerInKillRange = true;
            motherMoverScript.ActivateLunge(playerMovementScript.transform.position);
            Debug.Log("Lunging at player");
        }


        /**
        if (detectionType == detectionEvent.tooCloseToHideVolume && enteringVolume)
        {
            if (crouchScript.hiding && !playerActivelyDetected)
            {
                motherBrainScript.UpdateDebug("Ignoring hidden player");
            }

            if (crouchScript.hiding && playerActivelyDetected)
            {
                motherBrainScript.UpdateDebug("Saw player try to hide");
            }
        }
        **/

        if (detectionType == detectionEvent.detectionVolume && enteringVolume)
        {
            if(crouchScript.hiding)
            {
                motherBrainScript.UpdateDebug("Ignoring hidden player.");
            }

            else
            {
                motherBrainScript.AddQuedWaypoint(playerMovementScript.forwardWaypoint, playerMovementScript.backwardWaypoint);
            }
        }
    }
}
