using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    private void Start()
    {
        playerSpeed = GetComponent<PlayerMovement>().moveSpeed;
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
            }
            else
            {
                hiding = false;
            }
        }
        else
        {
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

        //DEBUG HIDING MESSAGES
        if (hideDebug != hiding)
        {
            if (hiding)
            {
                print("Hidden");
            }
            else
            {
                print("Exposed");
            }
            hideDebug = hiding;
        }
    }
}
