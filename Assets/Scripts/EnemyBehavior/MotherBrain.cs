using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MotherBrain : MonoBehaviour
{
    private GameObject previousWaypoint;
    private GameObject nextWaypoint;

    private GameObject activeWaypoint;
    private WaypointPlayerMovement activeWaypointScript;
    private MotherMover motherMoverScript;

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
    private string debugText = "Mother";

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Handles.Label((transform.position + Vector3.up * 10), debugText);
    }

#endif

    // Start is called before the first frame update
    void Start()
    {
        motherMoverScript = gameObject.GetComponent<MotherMover>();
    }

    // Update is called once per frame
    void Update()
    {
        //pause timer, at end decide next move again
        if (pauseDuration > minPauseDuration)
        {
            pauseDuration -= Time.deltaTime;
            if (pauseDuration <= minPauseDuration)
                DecideNextWaypoint();

        }

    }

    public void ChangeActiveWaypoint(GameObject waypoint)
    {
        activeWaypoint = waypoint;
        activeWaypointScript = activeWaypoint.GetComponent<WaypointPlayerMovement>();
    }

    public void DecideNextWaypoint()
    {
        //check to see if pause animation
        if (PauseCheck() == true)
        {
            PauseMovement();
        }
        else
        {
            WaypointCheck1();
        }
    }

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
    private void PauseMovement()
    {
        pauseDuration = Random.Range(minPauseDuration, maxPauseDuration);
        debugText = "Pausing...";
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
                    if (Random.Range(0, 1) <= 1)
                    {
                        debugText = "Up waypoint, " + nextWaypoint.name;
                        SetNextPoint(activeWaypointScript.altWayPointUp);
                    }
                    else
                    {
                        debugText = "Down waypoint, " + nextWaypoint.name;
                        SetNextPoint(activeWaypointScript.altWayPointDown);
                    }
                }
                else if (activeWaypointScript.virtualAltUpTransform != null)
                {
                    debugText = "Up waypoint, " + nextWaypoint.name;
                    SetNextPoint(activeWaypointScript.altWayPointUp);
                }
                else
                {
                    debugText = "Down waypoint, " + nextWaypoint.name;
                    SetNextPoint(activeWaypointScript.altWayPointDown);
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
                debugText = "Previous waypoint, " + nextWaypoint.name; 
            }

            else
            {
                //Rand foward, 'patrollingForward' inverts which path
                if (patrollingForward)
                    SetNextPoint(activeWaypointScript.nextWaypoint);
                else
                    SetNextPoint(activeWaypointScript.previousWayPoint);
                debugText = "Next waypoint, " + nextWaypoint.name;
            }

        //otherwise, use whichever direction is available.
        else if (activeWaypointScript.nextWaypoint != null)
        { 
            SetNextPoint(activeWaypointScript.nextWaypoint);
            debugText = "Next waypoint, " + nextWaypoint.name;
            patrollingForward = false;
        }
        else if (activeWaypointScript.previousWayPoint != null)
        { 
            SetNextPoint(activeWaypointScript.previousWayPoint);
            debugText = "Previous waypoint, " + nextWaypoint.name;
            patrollingForward = true;
        }

        //if there is somehow neither, go back to try and hit an alt path. This should NEVER happen, but who knows?
        else
            WaypointCheck1();
    }



    private void SetNextPoint(GameObject Waypoint)
    {
        previousWaypoint = activeWaypoint;
        nextWaypoint = Waypoint;
        motherMoverScript.SetMoveDestination(nextWaypoint);
    }

    public void SwapActiveWaypoint()
    {
        GameObject tempWaypoint = nextWaypoint;
        nextWaypoint = previousWaypoint;
        previousWaypoint = tempWaypoint;
        motherMoverScript.SetMoveDestination(nextWaypoint);
        debugText = "Stuck, turning around to " + nextWaypoint.name;
        patrollingForward = !patrollingForward;
    }
}
