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
    public float rotateSpeed = 1;
    private float targetRotationDegree;

    void Start()
    {
        motherAIAndModel = this.gameObject.GetComponentInParent<Transform>();
        motherAIAndModelCC = this.gameObject.GetComponentInParent<CharacterController>();
        motherBrainScript = gameObject.GetComponent<MotherBrain>();
        motherRotater = this.gameObject.GetComponentInParent<MotherRotate>();
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
        float step = rotateSpeed * Time.deltaTime;

        float yRotate = Vector3.RotateTowards(motherAIAndModel.transform.up, targetWaypoint.transform.position, step, 0.0f).y;

        motherRotater.RotateMother(yRotate);

        //motherAIAndModel.Rotate(0, Vector3.RotateTowards(motherAIAndModel.transform.up, targetWaypoint.transform.position, step, 0.0f).y, 0);
    }

    private float FindTargetRotation()
    {
        float angleDif = Vector3.Angle(motherAIAndModel.transform.forward, moveDirection);
        return angleDif;
    }
}
