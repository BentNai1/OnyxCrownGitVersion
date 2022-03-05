using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchToHide_Script : MonoBehaviour
{
    public GameObject PlayerPivot;
    public PlayerMovement playerMoveScript;
    public GameObject boxCol;
    public bool hiding;

    public P_Animation playerAnimation;

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
            hiding = true;
        }

        //Update player scale to reflect 'crouching'.
        if (crouching)
        {
            //PlayerPivot.transform.localScale = new Vector3(1, 0.5f, 1);
            playerMoveScript.moveSpeed = playerSpeed * crouchSpeedMultiplier;

            playerAnimation.PlayerAnimation(P_Animation.playerAnimationState.crouch);
        }
        else
        {
            //PlayerPivot.transform.localScale = Vector3.one;
            playerMoveScript.moveSpeed = playerSpeed;

            playerAnimation.PlayerAnimation(P_Animation.playerAnimationState.uncrouch);
        }

        if (!crouching) hiding = false;
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
        if (hiding)
        {
            hiding = false;
            print("unhiding!");
        }

        if (levelCrouch == true)
        {
            if (other.tag == "crouch")
            {
                crouching = false;
            }
        }
    }
}
