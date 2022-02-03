using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchToHide_Script : MonoBehaviour
{
    public GameObject PlayerPivot;
    public PlayerMovement Player;
    public GameObject boxCol;
    public bool hiding;

    public bool levelCrouch;
    public bool toggleCrouch;
    private bool crouching;
    private float playerSpeed;
    public float crouchSpeedMultiplier;
    

    private void Start()
    {
        playerSpeed = GetComponent<PlayerMovement>().moveSpeed;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (toggleCrouch)
            {
                crouching = !crouching;
            }
            else
            {
                crouching = true;
            }
        }

        if (Input.GetButtonUp("Crouch") && !toggleCrouch)
        {
            crouching = false;
        }
        
            if (boxCol.GetComponent<TriggerEnter>().colliding == true && crouching)
            {
                print("Hiding!");
                hiding = true;
            }
            if (boxCol.GetComponent<TriggerEnter>().colliding == false && hiding)
            {
                hiding = false;
            }

        //Update player scale to reflect 'crouching'. Replace this with animation changes.
        if (crouching)
        {
            PlayerPivot.transform.localScale = new Vector3(1, 0.5f, 1);
            GetComponent<PlayerMovement>().moveSpeed = playerSpeed * crouchSpeedMultiplier;
        }
        else
        {
            PlayerPivot.transform.localScale = Vector3.one;
            GetComponent<PlayerMovement>().moveSpeed = playerSpeed;
        }
    }

    //If levelCrouch is true, the level can force the character to crouch
    private void OnTriggerEnter(Collider other)
    {
        if (levelCrouch)
        {
            if (other.tag == "crouch")
            {
                crouching = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (levelCrouch == true)
        {
            if (other.tag == "crouch")
            {
                crouching = false;
            }
        }
    }
}
