using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]

public class CameraRotationController : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachingVirtualCamera;

    private CinemachineBrain cinemachingBrain;

    private CinemachineOrbitalTransposer orbitalTransposer;

    private float startingRotation;

    [HideInInspector]
    public float currentRotationFromOrigin;


    void Start()
    {
        cinemachingBrain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        if (cinemachingBrain == null)
        {
            cinemachingBrain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }

        cinemachingVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();

        orbitalTransposer = cinemachingVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        startingRotation = orbitalTransposer.m_XAxis.Value;
        print("Starting player Cinemachine rotation value: " + startingRotation);
    }

    public void RotateClockwiseAmountOfDegrees(float DegreesToRotate)
    {
        orbitalTransposer.m_XAxis.Value += DegreesToRotate;
        setPublicRotationValue();
    }

    public void RotateClockwiseToPointFromStartRotation(float DegreesToRotateTo)
    {
        orbitalTransposer.m_XAxis.Value = startingRotation + DegreesToRotateTo;
        setPublicRotationValue();
    }

    private void setPublicRotationValue()
    {
        //get rotation normalized 0-360 degrees
        currentRotationFromOrigin = ((((orbitalTransposer.m_XAxis.Value + 360) % 360)-((startingRotation + 360) % 360)) + 360) % 360;
        print("Current rotation from origin: " + currentRotationFromOrigin);
    }
}
