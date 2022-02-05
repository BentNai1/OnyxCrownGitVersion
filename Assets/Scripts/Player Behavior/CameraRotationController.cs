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

    private float targetRotation;
    private float rotationSpeed;

    private bool rotating;

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

    void Update()
    {
        if (rotating == true)
        {
            RotationIncrement();
        }
    }

    public void RotateClockwiseToPointFromStartRotation(float DegreesToRotateTo, float rotateSpeedMultiplier = 1)
    {
        //orbitalTransposer.m_XAxis.Value = startingRotation + DegreesToRotateTo;
        //setPublicRotationValue();
        if(rotateSpeedMultiplier == 0)
        {
            rotateSpeedMultiplier = 1;
        }
        targetRotation = startingRotation + DegreesToRotateTo;
        rotationSpeed = rotateSpeedMultiplier;
        rotating = true;
    }

    private void RotationIncrement()
    {
        orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, targetRotation, rotationSpeed * Time.deltaTime);
        setPublicRotationValue();
    }

    private void setPublicRotationValue()
    {
        //get rotation normalized 0-360 degrees
        currentRotationFromOrigin = ((((orbitalTransposer.m_XAxis.Value + 360) % 360)-((startingRotation + 360) % 360)) + 360) % 360;
    }
}
