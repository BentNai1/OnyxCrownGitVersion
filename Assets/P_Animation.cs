using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class P_Animation : MonoBehaviour
{
    private Animator playerAnimator;

    public enum playerAnimationState {idle, walk, crouch, uncrouch };

    public float uncrouchTimer = 0.1f;

    private bool crouchBool;
    private bool walkingBool;

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
            Debug.Log("IdleAnim");
        }
        //walk
        if (anim == playerAnimationState.walk && walkingBool == false)
        {
            playerAnimator.SetBool("isWalking", true);
            walkingBool = true;
            Debug.Log("WalkAnim");
        }

        //crouch
        if (anim == playerAnimationState.crouch && crouchBool == false)
        {
            playerAnimator.SetBool("isCrouching", true);
            crouchBool = true;
            Debug.Log("CrouchAnim");
        }
        //uncrouch
        if (anim == playerAnimationState.uncrouch && crouchBool == true)
        {
            playerAnimator.SetBool("isCrouching", false );
            crouchBool = false;
        }
    }
}
