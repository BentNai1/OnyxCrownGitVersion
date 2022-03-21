using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointPlayerMovement : MonoBehaviour
{
    #region Variables
    [HideInInspector] public GameObject previousWayPoint;
    public GameObject nextWaypoint;
    [HideInInspector] public WaypointPlayerMovement nextWaypointScript;

    [SerializeField] private float degreesRotateCameraNextWaypoint;
    [Tooltip ("A value of 0 defaults to 1")] [SerializeField] private float rotateSpeedNext;
    [SerializeField] private float degreesRotateCameraPreviousWaypoint;
    [Tooltip("A value of 0 defaults to 1")] [SerializeField] private float rotateSpeedPrevious;

    [SerializeField] private float effectingDistance;

    private PlayerMovement playerMovementScript;

    //transforms passed to player movement to tell it where to vertCorrect, and where to rotate.
    [HideInInspector] public Transform virtualNextTransform;
    [HideInInspector] public Transform virtualPreviousTransform;
    [HideInInspector] public Transform virtualAltUpTransform;
    [HideInInspector] public Transform virtualAltDownTransform;

    private bool playerInTriggerVolume;

    [Header ("Alternative Routes")]
    public GameObject altWayPointUp;
    public GameObject altWayPointDown;
    private bool isStartingDownSidePath;
    private float timer;

    [SerializeField] private float degreesRotateCameraUpWaypoint;
    [Tooltip("A value of 0 defaults to 1")] [SerializeField] private float rotateSpeedUp;
    [SerializeField] private float degreesRotateCameraDownWaypoint;
    [Tooltip("A value of 0 defaults to 1")] [SerializeField] private float rotateSpeedDown;

    [Header("Debug")]
    public bool printoutHiddenVariables;



    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (nextWaypoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
        }

        if (altWayPointUp != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, altWayPointUp.transform.position);
        }

        if (altWayPointDown != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, altWayPointDown.transform.position);
        }

        if (nextWaypoint == null)
            Gizmos.DrawIcon(transform.position, "StopIcon.png", true);
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmo labels; may cause build problems, comment out if needed

        Handles.Label(transform.position, this.gameObject.name);

    }

