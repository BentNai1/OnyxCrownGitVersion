using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Healing_Script : MonoBehaviour
{
    //Reference
    private Player_Health Health;
    private float maxHealth;
    private float currentHealth;

    //Regen
    [Tooltip("Regeneration rate in percent/second")]
    public float regenRate;
    private float regenAmount;
    private float regenThresh;
    private float currentRegen = 0;
    [Tooltip("Delay in seconds after injury before regen continues.")]
    public float healDelay = 0;
    private float lastHealth;
    
    void Start()
    {
        //Initialize variables
        Health = GetComponent<Player_Health>();
        maxHealth = Health.playerMaxHealth;
        lastHealth = maxHealth;

        regenAmount = maxHealth * regenRate / 100; //Calculates health per second

        //If greater than 1 hp/s then player is healed an amount per second
        if (regenAmount >= 1)
        {
            regenThresh = 1;
        }
        //If less than 1 hp/s then player is healed by 1 every amount of seconds
        else
        {
            regenAmount = 1;
            regenThresh = 100 / (maxHealth * regenRate);
        }

    }
    
    void Update()
    {
        if (lastHealth <= Health.playerCurrentHealth && Health.playerCurrentHealth < maxHealth) //Checks if player has not been injured
        {
            currentRegen += Time.deltaTime; //Timer

            //Player is healed every regenThresh seconds
            if (currentRegen >= regenThresh)
            {
                currentRegen -= regenThresh;
                Health.HealDamageOnPlayer(regenAmount);
            }
        }
        else if (lastHealth > Health.playerCurrentHealth) //If player injured, temporary delay is applied to the regen
        {
            print("Player injury detected! Delayed healing by " + healDelay + " seconds.");
            currentRegen -= healDelay;
        }
        
        lastHealth = Health.playerCurrentHealth; //Updates to player's current health
    }
}
