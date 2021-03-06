using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector_Script : MonoBehaviour
{
    //UI
    public GameObject Wheel;

    //Ability Scripts
    [HideInInspector]
    public Of_The_Shadows Cloak;
    [HideInInspector]
    public Spectacle_Ability_Script Spectacle;
    [HideInInspector]
    public Song_Of_Sorrows Lyre;

    //Variables
    public int abilityNum;
    private float abilityDuration;
    private float cooldownDuration;

    //Audio
    public AudioSource switchSound;

    void Start()
    {
        Cloak = gameObject.GetComponent<Of_The_Shadows>();
        Spectacle = gameObject.GetComponent<Spectacle_Ability_Script>();
        Lyre = gameObject.GetComponent<Song_Of_Sorrows>();

        abilityNum = 0;
    }
    
    void Update()
    {
        if (!Wheel.GetComponent<Cooldown_Script>().isCooling && Input.GetButtonDown("Fire1"))
        {
            Activate();
        }

        if (!Wheel.GetComponent<Cooldown_Script>().isUsing && Input.GetButtonDown("Fire2"))
        {
            Cycle();
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

        //Apply change to UI
        Wheel.GetComponent<AbilityWheel_Script>().UpdateUI(abilityNum);

        switchSound.Play();
    }

    private void Activate()
    {
        //Activate ability based on number
        switch (abilityNum)
        {
            //Cloak
            case 0:
                Cloak.Activate();
                abilityDuration = Cloak.abilityDuration;
                cooldownDuration = Cloak.coolDownDuration;
                print("Cloak ability activated");
                break;

            //Spectacle
            case 1:
                Spectacle.Activate();
                abilityDuration = Spectacle.abilityDuration;
                cooldownDuration = Spectacle.cooldownDuration;
                print("Spectacle ability activated");
                break;

            //Lyre
            case 2:
                Lyre.Activate();
                abilityDuration = Lyre.abilityDuration;
                cooldownDuration = Lyre.coolDownDuration;
                print("Lyre ability activated");
                break;
        }
        Wheel.GetComponent<Cooldown_Script>().Cooldown(abilityDuration, cooldownDuration);
    }
}
