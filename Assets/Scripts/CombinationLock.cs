using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationLock : MonoBehaviour
{
    private int[] result, answer;

    void Start()
    {
        result = new int[] { 5, 5, 5, 5};
        answer = new int[] { 1, 2, 3, 4 };
        CombinationRotate.Rotated += CheckResults;
    }

    private void CheckResults(string wheelname, int number)
    {
        switch (wheelname)
        {
            case "Wheel1":
                result[0] = number;
                break;

            case "wheel2":
                result[1] = number;
                break;

            case "wheel3":
                result[2] = number;
                break;

            case "wheel4":
                result[3] = number;
                break;
        }

        if (result[0] == answer[0] && result[1] == answer[1] && result[2] == answer[2] && result[3] == answer[3])
        {
            Debug.Log("Gate Opened!");
        }
    }

    private void OnDestroy()
    {
        CombinationRotate.Rotated -= CheckResults;
    }
}
