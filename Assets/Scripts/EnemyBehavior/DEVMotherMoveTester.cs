using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEVMotherMoveTester : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject waypointToMoveTo;
    private MotherBrain motherBrainScript;
    private MotherMover motherMoverScript;
    void Start()
    {
        motherBrainScript = this.gameObject.GetComponent<MotherBrain>();
        motherMoverScript = this.gameObject.GetComponent<MotherMover>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waypointToMoveTo != null)
        {
            motherBrainScript.ChangeActiveWaypoint(waypointToMoveTo);
            motherMoverScript.SetMoveDestination(waypointToMoveTo);
            waypointToMoveTo = null;
        }
    }
}
