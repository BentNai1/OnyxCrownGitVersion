using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockNavigation_Script : MonoBehaviour
{
    public GameObject Lock;

    public GameObject Wheel1;
    public GameObject Wheel2;
    public GameObject Wheel3;
    public GameObject Wheel4;

    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;

    public GameObject Selector;

    private bool axisDown = false;
    private int selectNum = 1;

    private bool usingMouse = false;
    
    void Update()
    {
        //Check if player is in range
        if (Lock.GetComponent<CombinationLock>().playerBusy)
        {
            //Switch to mouse input
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                Cursor.lockState = CursorLockMode.None;

                //Show selector arrow
                Selector.GetComponent<Image>().enabled = true;

            //Check if a lock wheel is being moused over
            CheckMouseOver(Wheel1, 1);
            CheckMouseOver(Wheel2, 2);
            CheckMouseOver(Wheel3, 3);
            CheckMouseOver(Wheel4, 4);

            //Check if axis input is being used (keyboard or controller)
            if (Input.GetAxisRaw("Horizontal") != 0 && !axisDown)
            {
                //Switch to controller input
                Cursor.lockState = CursorLockMode.Locked;

                axisDown = true;

                //Determine left or right
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    selectNum++;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    selectNum--;
                }

                UpdateArrow();
            }
            else if (axisDown && Input.GetAxisRaw("Horizontal") == 0)
            {
                axisDown = false;
            }
            
            if (Input.GetButtonDown("Submit"))
            {
                Rotate();
            }
        }
        else
        {
            Selector.GetComponent<Image>().enabled = false;
        }
    }

    private void CheckMouseOver(GameObject wheel, int num)
    {
        if (wheel.GetComponent<CombinationRotate>().mouseOver)
        {
            wheel.GetComponent<CombinationRotate>().mouseOver = false;
            selectNum = num;
            UpdateArrow();
        }
    }

    //Updates selector arrow position
    private void UpdateArrow()
    {
        switch (selectNum)
        {
            case 0: //wrap case
                selectNum = 4;
                Selector.transform.position = pos4.position;
                break;
            case 1:
                Selector.transform.position = pos1.position;
                break;
            case 2:
                Selector.transform.position = pos2.position;
                break;
            case 3:
                Selector.transform.position = pos3.position;
                break;
            case 4:
                Selector.transform.position = pos4.position;
                break;
            case 5: //wrap case
                selectNum = 1;
                Selector.transform.position = pos1.position;
                break;
        }
    }

    private void Rotate()
    {
        switch (selectNum)
        {
            case 1:
                Wheel1.GetComponent<CombinationRotate>().CallRotate();
                break;
            case 2:
                Wheel2.GetComponent<CombinationRotate>().CallRotate();
                break;
            case 3:
                Wheel3.GetComponent<CombinationRotate>().CallRotate();
                break;
            case 4:
                Wheel4.GetComponent<CombinationRotate>().CallRotate();
                break;
        }
    }
}
