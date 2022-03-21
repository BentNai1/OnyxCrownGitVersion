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
    


    void Start()
    {
        motherAIAndModel = this.gameObject.GetComponentInParent<Transform>();
        motherAIAndModelCC = this.gameObject.GetComponentInParent<CharacterController>();
        motherBrainScript = gameObject.GetComponent<MotherBrain>();
        motherRotater = this.gameObject.GetComponentInParent<MotherRotate>();

        lastFramePosition = motherAIAndModel.position;
    }


    void Update()
    {
        //only move if not directly on top of target
        if (!closeToTarget)
        {
            //if facing away, turn towards taget; otherwise, move towards target
            FindMoveDirection();
            targetRotationDegree = FindTargetRotation();

            if (targetRotationDegree >= faceAngleThreshhold)
            {
                RotateToTarget();
            }
            else
            {
                MoveToTarget();
                CheckMovingForward();
            }
        }
    }




    public void SetMoveDestination(GameObject destination)
    {
        targetWaypoint = destination;
        closeToTarget = false;
    }

    private void FindMoveDirection()
    {
        moveDirection = Vector3.Normalize(targetWaypoint.transform.position - motherAIAndModel.position);
        moveDirection.y = 0;
    }

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

    private void RotateToTarget()
    {
        float step = rotateSlowdown * Time.deltaTime;


        //if angle is getting bigger, flip direction
        if (previousFrameRotationDegree < targetRotationDegree)
            rotationInverterValue = rotationInverterValue * -1;
        

        float yRotate = Vector3.RotateTowards(motherAIAndModel.transform.up, targetWaypoint.transform.position, step, 0).y * rotationInverterValue;

        motherRotater.RotateMother(yRotate);

        previousFrameRotationDegree = targetRotationDegree;
    }

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
}
