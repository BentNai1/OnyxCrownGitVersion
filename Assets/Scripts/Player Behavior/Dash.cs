using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField]
    private float dashDistance;
    private Quaternion controllerQuaternion;
    public Transform quaternionCorrector;
    private Vector3 newPos;

    public void DashInDirection(CharacterController playerTransform, Quaternion currentDirection)
    {
        //calculate location to teleport to based on current location and rotation
        if(quaternionCorrector == null)
        {
            newPos = currentDirection * transform.forward * dashDistance;
        }

        //By using the rotation instead of the passed Quaternion, dash seems to work regardless of rotated camera position
        else
        {
            newPos = quaternionCorrector.rotation * transform.forward * dashDistance;
        }

        //teleport
        playerTransform.Move(newPos);
    }
}
