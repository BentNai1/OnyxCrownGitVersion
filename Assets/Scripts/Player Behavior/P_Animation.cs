using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class P_Animation : MonoBehaviour
{
    private Animator playerAnimator;

    public enum playerAnimationState {idle, walk, crouch, uncrouch, playerGrabbed };

    public float uncrouchTimer = 0.1f;

    private bool crouchBool;
    private bool walkingBool;
    private bool isGrabbed;

    [SerializeField] private float playerAnimationMultiplier = 1.0f;

    private float animationSpeedFromPlayerSpeed;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    //call animation to play from other scripts (playerAnimationState.idle, for example)
    public void PlayerAnimation(playerAnimationState anim)
    {

        //idle
        if(anim == playerAnimationState.idle && walkingBool == true)
        {
            playerAnimator.SetBool("isWalking", false);
            walkingBool = false;
            isGrabbed = false;
            playerAnimator.speed = 1f;
        }
        //walk
        if (anim == playerAnimationState.walk && walkingBool == false)
        {
            playerAnimator.SetBool("isWalking", true);
            walkingBool = true;
            isGrabbed = false;
            playerAnimator.speed = animationSpeedFromPlayerSpeed;
        }

        //crouch
        if (anim == playerAnimationState.crouch && crouchBool == false)
        {
            playerAnimator.SetBool("isCrouching", true);
            crouchBool = true;
        }
        //uncrouch
        if (anim == playerAnimationState.uncrouch && crouchBool == true)
        {
            playerAnimator.SetBool("isCrouching", false );
            crouchBool = false;
        }
        //player grabbed
        if (anim == playerAnimationState.playerGrabbed && !isGrabbed)
        {
            playerAnimator.SetBool("isBeingGrabbed", true);
            isGrabbed = true;
        }else playerAnimator.SetBool("isBeingGrabbed", false);
    }


    //to change walk animation speed based on player input
    public void changeMovementAnimSpeed(float speedInput)
    {
        if (Mathf.Abs(speedInput*playerAnimationMultiplier-animationSpeedFromPlayerSpeed) >= 0.02f )
        {
            animationSpeedFromPlayerSpeed = speedInput;

            if(walkingBool == true)
            {
                playerAnimator.speed = animationSpeedFromPlayerSpeed;
            }
        }
    }
}
