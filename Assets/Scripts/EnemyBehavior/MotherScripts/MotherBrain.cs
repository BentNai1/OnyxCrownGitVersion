using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MotherBrain : MonoBehaviour
{
    private GameObject previousWaypoint;
    private GameObject nextWaypoint;

    private GameObject quedWaypointFromHunterScript;
    private PlayerMovement playerMovementScript;

    private GameObject activeWaypoint;
    private WaypointPlayerMovement activeWaypointScript;
    private MotherMover motherMoverScript;
    private MotherAnimSoundScript animSoundScript;

    private bool patrollingForward = true;
    private int diceVariable;

    [Header ("   ")]
    public float minPauseDuration = 1;
    public float maxPauseDuration = 1;
    private float pauseDuration;

    
    private int successRange = 9;
    [Header("x/100 to trigger")]
    public int altPathPause = 1;
    public int noAltPathPause = 1;
    public int goAltPath = 1;
    public int goBack = 1;
    private string debugText = "Mother brain";

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Handles.Label((transform.position + Vector3.up * 10), debugText);
    }

#endif
    void Start()
    {
        motherMoverScript = gameObject.GetComponent<MotherMover>();
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        animSoundScript = GetComponent<MotherAnimSoundScript>();
    }

    //pause timer
    void Update()
    {
        //at end run DecideNextWaypoint again
        if (pauseDuration > minPauseDuration)
        {
            pauseDuration -= Time.deltaTime;

            //cancel pause if waypoint is qued up
            if (quedWaypointFromHunterScript != null)
            {
                DecideNextWaypoint();
            }

            else if (pauseDuration <= minPauseDuration)
            {
                DecideNextWaypoint();
                
            }
        }
    }

    //decide if pausing, if not go to waypoint
    public void DecideNextWaypoint()
    {
        //check for qued waypoint, use that if there is one
        if (quedWaypointFromHunterScript != null)
        {
            SetNextPoint(quedWaypointFromHunterScript);

            quedWaypointFromHunterScript = null;
        }


        //check to see if pause animation
        else if (PauseCheck() == true)
        {
            PauseMovement();
        }
        else
        {
            Debug.Log("Mother AI running waypoint check...");
            WaypointCheck1();
        }
    }

    //rand decide if pausing
    private bool PauseCheck()
    {
        //Check if alt paths are available, roll different odds

        if (activeWaypointScript.virtualAltUpTransform != null || activeWaypointScript.virtualAltDownTransform != null)
        {
            if (Random.Range(1, 100) <= altPathPause)
                return true;
            else
                return false;
        }
        else
        {
            if (Random.Range(1, 100) <= noAltPathPause)
                return true;
            else
                return false;
        }
    }


    //set pause timer
    public void PauseMovement()
    {
        pauseDuration = Random.Range(minPauseDuration, maxPauseDuration);
        debugText = "Pausing...";
        animSoundScript.PlayWalking(false);
        animSoundScript.PlayIdle();
    }


    //Decide if using alt paths, passes to WaypointCheck2 if not
    private void WaypointCheck1()
    {
        //Check if alt paths are available, rand decide if using
        if (activeWaypointScript.altWayPointUp != null || activeWaypointScript.altWayPointDown != null)
        {
            if (Random.Range(1, 100) <= goAltPath)
            {
                //if both available, 50/50, else default to whichever is available
                if (activeWaypointScript.altWayPointUp != null && activeWaypointScript.altWayPointDown != null)
                {
                    if (Random.Range(0, 1) == 1)
                    {
                        SetNextPoint(activeWaypointScript.altWayPointUp);
                        UpdateDebug("Up waypoint, " + nextWaypoint.name);
                    }
                    else
                    {
                        SetNextPoint(activeWaypointScript.altWayPointDown);
                        UpdateDebug("Down waypoint, " + nextWaypoint.name);
                    }
                }
                else if (activeWaypointScript.altWayPointUp != null)
                {
                    SetNextPoint(activeWaypointScript.altWayPointUp);
                    UpdateDebug("Up waypoint, " + nextWaypoint.name);
                }
                else
                {
                    SetNextPoint(activeWaypointScript.altWayPointDown);
                    UpdateDebug("Down waypoint, " + nextWaypoint.name);
                }
            }

            //go on to part 2 if fails to use alt paths
            else
                WaypointCheck2();
        }
        else
            WaypointCheck2();
    }

    //called from WaypointCheck1, decides if going forwards or backwards.
    private void WaypointCheck2()
    {
        //check if both waypoints are valid, rand decide which
        if (activeWaypointScript.nextWaypoint != null && activeWaypointScript.previousWayPoint != null)
            if (Random.Range(1, 100) <= goBack)
            {
                //Rand back, 'patrollingForward' inverts which path
                if (patrollingForward)
                {
                    SetNextPoint(activeWaypointScript.previousWayPoint);
                    patrollingForward = false;
                }
                else
                {
                    SetNextPoint(activeWaypointScript.nextWaypoint);
                    patrollingForward = true;
                }
                UpdateDebug("Previous waypoint, " + nextWaypoint.name); 
            }

            else
            {
                //Rand foward, 'patrollingForward' inverts which path
                if (patrollingForward)
                    SetNextPoint(activeWaypointScript.nextWaypoint);
                else
                    SetNextPoint(activeWaypointScript.previousWayPoint);
                UpdateDebug("Next waypoint, " + nextWaypoint.name);
            }

        //otherwise, use whichever direction is available.
        else if (activeWaypointScript.nextWaypoint != null)
        { 
            SetNextPoint(activeWaypointScript.nextWaypoint);
            UpdateDebug("Next waypoint, " + nextWaypoint.name);
            patrollingForward = false;
        }
        else if (activeWaypointScript.previousWayPoint != null)
        { 
            SetNextPoint(activeWaypointScript.previousWayPoint);
            UpdateDebug("Previous waypoint, " + nextWaypoint.name);
            patrollingForward = true;
        }

        //if there is somehow neither, go back to try and hit an alt path. This should NEVER happen, but who knows?
        else
            WaypointCheck1();
    }


    //change which waypoint is 'active'; other elements of the script care about that
    public void ChangeActiveWaypoint(GameObject waypoint)
    {
        activeWaypoint = waypoint;
        activeWaypointScript = activeWaypoint.GetComponent<WaypointPlayerMovement>();
    }

    //tell mover to go towards active waypoint
    private void SetNextPoint(GameObject Waypoint)
    {
        previousWaypoint = activeWaypoint;
        nextWaypoint = Waypoint;
        motherMoverScript.SetMoveDestination(nextWaypoint);
        animSoundScript.PlayWalking(true);
    }

    //used for unstuck, go to previous waypoint
    public void SwapActiveWaypoint()
    {
        GameObject tempWaypoint = nextWaypoint;
        nextWaypoint = previousWaypoint;
        previousWaypoint = tempWaypoint;
        motherMoverScript.SetMoveDestination(nextWaypoint);
        Debug.Log("Move destination swapped to " + nextWaypoint);
        patrollingForward = !patrollingForward;
    }

    public void UpdateDebug(string text)
    {
        debugText = text;
    }

    //instead of randomly determining next waypoint, go to this one instead (because hunter script spotted the player)
    public void AddQuedWaypoint(GameObject frontPlayerWaypoint, GameObject backPlayerWaypoint)
    {
        //check if ai is in same stretch of two points as player
        if ((GameObject.ReferenceEquals(frontPlayerWaypoint, nextWaypoint) && GameObject.ReferenceEquals(backPlayerWaypoint, previousWaypoint)) || (GameObject.ReferenceEquals(frontPlayerWaypoint, previousWaypoint) && GameObject.ReferenceEquals(backPlayerWaypoint, nextWaypoint)))
        {
            //check if player is closer to prev point than ai, if so swap points
            if(Vector3.Distance(playerMovementScript.transform.position, previousWaypoint.transform.position) < (Vector3.Distance(transform.position, previousWaypoint.transform.position)))
            {
                SwapActiveWaypoint();
                UpdateDebug("Player detected behind!");
            }

        }


        //if a waypoint matches next waypoint, set the other one to be qued up.
        else if (GameObject.ReferenceEquals(frontPlayerWaypoint, nextWaypoint))
        {
            quedWaypointFromHunterScript = backPlayerWaypoint;
            UpdateDebug("Player detected towards " + quedWaypointFromHunterScript);
        }
        else if (GameObject.ReferenceEquals(backPlayerWaypoint, nextWaypoint))
        {
            quedWaypointFromHunterScript = frontPlayerWaypoint;

        }
    }
}
