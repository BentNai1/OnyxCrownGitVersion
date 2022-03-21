using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherRotate : MonoBehaviour
{
    public void RotateMother(float yRotate)
    {
        transform.Rotate(0, yRotate, 0);
    }
}