#endif

    void Start()
    {
        //calculate turn rate of points (future feature)

        //create virtual self transforms to be passed to player movement script
        #region Create Virtual Transforms

        if (nextWaypoint !=  null)
        {
            virtualNextTransform.position = transform.position;
            virtualNextTransform.LookAt(nextWaypoint.transform.position);

            //give data about self to next script, including virtual transforms
            nextWaypointScript = nextWaypoint.GetComponent<WaypointPlayerMovement>();
            nextWaypointScript.previousWayPoint = this.gameObject;
            //nextWaypointScript.virtualPreviousTransform.position = this.transform.position;

            nextWaypointScript.virtualPreviousTransform.LookAt(transform.position);

            //flip so camera stays on main side
            nextWaypointScript.virtualPreviousTransform.RotateAround(nextWaypointScript.transform.position, Vector3.up, 180);


        }


        //also for alt paths
        if (altWayPointUp != null)
        {
            virtualAltUpTransform.position = transform.position;
            virtualAltUpTransform.LookAt(altWayPointUp.transform.position);

            //if alt waypoint has no regular prev waypoint, make this it
            WaypointPlayerMovement upPointScript = altWayPointUp.GetComponent<WaypointPlayerMovement>();
            if (upPointScript.previousWayPoint == null)
            {
                upPointScript.previousWayPoint = this.gameObject;
            }
        }

        if (altWayPointDown != null)
        {
            virtualAltDownTransform.position = transform.position;
            virtualAltDownTransform.LookAt(altWayPointDown.transform.position);

            //if alt waypoint has no regular prev waypoint, make this it
            WaypointPlayerMovement downPointScript = altWayPointDown.GetComponent<WaypointPlayerMovement>();
            if (downPointScript.previousWayPoint == null)
            {
                downPointScript.previousWayPoint = this.gameObject;
            }
        }

        //debugging virtuals
        if(nextWaypoint != null && altWayPointUp != null && altWayPointDown != null)
        {
            Debug.Log("Next: " + virtualNextTransform.rotation.eulerAngles.y + " | Alt Up: " + virtualAltUpTransform.rotation.eulerAngles.y + " | Alt down: " + virtualAltDownTransform.rotation.eulerAngles.y);
        }
        #endregion
    }

    private void FindNextVirtualRotation(Transform thingToLookAt)
    {
        virtualNextTransform = transform;
        virtualNextTransform.LookAt(thingToLookAt.transform.position);
    }



    private void OnTriggerEnter(Collider other)
    {
        
        //check for player object, run if not on timer
        if (other.GetComponent<PlayerMovement>() != null && timer <= 0)
        {
            //on first collision per waypoint, get players movementent script
            #region First Collision
            if (playerMovementScript == null)
            {
                playerMovementScript = other.GetComponent<PlayerMovement>();
                Debug.Log("First player collission at " + this.name);
            }

            //on first player collission with a waypoint
            if(playerMovementScript.forwardWaypoint == null)
            {
                GoToForwardRotation();
            }
            #endregion

            //on every collision, check that vertical correction is not happening, and that this is set as backwards waypoint

            /*

            #region Main Path forward/backward
            if (playerMovementScript.verticalCorrectionHappening == false && playerMovementScript.backwardWaypoint == this.gameObject)
            {
                GoToBackwardRotation();
                Debug.Log("Go to previous rotation (" + virtualPreviousTransform.rotation.eulerAngles.y  + ") - player is now: " + other.transform.rotation.eulerAngles.y + ", was matching rotation: " + virtualNextTransform.rotation.eulerAngles.y);
            }


            //as long as vert correction isn't happening and this is set as forward waypoint
            else if (playerMovementScript.verticalCorrectionHappening == false && playerMovementScript.forwardWaypoint == this.gameObject)
            {
                GoToForwardRotation();
                Debug.Log("Go to next rotation(" + virtualNextTransform.rotation.eulerAngles.y + ") - player is now: " + other.transform.rotation.eulerAngles.y);
            }

            #endregion

            */

            //Show player button prompts if option for alt route exists
            #region Alt Paths
            if (altWayPointUp != null && !isStartingDownSidePath)
            {
                playerMovementScript.showUpPromptUI = true;
                playerMovementScript.currentActiveWaypoint = this;
                playerMovementScript.upPrompt.enabled = true;
            }

            if(altWayPointDown != null && !isStartingDownSidePath)
            {
                playerMovementScript.showDownPromptUI = true;
                playerMovementScript.currentActiveWaypoint = this;
                playerMovementScript.downPrompt.enabled = true;
            }
            #endregion

            

            timer = 0.5f;

        }
        //pass calculated turn rate (future feature)
        //turn on rotating on player (future feature)
    }

    //set player rotation and tell player what it's current and next waypoints are
    //for forward/backward, check that this isn't an end of line, if it is, prevent player from going past.
    #region set rotation functions
    public void GoToForwardRotation()
    {
        if (virtualNextTransform != null && nextWaypoint != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualNextTransform, degreesRotateCameraNextWaypoint, rotateSpeedNext);
            playerMovementScript.SetPreviousAndNextWaypoints(this.gameObject, nextWaypoint);
        }

        if (nextWaypoint == null)
            playerMovementScript.PreventMovement(PlayerMovement.mainPathDirection.forward, true);

    }

    public void GoToBackwardRotation()
    {
        if (virtualPreviousTransform != null && previousWayPoint != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualPreviousTransform, degreesRotateCameraPreviousWaypoint, rotateSpeedPrevious);
            playerMovementScript.SetPreviousAndNextWaypoints(previousWayPoint, this.gameObject);
        }

        if (previousWayPoint == null)
            playerMovementScript.PreventMovement(PlayerMovement.mainPathDirection.backward, true);
    }

    public void GoToAltUpRotation()
    {
        if (virtualAltUpTransform != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualAltUpTransform, degreesRotateCameraUpWaypoint, rotateSpeedUp);
            playerMovementScript.SetPreviousAndNextWaypoints(this.gameObject, altWayPointUp);
        }
    }

    public void GoToAltDownRotation()
    {
        if (virtualAltDownTransform != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualAltDownTransform, degreesRotateCameraDownWaypoint, rotateSpeedDown);
            playerMovementScript.SetPreviousAndNextWaypoints(this.gameObject, altWayPointDown);
        }
    }

    #endregion

    private void Update()
    {
        #region Timer
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        #endregion

        if (printoutHiddenVariables == true)
        {
            print(previousWayPoint.ToString() + previousWayPoint.name + "___");
            printoutHiddenVariables = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        #region Turn off Alt Path Prompts
        if (other.tag == "Player")
        {
            playerMovementScript.showUpPromptUI = false;
            playerMovementScript.showDownPromptUI = false;
            playerMovementScript.upPrompt.enabled = false;
            playerMovementScript.downPrompt.enabled = false;

            if (isStartingDownSidePath)
            {
                timer = 0.5f;
            }
        }
        #endregion

        //if end of a line, return full player movement
        if (nextWaypoint == null || previousWayPoint == null)
            playerMovementScript.PreventMovement(PlayerMovement.mainPathDirection.forward, false);
        

        //turn off rotating (future feature)
        //PassNewVertCorrection(other); (future feature)
    }

    public float PassNewVertCorrection()
    {
        //(future feature)
        //set player to exact rotation (just in case they under/over shot)
        //calculate new vert correction
        //pass new vert correction
        return 0f;
    }
}
