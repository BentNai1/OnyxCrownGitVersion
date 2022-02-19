using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Teleporter : MonoBehaviour
{
    public GameObject teleportTarget;
    public GameObject thePlayer;
    // public GameObject newPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform playerTransform = thePlayer.GetComponentInChildren<Transform>();
            // playerTransform.transform.position = teleportTarget.transform.position;
            Vector3 targetRelative = teleportTarget.transform.InverseTransformPoint(playerTransform.position);
            Vector3 playerTarget = playerTransform.transform.position + targetRelative;
            playerTransform.position = playerTarget;
            // thePlayer.transform.position = worldCorrectedPlayerTransformThing;

            // Destroy(thePlayer);
            // Debug.Log("Is destroyed");
            // Instantiate(newPlayer, new Vector3(0f,0f,0f), Quaternion.identity);
            // Debug.Log("New player made");
            // newPlayer.transform.position = new Vector3(teleportTarget.transform.position.x, teleportTarget.transform.position.y, teleportTarget.transform.position.z);
            // Debug.Log("Teleported");
        }
    }
}
