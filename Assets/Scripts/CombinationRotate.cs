using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombinationRotate : MonoBehaviour
{
    public static event Action<string, int> Rotated = delegate { };
    private bool coroutineAllowed;
    private int numShown;
     
    private void Start()
    {
        coroutineAllowed = true;
        numShown = 1;
    }
    //Gets the input of the mouse for each wheel
    private void OnMouseDown()
    {
        if(coroutineAllowed)
        {
            StartCoroutine("RotateWheel");
        }
    }

    /**Coroutine that will rotate the wheel 30 degrees, and makes it so that the player cannot spam spin the wheel.
     * Sends the positional value gotten from the rotation to the CombinationLock script to check the values.**/
    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;

        for(int i = 0; i <= 11; i++)
        {
            transform.Rotate(0f, 0f, -3f);
            yield return new WaitForSeconds(0.01f);
        }

        coroutineAllowed = true;

        numShown +=1;

        if(numShown > 9)
        {
            numShown = 0;
        }

        Rotated(name, numShown);
    }
}
