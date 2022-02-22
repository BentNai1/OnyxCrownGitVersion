using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class P_Animation : MonoBehaviour
{
    private Animator playerAnimator;

    public enum playerAnimationState {idle, walk };


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    //call animation to play from other scripts (playerAnimationState.idle, for example)
public void PlayerAnimation(playerAnimationState anim)
    {
        if(anim == playerAnimationState.idle)
        {
            playerAnimator.SetBool("isWalking", false);
        }

        if(anim == playerAnimationState.walk)
        {
            playerAnimator.SetBool("isWalking", true);
        }
    }
}
