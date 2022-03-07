using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector_Script : MonoBehaviour
{
    //UI
    public GameObject CooldownUI;
    public GameObject SelectorUI;

    //Ability Scripts
    public Of_The_Shadows Cloak;
    public Spectacle_Ability_Script Spectacle;
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
        abilityNum++;
            switch (abilityNum)
            {
                case 0:
                    print("Cloak ability selected");
                    break;

                case 1:
                    print("Spectacle ability selected");
                    break;

                case 2:
                    print("Lyre ability selected");
                    break;

                case 3:
                    abilityNum = 0;
                    print("Cloak ability selected");
                    break;
            }
    }

    private void Activate()
    {
        switch (abilityNum)
        {
            case 0:
                Cloak.Activate();
                print("Cloak ability activated");
                break;

            case 1:
                Spectacle.Activate();
                print("Spectacle ability activated");
                break;

            case 2:
                Lyre.Activate();
                print("Lyre ability activated");
                break;
        }
    }
}
