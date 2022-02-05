using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================NOTE=====================
// The ground checker only looks at objects tagged
// with the "Ground" layer (to keep it from checking 
// itself). Any terrain you want the player to walk
// on should have the "Ground" layer on them.

public class PlayerMovement : MonoBehaviour
{

    #region visible Variables
    [Header("- General")]
    public CharacterController controller;
    public LayerMask groundMask;
    public Transform playerModel;
    public SpriteRenderer upPrompt;
    public SpriteRenderer downPrompt;

    [Header("- Movement")]
    public float moveSpeed;
    public float now;
    public float since;

    [Tooltip("Check to enable moving towards or away from the camera; uncheck to enforce Z axis movement.")]
    public bool verticalMovement;

    [Header("- Fall/Jump")]
    [Tooltip("Recommend -9.81")]
    public float gravity = -9.81f;
    [Tooltip("Recommend 0.4")]
    public float groundCheckDistance;
    [Tooltip("The ground checker only looks at objects tagged with the 'Ground' layer (to keep it from checking itself). Any terrain you want the player to walk on should have the 'Ground' layer on them.")]
    public Transform groundCheck;

    public float jumpStrength;

    public AudioSource JumpSound;

    [Header("- Abilities")]
    public Dash dashScript;

    
    #endregion

    #region hidden Variables
    private bool vertMovementWasActive = true;
    private float vertCorrectionTarget;

    private float xAxisInput;
    private float yAxisInput;

    private enum inputRotationAngle
    {
        DownDefaultCamera, RightCamera, OpositeCamera, LeftCamera
    }

    private bool rotationEventHappened;

    private inputRotationAngle lastUsedAngle;

    [HideInInspector]
    public bool isGrabbed;

    Vector3 fallVelocity;

    [HideInInspector]
    public bool isGrounded;

    private Quaternion modelRotation;

    private GameObject playerPlusCamera;

    private Quaternion targetRotation;

    [HideInInspector] public bool verticalCorrectionHappening;

    [HideInInspector] public GameObject forwardWaypoint;

    [HideInInspector] public GameObject backwardWaypoint;

    [HideInInspector] public bool showUpPromptUI;

    [HideInInspector] public bool showDownPromptUI;

    [HideInInspector] public WaypointPlayerMovement currentActiveWaypoint;

    [HideInInspector] public float degreesRotatedFromOrigin = 0;

    private CameraRotationController cameraRotationController;
    #endregion

    private void Start()
    {
        playerPlusCamera = transform.parent.gameObject;

        cameraRotationController = GetComponentInChildren<CameraRotationController>();
    }


    void Update()
    {

        //..............................................Ground check/Fall velocity reset
        #region Grounding
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);
        //resets fall velocity to a small number if close
        if (isGrounded && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f;
        }
        #endregion

        //..............................................Get input

        //if a rotation event happens, lock control scheme until player lets off of the input
        if (rotationEventHappened)
        {
            if(Mathf.Abs(xAxisInput) >= 0.2 || Mathf.Abs(yAxisInput) >= 0.2)
            {
                GetInputCameraAngle(lastUsedAngle);
            }
            else
            {
                rotationEventHappened = false;
            }
        }
        else
        {
            //input changes based on cinemachine rotation
            if (cameraRotationController.currentRotationFromOrigin > 45 && cameraRotationController.currentRotationFromOrigin <= 135)
            {
                GetInputCameraAngle(inputRotationAngle.RightCamera);
            }
            else if (cameraRotationController.currentRotationFromOrigin > 135 && cameraRotationController.currentRotationFromOrigin <= 225)
            {
                GetInputCameraAngle(inputRotationAngle.OpositeCamera);
            }
            else if (cameraRotationController.currentRotationFromOrigin > 225 && cameraRotationController.currentRotationFromOrigin <= 315)
            {
                GetInputCameraAngle(inputRotationAngle.LeftCamera);
            }
            else
            {
                GetInputCameraAngle(inputRotationAngle.DownDefaultCamera);
            }
        }

        float mathY = 0f;

