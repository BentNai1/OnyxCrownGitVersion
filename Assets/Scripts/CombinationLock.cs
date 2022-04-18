using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationLock : MonoBehaviour
{
    [Header("- Lock Stuff")]

    [SerializeField] public GameObject gate, lockCam, combLock;
    [SerializeField] private int[] lockAnswer;

    private int[] result;

    [HideInInspector] public bool playerBusy = false;

    [Header("- Sound")]
    [SerializeField] private AudioSource lockSpeaker;
    [SerializeField] private AudioClip unlockSound;

    void Start()
    {
        lockCam.SetActive(false);

        result = new int[] { 9, 9, 9, 9};
        //lockAnswer = new int[] { 1, 1, 1, 1};
        CombinationRotate.Rotated += CheckResults;
    }

    /**Check Results will check each wheel on a switch function based on the wheel name and put it in the place holder int array "result"
     * and then will check that against the "answer" int array.**/
    private void CheckResults(string wheelname, int number)
    {
        switch (wheelname)
        {
            case "wheel1":
                result[0] = number;
                break;

            case "wheel2":
                result[1] = number;
                break;

            case "wheel3":
                result[2] = number;
                break;

            case "wheel4":
                result[3] = number;
                break;
        }

        if (result[0] == lockAnswer[0] && result[1] == lockAnswer[1] && result[2] == lockAnswer[2] && result[3] == lockAnswer[3])
        {
            Debug.Log("Gate Opened!");
            lockSpeaker.PlayOneShot(unlockSound);
            gate.SetActive(false);
            Destroy(combLock);
        }
    }

    /**Detecting the collision of the player with the trigger that is around the lock, which will activate the viewport on the canvas of the lock
     * for the player to interact with**/
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lockCam.SetActive(true);
            playerBusy = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lockCam.SetActive(false);
            playerBusy = false;
        }
    }

    private void OnDestroy()
    {
        CombinationRotate.Rotated -= CheckResults;
    }
}
