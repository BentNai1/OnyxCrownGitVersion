using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombinationRotate : MonoBehaviour
{
    public static event Action<string, int> Rotated = delegate { };
    private bool coroutineAllowed;
    private int numShown;

    [HideInInspector]
    public bool mouseOver = false;

    [Header("- Sound")]
    [SerializeField] private AudioSource lockSpeaker;
    [SerializeField] private AudioClip combinationSound;

    private void Start()
    {
        coroutineAllowed = true;
        numShown = 9;
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    //Gets the input of the mouse for each wheel
    private void OnMouseDown()
    {
        CallRotate();
        mouseOver = true;
    }

    public void CallRotate()
    {
        if (coroutineAllowed)
        {
            StartCoroutine("RotateWheel");
        }
    }

    /**Coroutine that will rotate the wheel 30 degrees, and makes it so that the player cannot spam spin the wheel.
     * Sends the positional value gotten from the rotation to the CombinationLock script to check the values.**/
    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;

        lockSpeaker.PlayOneShot(combinationSound);

        for(int i = 0; i <= 11; i++)
        {
            //Yess it have to be that long to keep it rotating accuratley
            transform.Rotate(0f, 0f, -3.333333333333333f);
            yield return new WaitForSeconds(0.01f);
        }


        coroutineAllowed = true;

        numShown +=1;

        if(numShown > 9)
        {
            numShown = 1;
        }
        Debug.Log(numShown);

        Rotated(name, numShown);
    }
}
