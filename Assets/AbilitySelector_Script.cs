using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector_Script : MonoBehaviour
{
    //UI
    //public GameObject CooldownUI;
    //public GameObject SelectorUI;

    //Ability Scripts
    [HideInInspector]
    public Of_The_Shadows Cloak;
    [HideInInspector]
    public Spectacle_Ability_Script Spectacle;
    [HideInInspector]
    public Song_Of_Sorrows Lyre;

    //Variables
    private int abilityNum;

    void Start()
    {
        Cloak = gameObject.GetComponent<Of_The_Shadows>();
        Spectacle = gameObject.GetComponent<Spectacle_Ability_Script>();
        Lyre = gameObject.GetComponent<Song_Of_Sorrows>();

        abilityNum = 0;
    }
    
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Cycle();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Activate();
        }
    }

    private void Cycle()
    {
        //Change ability number by 1
        abilityNum++;

        //Tell ability based on number
        switch (abilityNum)
        {
            //Cloak
            case 0:
                print("Cloak ability selected");
                break;

            //Spectacle
            case 1:
                print("Spectacle ability selected");
                break;

            //Lyre
            case 2:
                print("Lyre ability selected");
                break;

            //Loop number to start
            case 3:
                abilityNum = 0;
                print("Cloak ability selected");
                break;
            }
    }

    private void Activate()
    {
        //Activate ability based on number
        switch (abilityNum)
        {
            //Cloak
            case 0:
                Cloak.Activate();
                print("Cloak ability activated");
                break;

            //Spectacle
            case 1:
                Spectacle.Activate();
                print("Spectacle ability activated");
                break;

            //Lyre
            case 2:
                Lyre.Activate();
                print("Lyre ability activated");
                break;
        }
    }
}
