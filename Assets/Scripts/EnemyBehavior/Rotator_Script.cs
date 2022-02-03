using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator_Script : MonoBehaviour
{
    void LateUpdate()
    {
        transform.Rotate(Random.Range(0, 1f), Random.Range(-5f, 5f), Random.Range(-5f, 5f), Space.World);
    }
}
