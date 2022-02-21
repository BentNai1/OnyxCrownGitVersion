using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Player_Health : MonoBehaviour
{
    //................................................VARIABLES
    [Tooltip("Recommend 3")]
    [SerializeField] public float playerMaxHealth;
    [SerializeField] private int regenRate = 1;//how fast we regenerate health
    private bool canRegen = false;

    [Tooltip("This variable can be used by outside scripts")]
    public float playerCurrentHealth;

    //private float playerCurrentHealth;

    [HideInInspector]
    public bool isFullHealth = true;

    [Tooltip("Recommend 1")]
    public float defaultDamage;

    [Tooltip("Recommend 1")]
    public float defaultHealing;

    [SerializeField] private Image bloodSplatterImage = null;
    [SerializeField] private Image hurtImage = null;
    [SerializeField] private float hurtTimer = 0.1f;

    [SerializeField] private float healCooldown = 3.0f;
    [SerializeField] private float maxHealCooldown = 3.0f;
    [SerializeField] private bool startCooldown = false; 


    //................................................START
    void Start()
    {
        //set healths to max on start
        playerCurrentHealth = playerMaxHealth;
        playerCurrentHealth = playerCurrentHealth;
    }

    //DEBUG damage self
    private void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            DealDamageToPlayer(1);
        }
        if (Input.GetKeyDown("j"))
        {
            HealDamageOnPlayer(1);
        }
        
        //Check if full health
        if (playerCurrentHealth == playerMaxHealth) isFullHealth = true;
        else isFullHealth = false;

        //cooldown to start regen
        if (startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }

        }
                //regenerate health
            if (canRegen)
            {
                if (playerCurrentHealth <= playerMaxHealth -0.01)
                {
                    playerCurrentHealth += Time.deltaTime * regenRate;
                    UpdateHealth();
                }
                else
                {
                    playerCurrentHealth = playerMaxHealth;
                    healCooldown = maxHealCooldown;
                    canRegen = false;
                }
            }

    }

    //Blood splatter flashes when taking damage

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }

    //updates image alpha i.e turns it on/off
    void UpdateHealth()
    {
     
       Color splatterAlpha = bloodSplatterImage.color;
       splatterAlpha.a = 1 - (playerCurrentHealth / playerMaxHealth);
       bloodSplatterImage.color = splatterAlpha;

    }



    //................................................DAMAGE FUNCTION
    public void DealDamageToPlayer(float amount)
    {
        //if no damage submitted, change damage to default
        if(amount == 0)
        {
            amount = defaultDamage;
        }

        //deal damage if positive number
        if(playerCurrentHealth > 0 && amount > 0)
        {
            playerCurrentHealth -= amount;
            print(amount + " damage taken! Player now has " + playerCurrentHealth + " health remaining out of " + playerMaxHealth);


            canRegen = false;
            StartCoroutine(HurtFlash());
            UpdateHealth();
            healCooldown = maxHealCooldown;
            startCooldown = true;

        }

        //heal if negative number
        else if (playerCurrentHealth < playerMaxHealth && amount <= 0)
        {
            if (amount < playerCurrentHealth - playerMaxHealth)
            {
                print(amount + "(inverted) is too much healing! Value reduced.");
                amount = amount + (playerMaxHealth - playerCurrentHealth);
            }
            playerCurrentHealth -= amount;
            print(amount + "(inverted) healing received! Player now has " + playerCurrentHealth + " health remaining out of " + playerMaxHealth + "(did you mean to use HealDamageOnPlayer?)");
        }

        else
        {
            print("Error! Cannot apply damage to player. Player has " + playerCurrentHealth + " health out of " + playerMaxHealth + ", and the value of " + amount + " cannot be applied!");
        }

        //set externally accessible variable to internal variable
        //playerCurrentHealth = playerCurrentHealth;
    }

    //................................................HEAL FUNCTION
    public void HealDamageOnPlayer(float amount)
    {
        //if no healing submitted, change healing to default
        if (amount == 0)
        {
            amount = defaultHealing;
        }

        //heal if positive number
        if (playerCurrentHealth < playerMaxHealth && amount > 0)
        {
            if(amount > playerMaxHealth-playerCurrentHealth)
            {
                print(amount + " is too much healing! Value reduced.");
                amount = amount - (playerMaxHealth - playerCurrentHealth);
            }
            playerCurrentHealth += amount;
            print(amount + " healing received! Player now has " + playerCurrentHealth + " health remaining out of " + playerMaxHealth);

        }

        //damage if negative number
        else if (playerCurrentHealth > 0 && amount <= 0)
        {
            playerCurrentHealth += amount;
            print(amount + "(inverted) damage taken! Player now has " + playerCurrentHealth + " health remaining out of " + playerMaxHealth + "(did you mean to use DealDamageToPlayer?)");
        }

        else
        {
            print("Error! Cannot apply healing to player. Player has " + playerCurrentHealth + " health out of " + playerMaxHealth + ", and the value of " + amount + " cannot be applied!");
        }

        //set externally accessible variable to internal variable
        //playerCurrentHealth = playerCurrentHealth;
    }
}

