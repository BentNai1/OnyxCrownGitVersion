using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Patrol : MonoBehaviour
{

    public Transform[] points;
    int current;
    public float speed;

   


    void Start()
    {
        current = 0;
    }


    private void Update()
    {

        if (Mathf.Round(transform.position.x) != Mathf.Round(points[current].position.x) || Mathf.Round(transform.position.z) != Mathf.Round(points[current].position.z))
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
        }
        else    
            current = (current + 1) % points.Length;

    }

}
