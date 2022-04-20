using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationLock : MonoBehaviour
{
    [Header("- Lock Stuff")]

    [SerializeField] public GameObject gate, lockCam, combLock, AbilitySelector;
    [SerializeField] private int[] lockAnswer;

    private int[] result;

    [HideInInspector] public bool playerBusy = false;
    [HideInInspector] public PlayerMovement player;

    [Header("- Sound")]
    [SerializeField] private AudioSource lockSpeaker;
    [SerializeField] private AudioClip unlockSound;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Start()
    {
        lockCam.SetActive(false);

        result = new int[] { 9, 9, 9, 9};
        //lockAnswer = new int[] { 1, 1, 1, 1};
        CombinationRotate.Rotated += CheckResults;

        AbilitySelector = GameObject.Find("AbilityWheel");
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
            AbilitySelector.SetActive(true);
            Destroy(combLock);
        }
    }

    //This is for the player to exit the combination lock interface through button press
    private void Update()
    {
        if (playerBusy)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                player.isGrabbed = false;
                lockCam.SetActive(false);
                playerBusy = false;
                AbilitySelector.SetActive(true);
                PlayerMovement.playerBusy = false;
            }
        }
    }

    /**Detecting the collision of the player with the trigger that is around the lock, which will activate the viewport on the canvas of the lock
     * for the player to interact with**/
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.isGrabbed = true;
            lockCam.SetActive(true);
            playerBusy = true;
            AbilitySelector.SetActive(false);
            PlayerMovement.playerBusy = true;
        }
    }

    private void OnDestroy()
    {
        CombinationRotate.Rotated -= CheckResults;
    }
}
