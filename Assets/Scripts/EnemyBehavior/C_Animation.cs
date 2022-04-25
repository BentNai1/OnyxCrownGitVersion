using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class C_Animation : MonoBehaviour
{
    private Animator corruptedAnimator;

    public enum corruptedAnimationState { walk, grab, ungrab };

    private bool iswalking;
    private bool isGrabbing;

    void Start()
    {
        corruptedAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void CorruptedAnimation(corruptedAnimationState anim)
    {
        //walk
        if (anim == corruptedAnimationState.walk)
        {
            corruptedAnimator.SetBool("isWalking", true);
            iswalking = true;
        }

        //grabbing
        if(anim == corruptedAnimationState.grab)
        {
            corruptedAnimator.SetBool("isGrabbing", true);
            isGrabbing = true;
        }
        //not grabbing
        if (anim == corruptedAnimationState.ungrab)
        {
            corruptedAnimator.SetBool("isGrabbing", false);
            isGrabbing = false;
        }
    }
}
