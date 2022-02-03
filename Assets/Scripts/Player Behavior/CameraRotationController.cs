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


    // Start is called before the first frame update
    void Start()
    {
        cinemachingBrain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        if (cinemachingBrain == null)
        {
            cinemachingBrain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
        }

        cinemachingVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();

        orbitalTransposer = cinemachingVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateClockwiseAmountOfDegrees(float DegreesToRotate)
    {
        orbitalTransposer.m_XAxis.Value += DegreesToRotate;
    }
}