        //..............................................Vertical Movement
        #region Vertical Movement
        //..............................................Get vertMove Change
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Vertical correction changed - THIS IS A DEVELOPMENT FEATURE, NOT FOR MAIN GAME.");
            verticalMovement = !verticalMovement;
        }

        //..............................................Check and replace(?) vertical movement

        if (verticalMovement == false)
        {
            //on start or on change, store current x coordinate for vertcorrector
            if (vertMovementWasActive == true)
            {
                vertMovementWasActive = false;
                vertCorrectionTarget = transform.localPosition.x;
            }
            //change y to match direction of saved direction
            float preClampY = transform.localPosition.x - vertCorrectionTarget;
            //preClampY = Mathf.SmoothStep(preClampY, vertCorrectionTarget, 1);
            mathY = Mathf.Clamp(preClampY, -1, 1);

            //set bool that indicates major vertical correction happened - relevant for outside scripts
            if (Mathf.Abs(mathY) > 0.1) verticalCorrectionHappening = true;
            else verticalCorrectionHappening = false;
        }
        //track vertMove change
        else if (vertMovementWasActive == false)
        {
            vertMovementWasActive = true;
        }

        #endregion


        //..............................................Move player (If not grabbed)
        #region Move Player

        //If player is not grabbed they can move, and if they are grabbed disable movement
        if (isGrabbed == false)
        {
            //creates a location to move to relative to 
            //direction player is facing

            Vector3 newLocation = transform.forward *  xAxisInput  + transform.right * mathY * -1;

            Debug.DrawLine(transform.position, (newLocation*2 + transform.position), Color.cyan);

            controller.Move(newLocation * moveSpeed * Time.deltaTime);

            //..............................................Rotate model
            //only rotate if movement happened
            if ( xAxisInput  > .01 || mathY > .01 ||  xAxisInput  < -.01 || mathY < -.01)
            {
                modelRotation.SetLookRotation(newLocation);
                playerModel.rotation = modelRotation;
            }

            //..............................................Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
                if (JumpSound != null) //check for jump sound
                {
                    JumpSound.Play();
                }
            }
            #endregion
            //..............................................Waypoint boounds check
            #region Waypoint Bounds
            //Vector3 localPreviousTransform = backwardWaypoint.transform.InverseTransformPoint(transform.position);



            Vector3 forward = transform.TransformDirection(Vector3.forward);

            //find directional magnitude of back waypoint
            if (backwardWaypoint != null)
            {
                Vector3 toOther = backwardWaypoint.transform.position - transform.position;

                float result = Vector3.Dot(forward, toOther);

                //Change player rotation if player backing up on waypoint without fully leaving
                if (result >= 0.5 &&  xAxisInput  <= -0.1)
                {
                    WaypointPlayerMovement waypointScript = backwardWaypoint.GetComponent<WaypointPlayerMovement>();
                    waypointScript.GoToBackwardRotation();
                    Debug.Log("Re-oriented towards previous waypoint, " + backwardWaypoint.name + " at " + forwardWaypoint.name);
                }
            }

            //find directional magnitude of forward waypoint
            if (forwardWaypoint != null)
            {
                Vector3 toOther = forwardWaypoint.transform.position - transform.position;

                float result = Vector3.Dot(forward, toOther);

                //Change player rotation if player backing up on waypoint without fully leaving
                if (result <= -0.5 &&  xAxisInput  >= 0.1)
                {
                    WaypointPlayerMovement waypointScript = forwardWaypoint.GetComponent<WaypointPlayerMovement>();
                    waypointScript.GoToForwardRotation();
                    Debug.Log("Re-orienting towards next waypoint, " + forwardWaypoint.name + " at " + backwardWaypoint);
                }
            }
            #endregion

            //..............................................Alt Paths
            #region Alt Paths
            if(showUpPromptUI && yAxisInput > 0.2)
            {
                currentActiveWaypoint.GoToAltUpRotation();
                upPrompt.enabled = false;
                downPrompt.enabled = false;
            }

            if(showDownPromptUI && yAxisInput < -0.2)
            {
                currentActiveWaypoint.GoToAltDownRotation();
                upPrompt.enabled = false;
                downPrompt.enabled = false;
            }
            #endregion
        }




        //..............................................Dash (Depriciated)
        #region Dash (Depreciated)
        /**if (Input.GetButtonDown("Fire3"))
        {
            dashScript.DashInDirection(controller, modelRotation);
        }**/
        #endregion

        //..............................................Player fall velocity
        #region Falling
        fallVelocity.y = fallVelocity.y + gravity * Time.deltaTime;
        controller.Move(fallVelocity * Time.deltaTime);
        #endregion
    }

    public void Jump()
    {
        fallVelocity.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
    }

    private void RotateCamera(float rotateLeftDegreeASec)
    {
        playerPlusCamera.transform.RotateAround(this.transform.position, Vector3.up, rotateLeftDegreeASec * Time.deltaTime);
    }

    public void SetRotationAndTargetCorrection(Transform targetWaypointTransform, float degreesRotateTo = 0)
    {

        //rotation

        float calculatedRotation = targetWaypointTransform.rotation.eulerAngles.y - playerPlusCamera.transform.rotation.eulerAngles.y;

        playerPlusCamera.transform.Rotate(playerPlusCamera.transform.rotation.eulerAngles.x, calculatedRotation, playerPlusCamera.transform.rotation.eulerAngles.z, Space.World);

        //get relative position

        Vector3 relativeWaypoint = playerPlusCamera.transform.InverseTransformPoint(targetWaypointTransform.position);

        //update vert correction target

        vertCorrectionTarget = relativeWaypoint.x;

        //rotate cincemachine camera to specified point

        cameraRotationController.RotateClockwiseToPointFromStartRotation(degreesRotateTo);

        //lock movement type until player lets off input

        rotationEventHappened = true;

    }

    public void SetPreviousAndNextWaypoints(GameObject recievedPreviousWaypoint, GameObject recievedNextWaypoint)
    {
        backwardWaypoint = recievedPreviousWaypoint;
        forwardWaypoint = recievedNextWaypoint;
    }

    #region getinputs

    private void GetInputCameraAngle(inputRotationAngle cameraAngle = inputRotationAngle.DownDefaultCamera)
    {
        if(cameraAngle == inputRotationAngle.RightCamera)
        {
            yAxisInput = Input.GetAxis("Horizontal");
            xAxisInput = Input.GetAxis("Vertical");
            lastUsedAngle = inputRotationAngle.RightCamera;
        }
        if (cameraAngle == inputRotationAngle.OpositeCamera)
        {
            xAxisInput = Input.GetAxis("Horizontal") * -1;
            yAxisInput = Input.GetAxis("Vertical") * -1;
            lastUsedAngle = inputRotationAngle.OpositeCamera;
        }
        if (cameraAngle == inputRotationAngle.LeftCamera)
        {
            yAxisInput = Input.GetAxis("Horizontal") * -1;
            xAxisInput = Input.GetAxis("Vertical") * -1;
            lastUsedAngle = inputRotationAngle.LeftCamera;
        }
        if (cameraAngle == inputRotationAngle.DownDefaultCamera)
        {
            xAxisInput = Input.GetAxis("Horizontal");
            yAxisInput = Input.GetAxis("Vertical");
            lastUsedAngle = inputRotationAngle.DownDefaultCamera;
        }

    }

    #endregion
}
