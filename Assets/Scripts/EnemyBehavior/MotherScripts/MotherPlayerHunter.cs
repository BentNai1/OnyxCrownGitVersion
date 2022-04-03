using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MotherPlayerHunter : MonoBehaviour
{
    private PlayerMovement playerMovementScript;
    private CrouchToHide_Script crouchScript;

    private bool playerActivelyDetected;

    private MotherBrain motherBrainScript;
    private MotherMover motherMoverScript;
    private MotherAnimSoundScript motherAnimSound;
    private bool playerInKillRange;

    private string debugText = "Player position";

    public enum detectionEvent { KillVolume, tooCloseToHideVolume, detectionVolume}

    

    private bool playerDetected;


#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Handles.Label((transform.position + Vector3.up * 20), debugText);
    }

#endif

    void Start()
    {
        //find the one player in the scene
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        crouchScript = FindObjectOfType<CrouchToHide_Script>();

        motherBrainScript = GetComponent<MotherBrain>();
        motherMoverScript = GetComponent<MotherMover>();
        motherAnimSound = GetComponent<MotherAnimSoundScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDebug(string text)
    {
        debugText = text;
    }

    public void PlayerDetected(detectionEvent detectionType, bool enteringVolume)
    {
        //if player gets RIGHT up to ai, tell movement to lunge at the player
        if (detectionType == detectionEvent.KillVolume && enteringVolume)
        {
            playerInKillRange = true;
            motherMoverScript.ActivateLunge(playerMovementScript.transform.position);
            UpdateDebug("Player in lunge distance.");
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
                UpdateDebug("Ignoring hidden player.");
            }

            else
            {
                UpdateDebug("Player detected!");
                motherBrainScript.AddQuedWaypoint(playerMovementScript.forwardWaypoint, playerMovementScript.backwardWaypoint);
            }
        }

        if (detectionType == detectionEvent.detectionVolume && !enteringVolume)
        {
            UpdateDebug("Player out of detection range.");
        }
    }
}
