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
    [HideInInspector] public bool closeToTarget = true;
    private MotherBrain motherBrainScript;

    void Start()
    {
        motherAIAndModel = this.gameObject.GetComponentInParent<Transform>();
        motherAIAndModelCC = this.gameObject.GetComponentInParent<CharacterController>();
        motherBrainScript = gameObject.GetComponent<MotherBrain>();
    }


    void Update()
    {
        if(!closeToTarget) MoveToTarget();


    }

    public void SetMoveDestination(GameObject destination)
    {
        targetWaypoint = destination;
        closeToTarget = false;
    }

    private void MoveToTarget()
    {
        float step = baseMovementSpeed * Time.deltaTime;

        Vector3 moveDirection = Vector3.Normalize(targetWaypoint.transform.position - this.transform.position);
        moveDirection.y = 0;

        motherAIAndModelCC.Move(moveDirection * baseMovementSpeed);


        //turn off movement when close enough to waypoint
        if(Mathf.Abs(motherAIAndModel.position.x - targetWaypoint.transform.position.x) <= closeEnoughThreshhold && Mathf.Abs(motherAIAndModel.position.z - targetWaypoint.transform.position.z) <= closeEnoughThreshhold)
        {
            closeToTarget = true;
            motherBrainScript.ChangeActiveWaypoint(targetWaypoint);
            motherBrainScript.DecideNextWaypoint();

        }
    }
}
