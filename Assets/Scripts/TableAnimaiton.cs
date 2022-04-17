using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAnimaiton : MonoBehaviour
{
    public Animator anim;

    public Corrupted_Script check;
    // Start is called before the first frame update
    void Start()
    {
        if (check == null) check = GameObject.Find("Enemy Model").GetComponent<Corrupted_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if(check.enemyRanIntoTable == true)
        {
            anim.SetBool("flipped", true);
        }
    }
}
