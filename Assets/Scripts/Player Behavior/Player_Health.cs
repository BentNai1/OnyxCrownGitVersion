using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Player_Health : MonoBehaviour
{
    //................................................VARIABLES
    [Tooltip("Recommend 3")]
    [SerializeField] public float playerMaxHealth;
    
    [Tooltip("This variable can be used by outside scripts")]
    public float playerCurrentHealth;

    private float privateCurrentHealth;

    [HideInInspector]
    public bool isFullHealth = true;

    [Tooltip("Recommend 1")]
    public float defaultDamage;

    [Tooltip("Recommend 1")]
    public float defaultHealing;

    [SerializeField] private Image bloodSplatterImage = null;
    [SerializeField] private Image hurtImage = null;
    [SerializeField] private float hurtTimer = 0.1f;


    //................................................START
    void Start()
    {
        //set healths to max on start
        privateCurrentHealth = playerMaxHealth;
        playerCurrentHealth = privateCurrentHealth;
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
        if (privateCurrentHealth == playerMaxHealth) isFullHealth = true;
        else isFullHealth = false;
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
        if(privateCurrentHealth > 0 && amount > 0)
        {
            privateCurrentHealth -= amount;
            print(amount + " damage taken! Player now has " + privateCurrentHealth + " health remaining out of " + playerMaxHealth);

            StartCoroutine(HurtFlash());
            UpdateHealth();
          


        }

        //heal if negative number
        else if (privateCurrentHealth < playerMaxHealth && amount <= 0)
        {
            if (amount < privateCurrentHealth - playerMaxHealth)
            {
                print(amount + "(inverted) is too much healing! Value reduced.");
                amount = amount + (playerMaxHealth - privateCurrentHealth);
            }
            privateCurrentHealth -= amount;
            print(amount + "(inverted) healing received! Player now has " + privateCurrentHealth + " health remaining out of " + playerMaxHealth + "(did you mean to use HealDamageOnPlayer?)");
        }

        else
        {
            print("Error! Cannot apply damage to player. Player has " + privateCurrentHealth + " health out of " + playerMaxHealth + ", and the value of " + amount + " cannot be applied!");
        }

        //set externally accessible variable to internal variable
        playerCurrentHealth = privateCurrentHealth;
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
        if (privateCurrentHealth < playerMaxHealth && amount > 0)
        {
            if(amount > playerMaxHealth-privateCurrentHealth)
            {
                print(amount + " is too much healing! Value reduced.");
                amount = amount - (playerMaxHealth - privateCurrentHealth);
            }
            privateCurrentHealth += amount;
            print(amount + " healing received! Player now has " + privateCurrentHealth + " health remaining out of " + playerMaxHealth);

        }

        //damage if negative number
        else if (privateCurrentHealth > 0 && amount <= 0)
        {
            privateCurrentHealth += amount;
            print(amount + "(inverted) damage taken! Player now has " + privateCurrentHealth + " health remaining out of " + playerMaxHealth + "(did you mean to use DealDamageToPlayer?)");
        }

        else
        {
            print("Error! Cannot apply healing to player. Player has " + privateCurrentHealth + " health out of " + playerMaxHealth + ", and the value of " + amount + " cannot be applied!");
        }

        //set externally accessible variable to internal variable
        playerCurrentHealth = privateCurrentHealth;
    }
}

