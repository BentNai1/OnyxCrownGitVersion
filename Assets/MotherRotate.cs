using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateMother(float yRotate)
    {
        transform.Rotate(0, yRotate, 0);
    }
}
