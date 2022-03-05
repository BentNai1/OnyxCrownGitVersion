using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelector_Script : MonoBehaviour
{
    public int abilityNum;

    public GameObject CooldownHUD;
    
    void Start()
    {
        abilityNum = 0;
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Selection Next"))
        {
            Cycle();
        }

        if (Input.GetButtonDown("Fire2"))
        {

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
}
