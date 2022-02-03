using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mana : MonoBehaviour
{
    public float playerMaxMana;
    [HideInInspector]
    public float playerCurrentMana;
    public Manabar_Script Bar;

    private void Start()
    {
        //Find
        Bar = FindObjectOfType<Manabar_Script>();
        //Full mana
        playerCurrentMana = playerMaxMana;
    }

    private void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            ConsumeMana(5f);
        }
        if (Input.GetKeyDown("b"))
        {
            ReplenishMana(5f);
        }
    }

    //Returns false if mana is insufficient, otherwise it is drained by the specified amount
    public bool ConsumeMana(float amount)
    {
        //Give warning if value negative
        if (amount < 0)
        {
            print("WARNING: Please use positive value for mana consumption.");
        }

        if (amount > playerCurrentMana)
        {
            Bar.NotEnoughMana();
            return false;
        }
        else
        {
            playerCurrentMana -= amount;
            if (playerCurrentMana < 0)
            {
                print("WARNING: Mana overconsumed!");
                playerCurrentMana = 0;
            }
            print(playerCurrentMana);
            return true;
        }
    }

    //Returns false if mana is full, otherwise adds mana by the specified amount
    public bool ReplenishMana(float amount)
    {
        //Give warning if value negative
        if (amount < 0)
        {
            print("WARNING: Please use positive value for mana replenish.");
        }

        if (playerCurrentMana == playerMaxMana)
        {
            return false;
        }
        else
        {
            playerCurrentMana += amount;
            if (playerCurrentMana > playerMaxMana)
            {
                playerCurrentMana = playerMaxMana;
            }
            print(playerCurrentMana);
            return true;
        }
    }
}
