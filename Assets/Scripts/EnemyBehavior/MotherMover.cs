using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherMover : MonoBehaviour
{
    public float baseMovementSpeed = 1;
    public float closeEnoughThreshhold = 1;
    private GameObject targetWaypoint;
    private Transform motherAIAndModel;
    private CharacterController motherAIAndModelCC;
    private MotherRotate motherRotater;
    [HideInInspector] public bool closeToTarget = true;
    private MotherBrain motherBrainScript;
    private Vector3 playerPosition;

    private Vector3 moveDirection;
    [Tooltip ("AI will turn towards target if outside of this threshhold")]
    public float faceAngleThreshhold = 1;
    public float rotateSlowdown = 1;
    
    private float targetRotationDegree;
    private float previousFrameRotationDegree;
    private float rotationInverterValue = 1;

    [Header ("Unstuck")]
    public float minimumMovementThreshhold = 1;
    [Tooltip("Number of frames AI can move less than threshhold before turning around")]
    public int minMoveInfractionLimit = 1;
    private int minMoveInfractionCount;
    private Vector3 lastFramePosition;

    [Header ("Lunge")]
    public float fastRotateSlowdown = 1;
    private bool lungeTurning;
    private bool lunging;
    private bool lungeWindUp;
    public float lungeWindUpDuration = 1;
    public float lungeSpeed = 1;
    public float lungeDuration = 1;
    private float lungeTimer;



    void Start()
    {
        motherAIAndModel = this.gameObject.GetComponentInParent<Transform>();
        motherAIAndModelCC = this.gameObject.GetComponentInParent<CharacterController>();
        motherBrainScript = gameObject.GetComponent<MotherBrain>();
        motherRotater = this.gameObject.GetComponentInParent<MotherRotate>();

        lastFramePosition = motherAIAndModel.position;
    }

    //rotate/move towards target
    void Update()
    {
        //only move if not directly on top of target
        if (!closeToTarget && !lunging)
        {
            //if facing away, turn towards taget; otherwise, move towards target
            FindMoveDirection(targetWaypoint.transform.position);
            targetRotationDegree = FindTargetRotation();

            if (targetRotationDegree >= faceAngleThreshhold)
            {
                RotateToTarget(rotateSlowdown, targetWaypoint.transform.position);
            }
            else
            {
                MoveToTarget();
                CheckMovingForward();
            }
        }

        //lunging replaces movement
        if (lunging)
        {
            FastTurnThenLunge();
        }
    }



    //outside scripts can set target for this to rotate/move towards
    public void SetMoveDestination(GameObject destination)
    {
        targetWaypoint = destination;
        closeToTarget = false;
    }

    //create a vector3 for where the ai needs to rotate/move to
    private void FindMoveDirection(Vector3 targetLocation)
    {
        moveDirection = Vector3.Normalize(targetLocation - motherAIAndModel.position);
        moveDirection.y = 0;
    }

    //move ai towards target
    private void MoveToTarget()
    {

        float step = baseMovementSpeed * Time.deltaTime;

        motherAIAndModelCC.Move(moveDirection * baseMovementSpeed);


        //turn off movement when close enough to waypoint
        if(Mathf.Abs(motherAIAndModel.position.x - targetWaypoint.transform.position.x) <= closeEnoughThreshhold && Mathf.Abs(motherAIAndModel.position.z - targetWaypoint.transform.position.z) <= closeEnoughThreshhold)
        {
            closeToTarget = true;
            motherBrainScript.ChangeActiveWaypoint(targetWaypoint);
            motherBrainScript.DecideNextWaypoint();

        }
    }

    //rotate ai towards target
    private void RotateToTarget(float slowdown, Vector3 targetPosition)
    {
        float step = slowdown * Time.deltaTime;


        //if angle is getting bigger, flip direction
        if (previousFrameRotationDegree < targetRotationDegree)
            rotationInverterValue = rotationInverterValue * -1;
        

        float yRotate = Vector3.RotateTowards(motherAIAndModel.transform.forward, targetPosition, step, 10000).y * rotationInverterValue;

        Debug.Log(step);
        Debug.Log(yRotate);

        motherRotater.RotateMother(yRotate);

        previousFrameRotationDegree = targetRotationDegree;
    }

    //calculate difference in angle between where ai is facing and where it needs to be facing
    private float FindTargetRotation()
    {
        float angleDif = Vector3.Angle(motherAIAndModel.transform.forward, moveDirection);
        return angleDif;
    }

    //Ensures movement is working, if block is detected try to return to previous waypoint
    private void CheckMovingForward()
    {
        //find distance moved from last frame
        float distanceMoved = Vector3.Distance(lastFramePosition, motherAIAndModel.position);

        //if below threshhold, move infraction var up; once high enough, swap waypoints.

        if (distanceMoved < minimumMovementThreshhold)
            minMoveInfractionCount++;

        if (minMoveInfractionCount >= minMoveInfractionLimit)
        {
            minMoveInfractionCount = 0;
            motherBrainScript.SwapActiveWaypoint();
        }

        //save this frame's position for next frame
        lastFramePosition = motherAIAndModel.position;
    }

    //Outside callable function to have AI lunge at a position
    public void ActivateLunge(Vector3 player)
    {
        if (!lunging)
        {
            lunging = true;
            lungeWindUp = true;
            lungeTimer = lungeWindUpDuration;
            playerPosition = player;
            FindMoveDirection(playerPosition);
        }
    }


    private void FastTurnThenLunge()
    {
        targetRotationDegree = FindTargetRotation();

        if (targetRotationDegree >= faceAngleThreshhold)
        {
            RotateToTarget(fastRotateSlowdown, targetWaypoint.transform.position);
        }
        else
        {
            Lunge();
        }
    }

    private void Lunge()
    {
        //pause for windup
        if(lungeWindUp && lungeTimer >= 0)
        {
            lungeTimer -= Time.deltaTime;
        }
        else if (lungeWindUp)
        {
            lungeWindUp = false;
            lungeTimer = lungeDuration;
        }

        //the lunge
        else if(!lungeWindUp && lungeTimer >= 0)
        {
            float step = lungeSpeed * Time.deltaTime;
            motherAIAndModelCC.Move(moveDirection * lungeSpeed);

            lungeTimer -= Time.deltaTime;
        }

        //turn off lunging, tell brain to pause and then carry on
        else
        {
            lunging = false;
            motherBrainScript.PauseMovement();
        }
    }
}
