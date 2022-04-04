using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrouchToHide_Script : MonoBehaviour
{
    public GameObject PlayerPivot;
    public PlayerMovement playerMoveScript;
    public GameObject boxCol;
    public bool hiding = false;
    private bool hideDebug = false;

    public P_Animation playerAnimation;
    
    public bool toggleCrouch;
    private bool crouching;
    private float playerSpeed;
    public float crouchSpeedMultiplier;
    public GameObject Eye;
    public Sprite eyeOpen;
    public Sprite eyeClose;

    private bool debugCollide;
    

    private void Start()
    {
        playerSpeed = GetComponent<PlayerMovement>().moveSpeed;
        debugCollide = false;
    }

    private void Update()
    {
        //Get collision status from boxCol
        if (boxCol.GetComponent<TriggerEnter>().colliding == true)
        {
            if (Input.GetButtonDown("Crouch"))
            {
                crouching = !crouching;
            }
            if (crouching)
            {
                hiding = true;
                // closed eye goes here
            }
            else
            {
                hiding = false;
            }
        }
        else
        {
            // remove eye
            crouching = false;
        }

        //Animation & speed
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

            hiding = false;
        }

        if (debugCollide !=boxCol.GetComponent<TriggerEnter>().colliding)
        {
            if(Eye != null)
            {
                if (boxCol.GetComponent<TriggerEnter>().colliding)
                {
                    Eye.GetComponent<Image>().enabled = true;
                    Eye.GetComponent<Image>().sprite = eyeOpen;
                }
                else
                {
                    Eye.GetComponent<Image>().enabled = false;
                }
            }
            debugCollide = boxCol.GetComponent<TriggerEnter>().colliding;
        }
        //DEBUG HIDING MESSAGES
        if (hideDebug != hiding)
        {
            
            if (hiding)
            {
                if (Eye != null)
                    Eye.GetComponent<Image>().sprite = eyeClose;
                print("Hidden");
            }
            else
            {
                if (Eye != null)
                    Eye.GetComponent<Image>().sprite = eyeOpen;
                print("Exposed");
            }
            hideDebug = hiding;
        }
    }
}
