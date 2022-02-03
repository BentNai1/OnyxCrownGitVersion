using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointPlayerMovement : MonoBehaviour
{
    #region Variables
    [HideInInspector] public GameObject previousWayPoint;
    [SerializeField] private GameObject nextWaypoint;
    [HideInInspector] public WaypointPlayerMovement nextWaypointScript;

    [SerializeField] private float degreesRotateCameraNextWaypoint;
    [SerializeField] private float degreesRotateCameraPreviousWaypoint;

    [SerializeField] private float effectingDistance;

    private PlayerMovement playerMovementScript;

    //transforms passed to player movement to tell it where to vertCorrect, and where to rotate.
    [HideInInspector] public Transform virtualNextTransform;
    [HideInInspector] public Transform virtualPreviousTransform;
    [HideInInspector] public Transform virtualAltUpTransform;
    [HideInInspector] public Transform virtualAltDownTransform;

    private bool playerInTriggerVolume;

    [Header ("Alternative Routes")]
    [SerializeField] private GameObject altWayPointUp;
    [SerializeField] private GameObject altWayPointDown;
    private bool isStartingDownSidePath;
    private float timer;

    [SerializeField] private float degreesRotateCameraUpWaypoint;
    [SerializeField] private float degreesRotateCameraDownWaypoint;



    #endregion

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
    }

    private void OnDrawGizmosSelected()
    {
        Handles.Label(transform.position, this.gameObject.name);

        //get rotation of waypoint, and apply inspector-based rotation value to help with visualization
        
        //Vector3 nextCameraPosition

        //Gizmos.DrawWireSphere((Vector3.Lerp(transform.position, ), new Vector3(1, 1, 1));

    }


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
        }

        if (altWayPointDown != null)
        {
            virtualAltDownTransform.position = transform.position;
            virtualAltDownTransform.LookAt(altWayPointDown.transform.position);
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
    #region set rotation functions
    public void GoToForwardRotation()
    {
        if (virtualNextTransform != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualNextTransform);
            playerMovementScript.SetPreviousAndNextWaypoints(this.gameObject, nextWaypoint);
        }
    }

    public void GoToBackwardRotation()
    {
        if (virtualPreviousTransform != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualPreviousTransform);
            playerMovementScript.SetPreviousAndNextWaypoints(previousWayPoint, this.gameObject);
        }
    }

    public void GoToAltUpRotation()
    {
        if (virtualAltUpTransform != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualAltUpTransform);
            playerMovementScript.SetPreviousAndNextWaypoints(this.gameObject, altWayPointUp);
        }
    }

    public void GoToAltDownRotation()
    {
        if (virtualAltDownTransform != null)
        {
            playerMovementScript.SetRotationAndTargetCorrection(virtualAltDownTransform);
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
    }


    private void OnTriggerExit(Collider other)
    {
        #region Turn off Alt Path Prompts
        playerMovementScript.showUpPromptUI = false;
        playerMovementScript.showDownPromptUI = false;
        playerMovementScript.upPrompt.enabled = false;
        playerMovementScript.downPrompt.enabled = false;

        if(isStartingDownSidePath)
        {
            timer = 0.5f;
        }
        #endregion

        

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
