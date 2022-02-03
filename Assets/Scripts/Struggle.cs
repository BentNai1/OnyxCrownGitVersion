using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Struggle : MonoBehaviour
{
    public int numPress;
    public Vector3 offset = new Vector3( 20, 20, 20);

    public GameObject capturePoint;
    public GameObject playerSocket;

    public float speedFromEnemy;

    public CharacterController playerCharacterController;

    public bool isStruggling;

    PlayerMovement playerScript2;

    //Mash Variables
    public float mashDelay = 1f;

    public float mash;
    public bool pressed;

    

    void Start()
    {
        mash = mashDelay;

        playerScript2 = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        
            
        
        if (isStruggling == true)
        {
            
            /**Vector3 diff = transform.TransformDirection(capturePoint.transform.position - transform.position);
            playerCharacterController.transform.TransformDirection(offset);
            playerCharacterController.Move(diff);**/

            if(Input.GetButtonDown("Jump") && !pressed)
            {
                mashDelay += 1f;
                pressed = true;
                mash += .5f;
            }

            else if(Input.GetButtonUp("Jump"))
            {
                pressed = false;
            }

            if (mash >= 7)
            {
                isStruggling = false;
                playerScript2.isGrabbed = false;

            }
        }
    }
}
