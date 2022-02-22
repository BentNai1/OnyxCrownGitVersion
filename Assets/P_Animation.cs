using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class P_Animation : MonoBehaviour
{
    private Animator playerAnimator;

    public enum playerAnimationState {idle, walk, crouch };

    public float uncrouchTimer = 0.1f;

    private bool crouchLock;

    private float timer;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //timer to keep animation from rapidly switching
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    //call animation to play from other scripts (playerAnimationState.idle, for example)
    public void PlayerAnimation(playerAnimationState anim)
    {
        if (anim == playerAnimationState.crouch)
        {
            crouchLock = true;
            timer = uncrouchTimer;
        }
        if (timer <= 0)
        {
            crouchLock = false;
        }

        //idle stand/crouch
        if(anim == playerAnimationState.idle && crouchLock == false)
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isCrouching", false);
        }

        if (anim == playerAnimationState.idle && crouchLock == true)
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isCrouching", true);
        }

        //walk stand/crouch
        if (anim == playerAnimationState.walk && crouchLock == false)
        {
            playerAnimator.SetBool("isWalking", true);
            playerAnimator.SetBool("isCrouching", false);
        }

        if (anim == playerAnimationState.walk && crouchLock == true)
        {
            playerAnimator.SetBool("isWalking", true);
            playerAnimator.SetBool("isCrouching", true);
        }
    }
}
