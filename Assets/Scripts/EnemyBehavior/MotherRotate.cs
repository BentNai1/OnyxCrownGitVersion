using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script exists only because Unity wont let me rotate this gameobject from an outside script while it has a character controller.
public class MotherRotate : MonoBehaviour
{
    public void RotateMother(float yRotate)
    {
        transform.Rotate(0, yRotate, 0);
    }
}
