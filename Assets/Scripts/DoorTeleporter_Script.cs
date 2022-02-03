using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTeleporter_Script : MonoBehaviour
{
    public GameObject Player;

    private bool inBounds = false;
    public Text promptObject;
    public string dest1Name = "[Input Destination 1 Name]";
    public string dest2Name = "[Input Destination 2 Name]";
    public Transform Dest1;
    public Transform Dest2;

    private void Update()
    {
        if (inBounds && Input.GetKeyDown("w"))
        {
            print("teleport! to 1");
            Player.transform.position = Dest1.transform.position;
        }
        if (inBounds && Input.GetKeyDown("s"))
        {
            print("teleport! to 2");
            Player.transform.position = Dest2.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inBounds = true;
            promptObject.text = "Press 'W' to go to " + dest1Name + ".\nPress 'S' to go to " + dest2Name + ".";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inBounds = false;
            promptObject.text = "";
        }
    }
}
